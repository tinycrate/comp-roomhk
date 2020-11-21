using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class FeatureEntryController : MonoBehaviour {
    public Text FeatureNameText;
    public FeatureEntryDragAreaController DragAreaController;
    public Feature Feature { get; private set; }
    public IEnumerable<Employee> SelectedEmployees => DragAreaController.SelectedEmployees;
    public bool EmployeesSelected { get; private set; } = false;

    void Start() {
        DragAreaController.OnEmployeeSelectionChanged += OnSelectionChanged;
    }

    public void SetFeature(Feature feature, int featureIndex) {
        FeatureNameText.text = $"Feature {featureIndex}:\n{feature.Name}";
        Feature = feature;
    }
    
    private void OnSelectionChanged(object sender, EventArgs e) {
        EmployeesSelected = SelectedEmployees.Any();
    }
}
