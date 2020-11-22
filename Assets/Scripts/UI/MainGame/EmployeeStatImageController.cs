using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

class EmployeeStatImageController : MonoBehaviour, IPointerEnterHandler {
    public Employee Employee { get; set; }
    public event EventHandler<Employee> OnEmployeeHover;

    public void OnPointerEnter(PointerEventData eventData) {
        OnEmployeeHover?.Invoke(this, Employee);
    }
}

