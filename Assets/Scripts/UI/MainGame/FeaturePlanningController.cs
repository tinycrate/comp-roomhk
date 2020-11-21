using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FeaturePlanningController : MonoBehaviour {

    public Canvas ParentCanvas;
    public GameObject EmployeeListContainer;
    public GameObject EmployeeDraggingContainer;
    public List<Employee> AvailableEmployees => GameManager.GetInstance.CurrentTeam.CurrentMembers;

    private ITask selectedTask;
    public ITask SelectedTask {
        get => selectedTask;
        set {
            selectedTask = value;
            UpdateTaskDisplay();
        }
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
    }

    public Sprite TestSprite1;
    public Sprite TestSprite2;

    public void Start() {
        GameManager.GetInstance.CurrentTeam = new Team();
        var list = new List<Employee> {
            new Employee("TEST1", 0, 0, 0, 0, 0, 0, 0, TestSprite1),
            new Employee("TEST2", 0, 0, 0, 0, 0, 0, 0, TestSprite2),
            new Employee("TEST3", 0, 0, 0, 0, 0, 0, 0, TestSprite1),
            new Employee("TEST4", 0, 0, 0, 0, 0, 0, 0, TestSprite2)
        };
        list.ForEach(x => GameManager.GetInstance.CurrentTeam.AddMember(x));
        UpdateTaskDisplay();
    }

}