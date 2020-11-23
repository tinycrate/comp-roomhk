using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTaskController : MonoBehaviour {

    public Text TaskTypeText;
    public Text TaskNameText;
    public Text EffectText;
    public Text EffectTitleText;

    public SimpleTask Task { get; set; }

    // Start is called before the first frame update
    void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            MainGameSceneManager.GetInstance.ShowSimpleTaskPlanning(Task);
        });
    }

    // Update is called once per frame
    void Update() {
        if (Task == null) return;
        if (Task.Completed) {
            GetComponent<Image>().color = new Color(0.5529412f, 1, 0.5529412f, 0.8588235f);
            TaskTypeText.text = "Completed";
            TaskNameText.text = Task.Name;
            EffectText.gameObject.SetActive(false);
            EffectTitleText.gameObject.SetActive(false);
            return;
        }
        EffectTitleText.gameObject.SetActive(true);
        EffectText.gameObject.SetActive(true);
        switch (Task.Nature) {
            case SimpleTask.TaskNature.Upgrade:
                GetComponent<Image>().color = new Color(1f, 0.8584991f, 0.5529412f);
                break;
            case SimpleTask.TaskNature.Fix:
                GetComponent<Image>().color = new Color(1f, 0.603f, 1f);
                break;
            case SimpleTask.TaskNature.Improve:
                GetComponent<Image>().color = new Color(0.9921f,1,0.469f);
                break;
        }
        TaskTypeText.text = Task.Nature.ToString();
        TaskNameText.text = Task.Name;
        EffectText.text = Task.EffectDescription;
    }
}
