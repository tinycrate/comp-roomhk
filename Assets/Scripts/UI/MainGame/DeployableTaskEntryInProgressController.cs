using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DeployableTaskEntryInProgressController : MonoBehaviour, ITaskEntryController {

    public Text TaskTypeText;
    public Text TaskNameText;
    public Text LeadTimeText;
    public Text CompletionText;
    public Text StatusText;
    public ITask TaskBeingDisplayed { get; set; } = null;

    private float displayingCompletionRate = 0;

    void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            MainGameSceneManager.GetInstance.ShowTaskProgress(TaskBeingDisplayed);
        });
        UpdateTaskStatus();
        GameManager.GetInstance.AfterDayTick += AfterDayTick;
    }

    private void AfterDayTick(object sender, EventArgs e) {
        UpdateTaskStatus();
    }

    void OnDestroy() {
        GameManager.GetInstance.AfterDayTick -= AfterDayTick;
    }

    public void Update() {
        if (TaskBeingDisplayed == null) return;
        TaskTypeText.text = "In Progress";
        TaskNameText.text = TaskBeingDisplayed.Name;
        if (!(TaskBeingDisplayed is DeployableTask deployableTask)) return;
        LeadTimeText.text = $"Lead time: {Mathf.RoundToInt(deployableTask.LeadTime)} day(s)";
        displayingCompletionRate = Utils.MoveTowardsProportion(
            displayingCompletionRate,
            deployableTask.FeatureCompletePercentage,
            0.02f,
            Time.deltaTime / 0.15f
        );
        CompletionText.text = $"Completion: {displayingCompletionRate*100:0.#}%";
    }

    private void UpdateTaskStatus() {
        // Very dirty status
        if (!(TaskBeingDisplayed is DeployableTask task)) return;
        if (!Mathf.Approximately(task.ReleaseEffort, task.RemainingReleaseEffort)) {
            StatusText.text = "Status: Deploying";
            return;
        }
        if (task.Features.Any(
            x => x.CurrentState == Feature.State.Coding || x.CurrentState == Feature.State.BugFixing
        )) {
            StatusText.text = "Status: Coding";
            return;
        }
        if (task.Features.Any(x => x.CurrentState == Feature.State.Testing)) {
            StatusText.text = "Status: Testing";
            return;
        }
        if (task.Features.Any(x => x.CurrentState == Feature.State.Idle)) {
            StatusText.text = "Status: Idle";
            return;
        }
    }
}
