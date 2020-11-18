using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

class DeployableTask : ITask, IDeployable {
    public string Name { get; } // Name of the Task
    public float Value { get; } // How important the task to the users, affect the users' satisfaction
    public List<Feature> Features { get; }
    public float TotalFeatureEffort => Features.Sum(x => x.Effort);
    public float ReleaseEffort => TotalFeatureEffort * Constants.ReleaseEffortPercentage;
    public float RemainingReleaseEffort { get; set; }
    public bool Completed => Deployed;
    public bool FeaturesCompleted => Features.TrueForAll(x => x.CurrentState == Feature.State.Merged);
    public bool Started => Features.Any(x => x.CurrentState != Feature.State.Idle);
    public float LeadTime { get; private set; }
    public event EventHandler OnTaskCompleted;
    public bool Deployed { get; private set; } = false;
    public float TotalSatisfaction { get; set; } = 0;
    public int ProductionDefectCount { get; private set; }
    public event EventHandler OnProductionDefect;
    public event EventHandler OnDeploymentFailure;
    public event EventHandler OnDeployed;

    public float EndProductQuality {
        get {
            if (!Deployed || Features.Count <= 0) return 0;
            if (Features.Any(x => x.CurrentState != Feature.State.Merged)) return 0;
            return Features.Sum(x => x.EndProductQuality) / Features.Count;
        }
    }

    public void TickDay() {
        if (!Deployed) {
            if (!FeaturesCompleted) {
                if (Started) LeadTime += 1;
                return;
            }
            LeadTime += 1;
            RemainingReleaseEffort -= Constants.GlobalReleaseFactor *
                                      (1f + GameManager.GetInstance.CurrentTeam.TeamReleaseEngKnowledge);
            if (RemainingReleaseEffort > 0) return;
            // Try deploy
            GameManager.GetInstance.StatManager.TotalChangesToProduction += 1;
            if (Random.value > Constants.LeastDeploySuccessProbability
                + (1f - Constants.LeastDeploySuccessProbability) * EndProductQuality) {
                // Deployment failure
                OnDeploymentFailure?.Invoke(this, EventArgs.Empty);
                GameManager.GetInstance.StatManager.FailedChangesToProduction += 1;
                RemainingReleaseEffort = ReleaseEffort;
            } else {
                RemainingReleaseEffort = 0;
                Deployed = true;
                GameManager.GetInstance.StatManager.LeadTimes.Add(LeadTime);
                OnDeployed?.Invoke(this, EventArgs.Empty);
            }
            return;
        };
        if (Random.value > Constants.LeastDeployedServiceReliability 
            + (1f - Constants.LeastDeployedServiceReliability) * EndProductQuality) {
            // Production defect
            ProductionDefectCount += 1;
            GameManager.GetInstance.StatManager.TotalProductionDefects += 1;
            OnProductionDefect?.Invoke(this, EventArgs.Empty);
        } else {
            TotalSatisfaction += EndProductQuality * Value;
        }
    }

    public DeployableTask(string name, float value, List<Feature> features) {
        Name = name;
        Value = value;
        Features = features;
        RemainingReleaseEffort = TotalFeatureEffort;
    }
}
