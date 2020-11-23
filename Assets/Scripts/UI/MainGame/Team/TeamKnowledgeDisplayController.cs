using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XCharts;

public class TeamKnowledgeDisplayController : MonoBehaviour, IMainGameToggleableView {

    public Image TeamTestProgress;
    public Image TeamOperationProgress;
    public Image TeamEngineeringProgress;
    public Image TeamAutomationProgress;
    public Image TeamDevOpsProgress;
    public GameObject EmployeeHoldingArea;

    public Text EmployeeNameText;
    public Text EmployeeStatText;

    private Animator animator;
    public Animator Animator {
        get {
            if (animator == null) return animator = GetComponent<Animator>();
            return animator;
        }
    }

    void Start() {
        MainGameSceneManager.GetInstance.RegisterToggleableView(this);
    }

    void Update() {
        TeamTestProgress.fillAmount = Utils.MoveTowardsProportion(
            TeamTestProgress.fillAmount,
            GameManager.GetInstance.CurrentTeam.TeamTestingKnowledge,
            0.02f,
            Time.deltaTime / 0.15f
        );
        TeamAutomationProgress.fillAmount = Utils.MoveTowardsProportion(
            TeamAutomationProgress.fillAmount,
            GameManager.GetInstance.CurrentTeam.TeamAutomationKnowledge,
            0.02f,
            Time.deltaTime / 0.15f
        );
        TeamEngineeringProgress.fillAmount = Utils.MoveTowardsProportion(
            TeamEngineeringProgress.fillAmount,
            GameManager.GetInstance.CurrentTeam.TeamReleaseEngKnowledge,
            0.02f,
            Time.deltaTime / 0.15f
        );
        TeamOperationProgress.fillAmount = Utils.MoveTowardsProportion(
            TeamOperationProgress.fillAmount,
            GameManager.GetInstance.CurrentTeam.TeamOperationKnowledge,
            0.02f,
            Time.deltaTime / 0.15f
        );
        TeamDevOpsProgress.fillAmount = Utils.MoveTowardsProportion(
            TeamDevOpsProgress.fillAmount,
            GameManager.GetInstance.CurrentTeam.DevOpsKnowledge,
            0.02f,
            Time.deltaTime / 0.15f
        );
    }

    public bool IsShowing => Animator.GetCurrentAnimatorStateInfo(0).IsName("Showing");

    public void ToggleDisplay() {
        transform.SetAsLastSibling();
        Animator.SetTrigger("ToggleDisplay");
        UpdateEmployees();
    }

    private void UpdateEmployees() {
        foreach (Transform child in EmployeeHoldingArea.transform) {
            Destroy(child.gameObject);
        }
        foreach (var employee in GameManager.GetInstance.CurrentTeam.CurrentMembers) {
            var newObject = new GameObject(employee.Name,typeof(CanvasRenderer));
            newObject.transform.SetParent(EmployeeHoldingArea.transform, false);
            newObject.AddComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f, 1);
            newObject.AddComponent<Image>().sprite = employee.Image;
            var controller = newObject.AddComponent<EmployeeStatImageController>();
            controller.Employee = employee;
            controller.OnEmployeeHover += (sender, selectedEmployee) => { OnDisplayEmployeeStat(employee); };
        }
        OnDisplayEmployeeStat(GameManager.GetInstance.CurrentTeam.CurrentMembers.First());
    }

    private void OnDisplayEmployeeStat(Employee employee) {
        EmployeeNameText.text = $"{employee.Name}";
        EmployeeStatText.text =
            $"Efficiency {Mathf.RoundToInt(employee.Efficiency * 100)}, Experience {Mathf.RoundToInt(employee.Experience * 100)}, Testing {Mathf.RoundToInt(employee.TestingSkills * 100)}, Operation {Mathf.RoundToInt(employee.OperationSkills * 100)}, Release {Mathf.RoundToInt(employee.ReleaseEngSkills * 100)},  Automation {Mathf.RoundToInt(employee.AutomationSkills * 100)}";
    }
}
