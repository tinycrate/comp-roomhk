using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoSlideSceneManager : MonoBehaviour {
    public Animator MainSlideAnimator;
    public Button NextButton;
    public Button PreviousButton;

    public readonly Color BlueButtonColor = new Color(148 / 255f, 188 / 255f, 255 / 255f);
    public readonly Color GreenButtonColor = new Color(24 / 255f, 176 / 255f, 47 / 255f);

    private bool transiting = false;

    // Start is called before the first frame update
    void Start() {
        if (InfoSlideSceneManagerState.PreviousSlideRequested) {
            MainSlideAnimator.SetBool("EnterFromRight", true);
            InfoSlideSceneManagerState.PreviousSlideRequested = false;
        } else {
            MainSlideAnimator.SetBool("EnterFromRight", false);
        }
        var currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        var nextSceneName = (currentBuildIndex < SceneManager.sceneCountInBuildSettings - 1)
            ? Utils.GetSceneNameByBuildIndex(currentBuildIndex + 1)
            : null;
        var previousSceneName = (currentBuildIndex > 0) ? Utils.GetSceneNameByBuildIndex(currentBuildIndex - 1) : null;
        PreviousButton.gameObject.SetActive(
                previousSceneName != null  && previousSceneName.StartsWith(
                "InfoSlide",
                StringComparison.InvariantCultureIgnoreCase
            )
        );
        NextButton.gameObject.GetComponent<Image>().color =
            nextSceneName != null && nextSceneName.StartsWith("InfoSlide", StringComparison.InvariantCultureIgnoreCase)
                ? BlueButtonColor
                : GreenButtonColor;
    }

    public void OnGitFlowToggleChanged(bool enable) {
        if(!enable) return;
        GameManager.GetInstance.SelectedSourceControl = GameManager.SourceControlMode.GitFlow;
    }

    public void OnTrunkBasedToggleChanged(bool enable) {
        if(!enable) return;
        GameManager.GetInstance.SelectedSourceControl = GameManager.SourceControlMode.TrunkBased;
    }

    public void Previous() {
        if (transiting) return;
        transiting = true;
        InfoSlideSceneManagerState.PreviousSlideRequested = true;
        StartCoroutine(OnExit(true, true));
    }

    public void Next() {
        if (transiting) return;
        transiting = true;
        StartCoroutine(OnExit(false, false));
    }

    IEnumerator OnExit(bool exitToLeft, bool movePrevious) {
        MainSlideAnimator.SetBool("ExitToLeft", exitToLeft);
        MainSlideAnimator.SetTrigger("Exit");
        while (!MainSlideAnimator.GetCurrentAnimatorStateInfo(0).IsName("End")) yield return null;
        if (movePrevious) {
            GameManager.GetInstance.PreviousScene();
        } else {
            GameManager.GetInstance.NextScene();
        }
    }
}
