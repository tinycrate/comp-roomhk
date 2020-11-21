using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FeaturePlanningController : MonoBehaviour {

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
        foreach (Transform child in FeatureEntryContainer.transform) {
            Destroy(child.gameObject);
        }
        featureEntryControllers.Clear();
        var index = 1;
        foreach (var feature in SelectedTask.Features) {
            var newObject = Instantiate(FeatureEntryPrefab, FeatureEntryContainer.transform, false);
            var controller = newObject.GetComponent<FeatureEntryController>();
            controller.SetFeature(feature, index++);
            featureEntryControllers[feature] = controller;
        }
        TaskNameText.text = SelectedTask.Name;
    }

    [Header("Debug")]
    public Sprite TestSprite1;
    public Sprite TestSprite2;

    public void Start() {
        DebugFromEditorPlay();
    }

    private void DebugFromEditorPlay() {
        if (Application.isEditor && GameManager.GetInstance.CurrentTeam == null) {
            GameManager.GetInstance.CurrentTeam = new Team();
            var list = new List<Employee> {
                new Employee("TEST1", 0, 0, 0, 0, 0, 0, 0, TestSprite1),
                new Employee("TEST2", 0, 0, 0, 0, 0, 0, 0, TestSprite2),
                new Employee("TEST3", 0, 0, 0, 0, 0, 0, 0, TestSprite1),
                new Employee("TEST4", 0, 0, 0, 0, 0, 0, 0, TestSprite2)
            };
            list.ForEach(x => GameManager.GetInstance.CurrentTeam.AddMember(x));
            Debug.LogWarning("Scene is running standalone in the editor, GameManager is overwritten with dummy employees for debug.");
        }
    }
}