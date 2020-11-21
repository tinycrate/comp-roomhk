using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class FeatureEntryController : MonoBehaviour {
    public Text FeatureNameText;
    public Text FeatureDescriptionText;
    public FeatureEntryDragAreaController DragAreaController;
    public Feature Feature { get; private set; }
    public IEnumerable<Employee> SelectedEmployees => DragAreaController.SelectedEmployees;
    public event EventHandler OnSelectionChanged {
        add => DragAreaController.OnEmployeeSelectionChanged += value;
        remove => DragAreaController.OnEmployeeSelectionChanged -= value;
    }

    public void SetFeature(Feature feature, int featureIndex) {
        FeatureNameText.text = $"Feature {featureIndex}:\n{feature.Name}";
        FeatureDescriptionText.text = $"Effort: {feature.Effort} Difficulty: {feature.Difficulty}";
        Feature = feature;
    }
}
