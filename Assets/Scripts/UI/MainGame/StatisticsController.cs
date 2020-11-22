using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class StatisticsController : MonoBehaviour {

    public Dropdown OptionDropdown;
    public LineChart Chart;
    public Button MetricsShowButton;

    private readonly Dictionary<string, List<float>> metrics = new Dictionary<string, List<float>>();

    private readonly List<PropertyInfo> properties = typeof(GameStatSnapshot)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(x => x.PropertyType == typeof(int) || x.PropertyType == typeof(float)).ToList();

    // Start is called before the first frame update
    void Start() {
        OptionDropdown.ClearOptions();
        UpdateMetrics();
        GameManager.GetInstance.AfterDayTick += AfterDayTick;
        OptionDropdown.onValueChanged.AddListener((selection) => { UpdateChart(); });
    }

    void OnDestroy() {
        GameManager.GetInstance.AfterDayTick -= AfterDayTick;
    }

    public void ToggleDisplay() {
        var animator = GetComponent<Animator>();
        animator.SetTrigger("ToggleDisplay");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Showing")) {
            Chart.AnimationFadeOut();
        } else {
            Chart.AnimationReset();
            Chart.AnimationFadeIn();
        }
    }

    private void UpdateChart() {
        var selectedMetric = OptionDropdown.options[OptionDropdown.value].text;
        if (!metrics.TryGetValue(selectedMetric, out var values)) {
            Debug.LogWarning($"Statistics: Unable to get metrics for {selectedMetric}");
            return;
        }
        Chart.title.text = selectedMetric;
        Chart.ClearData();
        for (var i = 0; i < values.Count; i++) {
            var x = values[i];
            Chart.AddData(0, i, x);
        }
    }

    private void UpdateMetrics() {
        metrics.Clear();
        if (GameManager.GetInstance.StatManager == null) {
            MetricsShowButton.interactable = false;
            return;
        }
        MetricsShowButton.interactable = true;
        foreach (var property in properties) {
            var values = new List<float>();
            foreach (var snapshot in GameManager.GetInstance.StatManager.Snapshots) {
                values.Add((float) Convert.ChangeType(property.GetValue(snapshot), typeof(float)));
            }
            metrics.Add(ConvertCamelCaseName(property.Name), values);
        }
        metrics.Add("Testing Knowledge", new List<float>());
        metrics.Add("Operation Knowledge", new List<float>());
        metrics.Add("Release Engineering Knowledge", new List<float>());
        metrics.Add("Automation Knowledge", new List<float>());
        metrics.Add("DevOps Knowledge", new List<float>());
        foreach (var knowledge in GameManager.GetInstance.StatManager.Snapshots.Select(snapshot => snapshot.TeamKnowledgeSnapshot)) {
            metrics["Testing Knowledge"].Add(knowledge.TeamTestingKnowledge);
            metrics["Operation Knowledge"].Add(knowledge.TeamOperationKnowledge);
            metrics["Release Engineering Knowledge"].Add(knowledge.TeamReleaseEngKnowledge);
            metrics["Automation Knowledge"].Add(knowledge.TeamAutomationKnowledge);
            metrics["DevOps Knowledge"].Add(knowledge.DevOpsKnowledge);
        }
        if (!metrics.Keys.SequenceEqual(OptionDropdown.options.Select(x => x.text))) {
            OptionDropdown.ClearOptions();
            OptionDropdown.AddOptions(metrics.Keys.ToList());
        }
    }

    private void AfterDayTick(object sender, EventArgs args) {
        UpdateMetrics();
        UpdateChart();
    }

    private readonly Dictionary<string, string> convertCamelCaseNameCache = new Dictionary<string, string>();
    private string ConvertCamelCaseName(string name) {
        if (convertCamelCaseNameCache.TryGetValue(name, out var convertedName)) return convertedName;
        convertedName = Utils.SplitCamelCase(name);
        convertCamelCaseNameCache[name] = convertedName;
        return convertedName;
    }
}
