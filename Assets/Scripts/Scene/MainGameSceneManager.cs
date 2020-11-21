using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneManager : MonoBehaviourSingleton<MainGameSceneManager> {

    [Header("Play Area")]
    public Canvas MainCanvas;

    [Header("Prefabs")] 
    public GameObject PlanningGameView;

    public IMainGameView CurrentView { get; private set; } = null;

    public void ShowTaskPlanning(ITask task) {
        var newObject = Instantiate(PlanningGameView, MainCanvas.gameObject.transform);
        newObject.GetComponent<FeaturePlanningController>().SetDisplay(task, MainCanvas);
        ChangeView(newObject.GetComponent<IMainGameView>());
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

    /* Code for debug */

    [Header("Debug")]
    public Sprite TestSprite1;
    public Sprite TestSprite2;

    public void Start() {
        DebugFromEditorPlay();
    }

    private void DebugFromEditorPlay() {
        if (Application.isEditor && GameManager.GetInstance.CurrentTeam == null) {
            GameManager.GetInstance.CurrentTeam = new Team();
            var list = new List<Employee> {
                new Employee("TEST1", 0, 0, 0, 0, 0, 0, 0, TestSprite1),
                new Employee("TEST2", 0, 0, 0, 0, 0, 0, 0, TestSprite2),
                new Employee("TEST3", 0, 0, 0, 0, 0, 0, 0, TestSprite1),
                new Employee("TEST4", 0, 0, 0, 0, 0, 0, 0, TestSprite2)
            };
            list.ForEach(x => GameManager.GetInstance.CurrentTeam.AddMember(x));
            Debug.LogWarning("Scene is running standalone in the editor, GameManager is overwritten with dummy employees for debug.");
        }
    }
}