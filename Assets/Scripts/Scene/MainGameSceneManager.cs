using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameSceneManager : MonoBehaviourSingleton<MainGameSceneManager> {

    [Header("References")]
    public Canvas MainCanvas;
    public TaskListController TaskListController;

    [Header("Prefabs")] 
    public GameObject PlanningGameView;
    public GameObject DevelopGameView;

    [Header("Display")] 
    public Text DayText;

    public IMainGameView CurrentView { get; private set; } = null;

    public void ShowTaskPlanning(ITask task) {
        var newObject = Instantiate(PlanningGameView, MainCanvas.gameObject.transform);
        newObject.GetComponent<FeaturePlanningController>().SetDisplay(task, MainCanvas);
        ChangeView(newObject.GetComponent<IMainGameView>());
    }

    public void ShowTaskProgress(ITask taskBeingDisplayed) {
        var newObject = Instantiate(DevelopGameView, MainCanvas.gameObject.transform);
        newObject.GetComponent<DevelopProgressController>().DisplayingTask = taskBeingDisplayed;
        ChangeView(newObject.GetComponent<IMainGameView>());
    }

    public void OnTaskAssignmentCompleted(FeaturePlanningController controller, ITask task) {
        TaskListController.UpgradeTaskInProgress(task);
        ShowTaskProgress(task);
    }

    public void ChangeView(IMainGameView view) {
        if (CurrentView != null) {
            StartCoroutine(OnDestroyView(CurrentView));
        }
        CurrentView = view;
    }

    IEnumerator OnDestroyView(IMainGameView view) {
        if (view.Animator != null) {
            view.Animator.SetTrigger("Exit");
            while (!view.Animator.GetCurrentAnimatorStateInfo(0).IsName("End")) yield return null;
        }
        Destroy(view.CurrentGameObject);
    }

    public void Start() {
        DebugFromEditorPlay();
        GameManager.GetInstance.StartSimulation();
        GameManager.GetInstance.AfterDayTick += AfterDayTick;
    }

    public void TriggerDayTick() {
        GameManager.GetInstance.TriggerDayTick();
    }

    private void AfterDayTick(object sender, EventArgs e) {
        DayText.text = $"Day: {Mathf.RoundToInt(GameManager.GetInstance.StatManager.DayPassed)}";
    }

    void OnDestroy() {
        GameManager.GetInstance.AfterDayTick -= AfterDayTick;
    }

    /* Code for debug */

    [Header("Debug")]
    public Sprite TestSprite1;
    public Sprite TestSprite2;

    private void DebugFromEditorPlay() {
        if (Application.isEditor && GameManager.GetInstance.CurrentTeam == null) {
            GameManager.GetInstance.CurrentTeam = new Team();
            var list = new List<Employee> {
                new Employee("TEST1", 1000, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, TestSprite1),
                new Employee("TEST2", 1000, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, TestSprite2),
                new Employee("TEST3", 1000, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, TestSprite1),
                new Employee("TEST4", 1000, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, TestSprite2)
            };
            list.ForEach(x => GameManager.GetInstance.CurrentTeam.AddMember(x));
            Debug.LogWarning("Scene is running standalone in the editor, GameManager is overwritten with dummy employees for debugging.");
        }
    }
}