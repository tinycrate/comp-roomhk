using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class FeaturePlanningController : MonoBehaviour, IMainGameView {
    public Animator Animator => GetComponent<Animator>();
    public GameObject CurrentGameObject => gameObject;

    [Header("Control")] 
    public Button ConfirmButton;

    [Header("Displaying")]
    public Text TaskNameText;

    [Header("Dragging")]
    public Canvas ParentCanvas;
    public GameObject EmployeeListContainer;
    public GameObject EmployeeDraggingContainer;

    [Header("Feature Entry")] 
    public GameObject FeatureEntryPrefab;
    public GameObject FeatureEntryContainer;

    public List<Employee> AvailableEmployees => GameManager.GetInstance.CurrentTeam.CurrentMembers;

    public ITask SelectedTask { get; private set; }

    private readonly Dictionary<Feature, FeatureEntryController> featureEntryControllers = new Dictionary<Feature, FeatureEntryController>();

    public void SetDisplay(ITask task, Canvas parentCanvas) {
        SelectedTask = task;
        ParentCanvas = parentCanvas;
        UpdateTaskDisplay();
    }

    void Start() {
        ConfirmButton.onClick.AddListener(() => {
            ConfirmButton.interactable = false;
            foreach (var featureEntryController in featureEntryControllers) {
                var feature = featureEntryController.Key;
                var controller = featureEntryController.Value;
                foreach (var employee in controller.SelectedEmployees) {
                    employee.AssignedFeatures.Add(feature);
                }
            }
            SelectedTask.Assigned = true;
            MainGameSceneManager.GetInstance.OnTaskAssignmentCompleted(this, SelectedTask);
        });
    }

    private void UpdateTaskDisplay() {
        foreach (Transform child in EmployeeListContainer.transform) {
            Destroy(child.gameObject);
        }
        foreach (var employee in AvailableEmployees) {
            var newObject = new GameObject(employee.Name, typeof(RectTransform), typeof(CanvasRenderer));
            newObject.AddComponent<DraggableEmployeeController>().Register(this, employee, ParentCanvas);
            newObject.transform.SetParent(EmployeeListContainer.transform, false);
            newObject.GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f);
        }
        foreach (var child in featureEntryControllers.Values) {
            Destroy(child.gameObject);
        }
        featureEntryControllers.Clear();
        var index = 1;
        foreach (var feature in SelectedTask.Features) {
            var newObject = Instantiate(FeatureEntryPrefab, FeatureEntryContainer.transform, false);
            var controller = newObject.GetComponent<FeatureEntryController>();
            controller.OnSelectionChanged += OnSelectionChanged;
            controller.SetFeature(feature, index++);
            featureEntryControllers[feature] = controller;
        }
        TaskNameText.text = SelectedTask.Name;
    }

    private void OnSelectionChanged(object sender, EventArgs e) {
        ConfirmButton.interactable = featureEntryControllers.Values.All(x => x.SelectedEmployees.Any());
    }
}