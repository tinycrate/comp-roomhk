﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RecruitController : MonoBehaviour {

    [Header("Anchors")] 
    public RectTransform SlotAnchorCornerLeft;
    public RectTransform SlotAnchorCornerRight;
    public RectTransform QueueAnchorLeft;
    public RectTransform QueueAnchorRight;

    [Header("Display")] 
    public Text NameText;
    public Text PriceText;
    public Text BudgetText;
    public AbilityHexagon AbilityHexagon;

    [Header("Parameters")] 
    public float Budget;
    public Employee[] Employees = new Employee[9];

    public List<Employee> Selected { get; } = new List<Employee>();

    public Employee ShowingEmployee { get; private set; } = null;

    public float RemainingBudget => Budget - Selected.Sum(x => x.Cost);
    private float displayingRemainingBudget = 0;

    private enum AbilityHexParams {
        Operation = 0,
        Testing = 1,
        RelEng = 2,
        Automation = 3,
        Experience = 4,
        Efficiency = 5
    }

    private Dictionary<AbilityHexParams, Func<float>> abilityMapping = null;
    private Dictionary<AbilityHexParams, Func<float>> AbilityMapping {
        get {
            return abilityMapping ?? (abilityMapping = new Dictionary<AbilityHexParams, Func<float>> {
                {AbilityHexParams.Operation, () => ShowingEmployee.OperationSkills},
                {AbilityHexParams.Testing, () => ShowingEmployee.TestingSkills},
                {AbilityHexParams.RelEng, () => ShowingEmployee.ReleaseEngSkills},
                {AbilityHexParams.Automation, () => ShowingEmployee.AutomationSkills},
                {AbilityHexParams.Experience, () => ShowingEmployee.Experience},
                {AbilityHexParams.Efficiency, () => ShowingEmployee.Efficiency},
            });
        }
    }

    public void ShowStatistics(Image employeeImage) {
        var employee = findEmployee(employeeImage);
        if (employeeImage != null) ShowingEmployee = employee;
    }

    public void ToggleSelection(Image employeeImage) {
        var employee = findEmployee(employeeImage);
        if (employeeImage == null) return;
        if (Selected.Contains(employee)) {
            Selected.Remove(employee);
        } else if (RemainingBudget >= employee.Cost) {
            Selected.Add(employee);
        }
        ArrangeEmployees();
    }

    // Start is called before the first frame update
    void Start() {
        ArrangeEmployees();
    }

    // Update is called once per frame
    void Update() {
        displayingRemainingBudget = Utils.MoveTowardsProportion(
            displayingRemainingBudget,
            RemainingBudget,
            100,
            Time.deltaTime / 0.15f
        );
        BudgetText.text = $"Budget: ${Mathf.RoundToInt(displayingRemainingBudget)}";
        if (ShowingEmployee != null) {
            NameText.text = $"{ShowingEmployee.Name}";
            PriceText.text = $"${ShowingEmployee.Cost}";
            foreach (var mapping in AbilityMapping) {
                AbilityHexagon.Stats[(int) mapping.Key].Value = Utils.MoveTowardsProportion(
                    AbilityHexagon.Stats[(int) mapping.Key].Value,
                    mapping.Value.Invoke(),
                    0.02f,
                    Time.deltaTime / 0.15f
                );
            }
        }
    }

    private void ArrangeEmployees() {
        // Arrange slots
        var xStep = SlotAnchorCornerRight.anchoredPosition.x - SlotAnchorCornerLeft.anchoredPosition.x;
        var yStep = SlotAnchorCornerRight.anchoredPosition.y - SlotAnchorCornerLeft.anchoredPosition.y;
        var pos = 0;
        foreach (var employee in Employees.Where(x=>!Selected.Contains(x))) {
            employee.Image.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                SlotAnchorCornerLeft.anchoredPosition.x + xStep * (int)(pos/3),
                SlotAnchorCornerLeft.anchoredPosition.y + yStep * (pos%3)
            );
            pos++;
        }
        // Arrange queue
        var xStepQueue = QueueAnchorRight.anchoredPosition.x - QueueAnchorLeft.anchoredPosition.x;
        pos = 0;
        foreach (var employee in Selected) {
            employee.Image.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                QueueAnchorLeft.anchoredPosition.x + xStepQueue * pos,
                QueueAnchorLeft.anchoredPosition.y
            );
            pos++;
        }
    }

    private Employee findEmployee(Image employeeImage) {
        foreach (var employee in Employees) {
            if (employee != null && employee.Image == employeeImage) {
                return employee;
            }
        }
        return null;
    }
}
