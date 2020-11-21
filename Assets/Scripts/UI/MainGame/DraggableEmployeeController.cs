using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using RectTransform = UnityEngine.RectTransform;

public class DraggableEmployeeController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, 
    IPointerClickHandler {
    public Canvas ParentCanvas { get; set; }
    public FeaturePlanningController ParentController { get; private set; }
    public Employee Employee { get; private set; }

    public bool Registered { get; private set; } = false;
    public FeatureEntryDragAreaController AssignedDragArea { get; private set; } = null;

    private Image ImageComponent {
        get {
            var component = GetComponent<Image>();
            return component == null ? gameObject.AddComponent<Image>() : component;
        }
    }

    public void Register(FeaturePlanningController parentController, Employee employee, Canvas parentCanvas) {
        ParentController = parentController;
        Employee = employee;
        ImageComponent.sprite = employee.Image;
        ParentCanvas = parentCanvas;
        Registered = true;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (AssignedDragArea != null) {
            DetachFromDragArea();
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!Registered) return;
        transform.SetParent(ParentController.EmployeeDraggingContainer.transform, false);
        GetComponent<Image>().raycastTarget = false;
        GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    public void OnDrag(PointerEventData eventData) {
        if (!Registered) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            ParentCanvas.GetComponent<RectTransform>(), 
            eventData.position,
            ParentCanvas.worldCamera,
            out var localPosition
        );
        transform.position = ParentCanvas.transform.TransformPoint(localPosition);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!Registered) return;
        var results = new List<RaycastResult>();
        ParentCanvas.GetComponent<GraphicRaycaster>().Raycast(eventData, results);
        foreach (var result in results) {
            if (result.gameObject.TryGetComponent(typeof(FeatureEntryDragAreaController), out var component)) {
                var controller = (FeatureEntryDragAreaController) component;
                if (component == AssignedDragArea) {
                    RestorePosition();
                    return;
                }
                if (AssignedDragArea != null) {
                    DetachFromDragArea();
                }
                if (controller.AddEmployeeController(this)) {
                    AssignedDragArea = controller;
                    RestorePosition();
                }
                return;
            }
        }
        if (AssignedDragArea != null) {
            DetachFromDragArea();
        }
        // Restore to original position if no draggable area is found
        RestorePosition();
    }

    private void RestorePosition() {
        GetComponent<Image>().raycastTarget = true;
        var parentObject = (AssignedDragArea == null)
            ? ParentController.EmployeeListContainer.transform
            : AssignedDragArea.HoldingArea.transform;
        transform.SetParent(parentObject, false);
        GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
    }

    private void DetachFromDragArea() {
        AssignedDragArea.RemoveEmployeeController(this);
        AssignedDragArea = null;
        RestorePosition();
    }
}
