using System;
using UnityEngine;
using UnityEngine.UI;

public class DeployableTaskEntryDeployedController : MonoBehaviour, ITaskEntryController {

    public Text TaskTypeText;
    public Text TaskNameText;
    public Text SatisfactionText;
    public Text DefectText;
    public Text QualityText;
    public ITask TaskBeingDisplayed { get; set; } = null;

    private float displayingSatisfaction = 0;

    void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            // To be implemented
        });
        UpdateTaskAfterDayTick();
        GameManager.GetInstance.AfterDayTick += AfterDayTick;
        if (IsDebugging) {
            Debug.LogWarning("Debug mode activated: Product quality will always be shown");
        }
    }

    private void AfterDayTick(object sender, EventArgs e) {
        UpdateTaskAfterDayTick();
    }

    void OnDestroy() {
        GameManager.GetInstance.AfterDayTick -= AfterDayTick;
    }

    public void Update() {
        if (TaskBeingDisplayed == null) return;
        TaskTypeText.text = "Deployed";
        TaskNameText.text = TaskBeingDisplayed.Name;
        if (!(TaskBeingDisplayed is DeployableTask deployableTask)) return;
        DefectText.text = $"Defects: {deployableTask.ProductionDefectCount}";
        displayingSatisfaction = Utils.MoveTowardsProportion(
            displayingSatisfaction,
            deployableTask.TotalSatisfaction,
            2f,
            Time.deltaTime / 0.15f
        );
        SatisfactionText.text = $"Satisfaction: {displayingSatisfaction:0.0}";
    }

    private void UpdateTaskAfterDayTick() {
        if (!(TaskBeingDisplayed is DeployableTask task)) return;
        if (GameManager.GetInstance.DisplayEndProductQuality || IsDebugging) {
            QualityText.text = $"Quality: {Mathf.RoundToInt(task.EndProductQuality * 100)}/100";
        } else {
            QualityText.text = $"Quality: ???/100";
        }
        if (IsDebugging) {
            QualityText.text += "\n(Debug mode)";
        }
    }

    private bool IsDebugging => GameManager.GetInstance.IsDebugging;
}
