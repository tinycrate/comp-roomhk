using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager> {
    public enum SourceControlMode {
        GitFlow,
        TrunkBased
    }

    public GameStatManager StatManager { get; private set; }

    public SourceControlMode SelectedSourceControl { get; set; } = SourceControlMode.GitFlow;

    public Team CurrentTeam { get; set; }
    public List<ITask> Tasks { get; private set; }

    public Production ProductionService { get; private set; }
    public List<IDeployable> DeployedServices { get; private set; }

    public event EventHandler AfterDayTick;

    public bool DisplayEndProductQuality { get; set; } = false;

    public bool IsDebugging { get; private set; } = false;

    protected override void AfterAwake() {
        DontDestroyOnLoad(gameObject);
        IsDebugging = Application.isEditor && SceneManager.GetActiveScene().buildIndex != 0;
    }

    public void PreviousScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void NextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartSimulation() {
        Tasks = TaskFactory.DefaultList;
        DeployedServices = new List<IDeployable>();
        foreach (var task in Tasks.Where(x=>x is DeployableTask)) {
            ((DeployableTask)task).OnDeployed += OnTaskDeployed;
        }
        ProductionService = new Production();
        DeployedServices.Add(ProductionService);
        StatManager = new GameStatManager();
    }

    public void TriggerDayTick() {
        CurrentTeam.TickBonus();
        CurrentTeam.CurrentMembers.ForEach(x => x.Work());
        foreach (var task in Tasks.ToArray()) {
            task.TickDay();
        }
        foreach (var deployableNonTask in DeployedServices.Except(Tasks.OfType<IDeployable>()).ToArray()) {
            deployableNonTask.TickDay();
        }
        StatManager.EndDay();
        AfterDayTick?.Invoke(this, EventArgs.Empty);
    }

    private void OnTaskDeployed(object sender, EventArgs e) {
        DeployedServices.Add((IDeployable)sender);
        var sceneManager = MainGameSceneManager.GetInstance;
        if (sceneManager == null) return;
        sceneManager.OnDeployableTaskDeployed((DeployableTask) sender);
    }
}

