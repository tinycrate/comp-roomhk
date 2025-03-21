﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmployeeButtonController : MonoBehaviour {

    public Employee Employee { get; set; }

    private Button button;
    private EventTrigger eventTrigger;

    // Start is called before the first frame update
    void Start() {
        button = gameObject.AddComponent<Button>();
        eventTrigger = gameObject.AddComponent<EventTrigger>();
        var trigger = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        trigger.callback.AddListener(
            (data) => {
                TeamSelectSceneManager.GetInstance.RecruitController.ShowStatistics(Employee);
            });
        eventTrigger.triggers.Add(trigger);
        button.onClick.AddListener(() => {
            TeamSelectSceneManager.GetInstance.RecruitController.ToggleSelection(Employee);
        });
    }

    // Update is called once per frame
    void Update() {
        
    }
}
