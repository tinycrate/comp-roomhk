using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager> {
    public enum SourceControlMode {
        GitFlow,
        TrunkBased
    }

    public enum DeploymentMode {
        Canary,
        BlueGreen,
        Rolling
    }

    public GameStatManager StatManager { get; private set; }

    public SourceControlMode SelectedSourceControl { get; set; } = SourceControlMode.GitFlow;
    public DeploymentMode SelectedDeploymentMode { get; set; } = DeploymentMode.Canary;

    public Team CurrentTeam { get; private set; }
    public List<ITask> Tasks { get; private set; } = TaskFactory.DefaultList;

    public Production ProductionService { get; private set; }
    public List<IDeployable> DeployedServices { get; private set; }

    public void SetTeam(Team team) {
        CurrentTeam = team;
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

    private void OnTaskDeployed(object sender, EventArgs e) {
        DeployedServices.Add((IDeployable)sender);
    }
}

