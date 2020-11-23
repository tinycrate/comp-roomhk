using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class SimpleTaskPlanningController : MonoBehaviour, IMainGameView {
    public Animator Animator => GetComponent<Animator>();
    public GameObject CurrentGameObject => gameObject;

    [Header("Control")] 
    public Button ConfirmButton;

    [Header("Displaying")]
    public Text TaskNameText;
    public Text TaskDescriptionText;
    public GameObject TaskCompletionProgressBarObject;
    public Image TaskCompletionProgressBar;

    [Header("Dragging")]
    public Canvas ParentCanvas;
    public GameObject EmployeeListContainer;
    public GameObject EmployeeDraggingContainer;

    [Header("Feature Entry")] 
    public GameObject FeatureEntryPrefab;
    public GameObject FeatureEntryContainer;
    public FeatureEntryDragAreaController CustomTaskEmployeeDragController;

    public List<Employee> AvailableEmployees => GameManager.GetInstance.CurrentTeam.CurrentMembers;

    public SimpleTask SelectedTask { get; private set; }

    private readonly Dictionary<Feature, FeatureEntryController> featureEntryControllers = new Dictionary<Feature, FeatureEntryController>();

    public void SetDisplay(SimpleTask task, Canvas parentCanvas) {
        SelectedTask = task;
        ParentCanvas = parentCanvas;
        UpdateTaskDisplay();
    }

    void Start() {
        GameManager.GetInstance.AfterDayTick += AfterDayTick;
        if (SelectedTask.Accepted) {
            ConfirmButton.gameObject.SetActive(false);
        }
        ConfirmButton.onClick.AddListener(() => {
            ConfirmButton.interactable = false;
            if (SelectedTask.CompletionMethod == SimpleTask.TaskCompletionMethod.EmployeeManaged) {
                foreach (var featureEntryController in featureEntryControllers) {
                    var feature = featureEntryController.Key;
                    var controller = featureEntryController.Value;
                    foreach (var employee in controller.SelectedEmployees) {
                        employee.AssignedFeatures.Add(feature);
                    }
                }
                SelectedTask.Assigned = true;
            }
            if (SelectedTask.CompletionMethod == SimpleTask.TaskCompletionMethod.CustomManaged &&
                SelectedTask.CustomCompletionRequiresEmployees) {
                SelectedTask.AssignedEmployees = CustomTaskEmployeeDragController.SelectedEmployees.ToList();
            }
            SelectedTask.AcceptAndExecuteTask();
            if (SelectedTask.CompletionMethod == SimpleTask.TaskCompletionMethod.EmployeeManaged) {
                MainGameSceneManager.GetInstance.OnTaskAssignmentCompleted(SelectedTask);
            }
        });
    }

    void OnDestroy() {
        GameManager.GetInstance.AfterDayTick -= AfterDayTick;
    }

    void Update() {
        if (!SelectedTask.Accepted) return;
        if (!TaskCompletionProgressBarObject.activeSelf) {
            TaskCompletionProgressBarObject.SetActive(true);
        }
        TaskCompletionProgressBar.fillAmount = Utils.MoveTowardsProportion(
            TaskCompletionProgressBar.fillAmount,
            SelectedTask.Progress,
            0.02f,
            Time.deltaTime / 0.15f
        );

    }

    private void UpdateTaskDescriptionText() {
        TaskDescriptionText.text =
            $"{SelectedTask.TaskDescription}\n\nThis task requires the following prerequisites: {SelectedTask.UnlockRequirementStatus}";
    }

    private void UpdateTaskDisplay() {
        TaskNameText.text = SelectedTask.Name;
        UpdateTaskDescriptionText();
        foreach (Transform child in EmployeeListContainer.transform) {
            Destroy(child.gameObject);
        }
        foreach (var employee in AvailableEmployees) {
            var newObject = new GameObject(employee.Name, typeof(RectTransform), typeof(CanvasRenderer));
            newObject.AddComponent<DraggableEmployeeController>().Register(
                EmployeeListContainer,
                EmployeeDraggingContainer,
                employee,
                ParentCanvas
            );
            newObject.transform.SetParent(EmployeeListContainer.transform, false);
            newObject.GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f);
        }
        foreach (var child in featureEntryControllers.Values) {
            Destroy(child.gameObject);
        }
        if(SelectedTask.Accepted) return;
        featureEntryControllers.Clear();
        if (SelectedTask.CompletionMethod == SimpleTask.TaskCompletionMethod.EmployeeManaged) {
            var index = 1;
            foreach (var feature in SelectedTask.Features) {
                var newObject = Instantiate(FeatureEntryPrefab, FeatureEntryContainer.transform, false);
                var controller = newObject.GetComponent<FeatureEntryController>();
                controller.OnSelectionChanged += OnSelectionChanged;
                controller.SetFeature(feature, index++);
                featureEntryControllers[feature] = controller;
            }
        }
        if (SelectedTask.CompletionMethod == SimpleTask.TaskCompletionMethod.CustomManaged &&
            SelectedTask.CustomCompletionRequiresEmployees) {
            CustomTaskEmployeeDragController.gameObject.SetActive(true);
            CustomTaskEmployeeDragController.OnEmployeeSelectionChanged += OnSelectionChanged;
        }
        CheckCanConfirm();
    }

    private void OnSelectionChanged(object sender, EventArgs e) {
        CheckCanConfirm();
    }
    private void AfterDayTick(object sender, EventArgs e) {
        if (SelectedTask.Accepted) return;
        CheckCanConfirm();
    }

    private void CheckCanConfirm() {
        if (SelectedTask.Accepted) return;
        UpdateTaskDescriptionText();
        if (!SelectedTask.Unlocked) return;
        switch (SelectedTask.CompletionMethod) {
            case SimpleTask.TaskCompletionMethod.EmployeeManaged:
                ConfirmButton.interactable = featureEntryControllers.Values.All(x => x.SelectedEmployees.Any());
                break;
            case SimpleTask.TaskCompletionMethod.CustomManaged:
                ConfirmButton.interactable = !SelectedTask.CustomCompletionRequiresEmployees || CustomTaskEmployeeDragController.SelectedEmployees.Any();
                break;
            case SimpleTask.TaskCompletionMethod.Instant:
                ConfirmButton.interactable = true;
                break;
        }
    }
}