using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingSceneManager : MonoBehaviour {

    public Animator SceneExitAnimator;

    public void StartGame() {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine() {
        SceneExitAnimator.SetTrigger("Start");
        while (!SceneExitAnimator.GetCurrentAnimatorStateInfo(0).IsName("End")) yield return null;
        GameManager.GetInstance.NextScene();
    }
}
