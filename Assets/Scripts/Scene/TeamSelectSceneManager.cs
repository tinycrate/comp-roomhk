using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelectSceneManager : MonoBehaviourSingleton<TeamSelectSceneManager> {

    public RecruitController RecruitController;
    public List<Animator> ExitAnimators;
    public Animator LastExitAnimator;

    public void ConfirmSelection(List<Employee> selected) {
        var team = new Team();
        selected.ForEach(x => team.AddMember(x));
        GameManager.GetInstance.SetTeam(team);
        StartCoroutine(OnConfirmSelection());
    }

    IEnumerator OnConfirmSelection() {
        ExitAnimators.ForEach(x=>x.SetTrigger("Exit"));
        while (!LastExitAnimator.GetCurrentAnimatorStateInfo(0).IsName("End")) yield return null;
        GameManager.GetInstance.NextScene();
    }
}
