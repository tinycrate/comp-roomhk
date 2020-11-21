using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class FeatureEntryDragAreaController : MonoBehaviour {

    public GameObject HoldingArea;
    public GameObject HintText;
    private readonly HashSet<DraggableEmployeeController> employees = new HashSet<DraggableEmployeeController>();
    public IEnumerable<Employee> SelectedEmployees => employees.Select(x => x.Employee);
    public event EventHandler OnEmployeeSelectionChanged;

    // Tries to add an employee, returns false if failed (maybe existing)
    public bool AddEmployeeController(DraggableEmployeeController employeeController) {
        if (employees.Contains(employeeController)) return false;
        employees.Add(employeeController);
        OnSelectionChanged();
        return true;
    }

    public void RemoveEmployeeController(DraggableEmployeeController employeeController) {
        employees.Remove(employeeController);
        OnSelectionChanged();
    }

    private void OnSelectionChanged() {
        if (employees.Count == 0) {
            HintText.SetActive(true);
        } else {
            HintText.SetActive(false);
        }
        OnEmployeeSelectionChanged?.Invoke(this, EventArgs.Empty );
    }
}
