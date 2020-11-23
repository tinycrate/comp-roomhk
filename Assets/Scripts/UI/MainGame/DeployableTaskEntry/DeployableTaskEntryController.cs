using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeployableTaskEntryController : MonoBehaviour, ITaskEntryController {

    public Text TaskTypeText;
    public Text TaskNameText;
    public Text EffortText;
    public Text DifficultyText;
    public Text ValueText;

    public void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            MainGameSceneManager.GetInstance.ShowTaskPlanning(TaskBeingDisplayed);
        });
    }

    private ITask taskBeingDisplayed = null;
    public ITask TaskBeingDisplayed {
        get => taskBeingDisplayed;
        set {
            taskBeingDisplayed = value;
            if (value == null) return;
            TaskTypeText.text = "Develop";
            TaskNameText.text = value.Name;
            if (value is DeployableTask deployableTask) {
                EffortText.text = $"Total Effort: {Mathf.RoundToInt(deployableTask.TotalFeatureEffort)}";
                var averageDifficulty = deployableTask.Features.Sum(x => x.Difficulty) / deployableTask.Features.Count;
                DifficultyText.text = $"Avg. Difficulty: {averageDifficulty:0.##}";
                ValueText.text = $"Satisfaction: {deployableTask.Value}/day";
            } else {
                Debug.LogWarning(
                    "DeployableTaskEntryController is getting a task that is not a DeployableTask (Which it should)"
                );
            }
        }
    }
}
