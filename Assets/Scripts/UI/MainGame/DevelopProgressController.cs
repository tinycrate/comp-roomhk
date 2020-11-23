using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DevelopProgressController : MonoBehaviour, IMainGameView {
    [Header("Object Reference Prefixes")] 
    public GameObject TextLocation;
    public string CodeTextObjectPrefix = "code_";
    public string BuildTextObjectPrefix = "build_";
    public string TestTextObjectPrefix = "test_";
    public string MergeTextObjectPrefix = "merge_";
    
    [Header("Object References")]
    public Image ReleaseBar;
    public Text DeployMessage;
    public Text TaskNameText;

    private readonly Dictionary<string, Text> textObjectCache = new Dictionary<string, Text>();
    private readonly Dictionary<Text, float> textDisplayValues = new Dictionary<Text, float>();
    private float progressBarDisplayValue = 0f;

    public Animator Animator => GetComponent<Animator>();
    public GameObject CurrentGameObject => gameObject;

    private ITask displayingTask;

    public ITask DisplayingTask {
        get => displayingTask;
        set {
            if (displayingTask != null) {
                if (displayingTask is DeployableTask task){
                    task.OnDeployed -= OnSuccessfulDeployment;
                    task.OnDeploymentFailure -= OnDeploymentFailure;
                }
            }
            displayingTask = value;
            TaskNameText.text = displayingTask.Name;
            if (value is DeployableTask newTask){
                newTask.OnDeployed += OnSuccessfulDeployment;
                newTask.OnDeploymentFailure += OnDeploymentFailure;
            }
        }
    }

    public void Update() {
        if (DisplayingTask == null) return;
        for (var i = 0; i < DisplayingTask.Features.Count; i++) {
            var feature = DisplayingTask.Features[i];
            var codeText = TryGetText($"{CodeTextObjectPrefix}{i}");
            var buildText = TryGetText($"{BuildTextObjectPrefix}{i}");
            var testText = TryGetText($"{TestTextObjectPrefix}{i}");
            var mergeText = TryGetText($"{MergeTextObjectPrefix}{i}");
            if (codeText == null) continue;
            if (buildText == null) continue;
            if (testText == null) continue;
            if (mergeText == null) continue;
            SetTextProgress(codeText, 1f - feature.RemainingEffort / feature.Effort);
            buildText.text = (feature.BuildFailed) ? "Fail" :
                (feature.CurrentState == Feature.State.Testing || feature.CurrentState == Feature.State.Merged)
                    ? "Pass" : "-";
            if (feature.TestFailed) {
                testText.text = "Fail";
            } else {
                SetTextProgress(testText, 1f - feature.RemainingUnitTestEffort / feature.UnitTestEffort);
            }
            mergeText.text = (feature.CurrentState == Feature.State.Merged) ? "Pass" : "-";
            var textsToChangeColor = new List<Text> {buildText, mergeText, testText};
            foreach(var text in textsToChangeColor) {
                switch (text.text) {
                    case "Pass":
                        text.color = new Color(0, 0.6f, 0);
                        break;
                    case "Fail":
                        text.color = Color.red;
                        break;
                    default:
                        text.color = new Color(0.1960784f,0.1960784f,0.1960784f);
                        break;
                }
            }
        }
        var deployableTask = DisplayingTask as DeployableTask;
        if (deployableTask == null) return;
        if (!Mathf.Approximately(deployableTask.RemainingReleaseEffort, deployableTask.ReleaseEffort)) {
            var percentage = 1f - deployableTask.RemainingReleaseEffort / deployableTask.ReleaseEffort;
            progressBarDisplayValue = Utils.MoveTowardsProportion(
                progressBarDisplayValue,
                percentage,
                0.02f,
                Time.deltaTime / 0.15f
            );
            ReleaseBar.fillAmount = progressBarDisplayValue;
        }
    }

    void OnDestroy() {
        if (displayingTask == null) return;
        if (!(displayingTask is DeployableTask task)) return;
        task.OnDeployed -= OnSuccessfulDeployment;
        task.OnDeploymentFailure -= OnDeploymentFailure;
    }

    private void OnSuccessfulDeployment(object sender, EventArgs args) {
        DeployMessage.text = "Deployed!";
    }

    private void OnDeploymentFailure(object sender, EventArgs args) {
        DeployMessage.text = "Deploy Failure";
    }

    private Text TryGetText(string textName) {
        if (textObjectCache.TryGetValue(textName, out var result)) {
            return result;
        }
        result = TextLocation.transform.Find(textName)?.gameObject?.GetComponent<Text>();
        textObjectCache[textName] = result;
        return result;
    }

    private void SetTextProgress(Text text, float value) {
        if (!textDisplayValues.TryGetValue(text, out var displayValue)) {
            displayValue = 0;
        }
        displayValue = Utils.MoveTowardsProportion(
            displayValue,
            value,
            0.02f,
            Time.deltaTime / 0.15f
        );
        textDisplayValues[text] = displayValue;
        text.text = $"{displayValue * 100:0}%";
    }
}