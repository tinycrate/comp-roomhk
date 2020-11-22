using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class StatisticsController : MonoBehaviour, IMainGameToggleableView {

    public Dropdown OptionDropdown;
    public LineChart Chart;
    public Button MetricsShowButton;

    private Animator animator;
    public Animator Animator {
        get {
            if (animator == null) return animator = GetComponent<Animator>();
            return animator;
        }
    }

    private readonly Dictionary<string, List<float>> metrics = new Dictionary<string, List<float>>();

    private readonly List<PropertyInfo> properties = typeof(GameStatSnapshot)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(x => x.PropertyType == typeof(int) || x.PropertyType == typeof(float)).ToList();

    // Start is called before the first frame update
    void Start() {
        MainGameSceneManager.GetInstance.RegisterToggleableView(this);
        OptionDropdown.ClearOptions();
        UpdateMetrics();
        GameManager.GetInstance.AfterDayTick += AfterDayTick;
        OptionDropdown.onValueChanged.AddListener((selection) => { UpdateChart(); });
    }

    void OnDestroy() {
        GameManager.GetInstance.AfterDayTick -= AfterDayTick;
    }

    public bool IsShowing => Animator.GetCurrentAnimatorStateInfo(0).IsName("Showing");

    public void ToggleDisplay() {
        transform.SetAsLastSibling();
        Animator.SetTrigger("ToggleDisplay");
        if (IsShowing) {
            Chart.AnimationFadeOut();
        } else {
            Chart.AnimationReset();
            Chart.AnimationFadeIn();
        }
    }

    // Very dirty
    private readonly Dictionary<string, string> metricNameAlias = new Dictionary<string, string>() {
        {"MTTR", "MTTR (Hours)"},
        {"MTTF", "MTTF (Days)"},
        {"Production Availability", "Production Availability (%)"},
        {"Average Lead Time", "Average Lead Time (Days)"},
        {"Deployment Frequency", "Deployment Frequency (Deployment/Week)"},
    };

    private void UpdateChart() {
        var selectedMetric = OptionDropdown.options[OptionDropdown.value].text;
        if (!metrics.TryGetValue(selectedMetric, out var values)) {
            Debug.LogWarning($"Statistics: Unable to get metrics for {selectedMetric}");
            return;
        }
        Chart.yAxis0.minMaxType = Axis.AxisMinMaxType.MinMax;
        Chart.yAxis0.min = 0;
        Chart.yAxis0.max = 0;
        // Very dirty way of changing scaling
        var modifier = 1;
        if (selectedMetric.Contains("Percentage") || selectedMetric.Contains("Knowledge")) {
            modifier = 100;
            Chart.yAxis0.minMaxType = Axis.AxisMinMaxType.Custom;
            Chart.yAxis0.max = 100f;
        }
        if (selectedMetric == "MTTR") {
            modifier = 24;
        }
        if (selectedMetric == "Deployment Frequency") {
            modifier = 7;
        }
        if (selectedMetric == "Production Availability") {
            Chart.yAxis0.minMaxType = Axis.AxisMinMaxType.Custom;
            Chart.yAxis0.min = 0.96f;
            Chart.yAxis0.max = 1f;
        }
        if (metricNameAlias.TryGetValue(selectedMetric, out var alias)) {
            Chart.title.text = alias;
        } else {
            Chart.title.text = selectedMetric;
        }
        Chart.ClearData();
        for (var i = 0; i < values.Count; i++) {
            var x = values[i];
            Chart.AddData(0, i, x * modifier);
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
