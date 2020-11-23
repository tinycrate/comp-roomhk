using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeployableTask : ITask, IDeployable {
    public string Name { get; } // Name of the Task
    public float Value { get; } // How important the task to the users, affect the users' satisfaction
    public bool Compulsory => true;
    public List<Feature> Features { get; }
    public float TotalFeatureEffort => Features.Sum(x => x.Effort);
    public float ReleaseEffort => TotalFeatureEffort * Constants.ReleaseEffortPercentage;
    public float FeatureCompletePercentage { get; private set; } = 0; // This only gets updated every day (tick)
    public float RemainingReleaseEffort { get; set; }
    public bool Completed => Deployed;
    public bool FeaturesCompleted => Features.TrueForAll(x => x.CurrentState == Feature.State.Merged);
    public bool Started => Features.Any(x => x.CurrentState != Feature.State.Idle);
    public bool Assigned { get; set; } = false;
    public float LeadTime { get; private set; }
    public event EventHandler<float> OnSatisfactionChange;
    public bool Deployed { get; private set; } = false;

    public bool FeatureFixTaskGenerated { get; private set; } = false;

    private float totalSatisfaction = 0;
    public float TotalSatisfaction {
        get => totalSatisfaction;
        set {
            var diff = value - totalSatisfaction;
            totalSatisfaction = value;
            if (diff < Mathf.Epsilon) {
                OnSatisfactionChange?.Invoke(this, diff);
            }
        }
    }

    public int ProductionDefectCount { get; private set; }
    public event EventHandler OnProductionDefect;
    public event EventHandler OnDeploymentFailure;
    public event EventHandler OnDeployed;

    public float EndProductQualityBonus { get; set; } = 0;

    public float EndProductQuality {
        get {
            if (Features.Count <= 0) return 0;
            if (Features.Any(x => x.CurrentState != Feature.State.Merged)) return 0;
            return Mathf.Min(1f, Features.Sum(x => x.EndProductQuality) / Features.Count + EndProductQualityBonus);
        }
    }

    public void TickDay() {
        if (!FeatureFixTaskGenerated) {
            GenerateFeatureFixTasks();
            FeatureFixTaskGenerated = true;
        }
        if (!Deployed) {
            if (!FeaturesCompleted) {
                if (Started) {
                    LeadTime += 1;
                }
                FeatureCompletePercentage = 1f - Features.Sum(x => x.RemainingEffort) / TotalFeatureEffort;
                return;
            }
            FeatureCompletePercentage = 1f;
            LeadTime += 1;
            RemainingReleaseEffort -= Constants.GlobalReleaseFactor *
                                      (1f + GameManager.GetInstance.CurrentTeam.TeamReleaseEngKnowledge);
            if (RemainingReleaseEffort > 0) return;
            // Try deploy
            GameManager.GetInstance.StatManager.TotalChangesToProduction += 1;
            if (Random.value > Constants.LeastDeploySuccessProbability
                + (1f - Constants.LeastDeploySuccessProbability) * EndProductQuality) {
                // Deployment failure
                GameManager.GetInstance.StatManager.FailedChangesToProduction += 1;
                RemainingReleaseEffort = ReleaseEffort;
                OnDeploymentFailure?.Invoke(this, EventArgs.Empty);
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
            OnTriggerProductionDefect();
        } else {
            TotalSatisfaction += EndProductQuality * Value * (1f- Mathf.Min(
                                     Constants.MaxDeployableSatisfactionPunishment,
                                     Constants.DeployableSatisfactionPunishmentPerDefect * ProductionDefectCount
                                 ));
        }
    }

    public DeployableTask(string name, float value, List<Feature> features) {
        Name = name;
        Value = value;
        Features = features;
        RemainingReleaseEffort = ReleaseEffort;
    }

    private void OnTriggerProductionDefect() {
        OnProductionDefect?.Invoke(this, EventArgs.Empty);
    }

    private void GenerateFeatureFixTasks() {
        MainGameSceneManager.GetInstance.RegisterFeatureFixTasks(SimpleTask.CreateTaskEmployeeManaged(
            SimpleTask.TaskNature.Fix,
            $"[Large] Deploy fix for {Name}",
            "Remove maximum of 5 defects",
            $"Your team notices large amount of defects in the deployed feature {Name}. They suggested they can fix at most 5 of them, but this requires a scheduled downtime of 3 hours. This task can only be done once. You should look for another way to improve your product quality if defects keep happening.",
            new Requirement("DefectCount", () => ProductionDefectCount >= 5),
            new List<Requirement>() {new Requirement("[Alert] Will cause 3 hours of downtime", () => true)},
            new List<Feature>{new Feature("Fix 5 defects", TotalFeatureEffort * 0.5f, 0.8f)},
            () => {
                GameManager.GetInstance.ProductionService.ScheduledDowntime = 3 / 24f;
                ProductionDefectCount -= 5;
            }
        ), this);
        MainGameSceneManager.GetInstance.RegisterFeatureFixTasks(SimpleTask.CreateTaskEmployeeManaged(
            SimpleTask.TaskNature.Fix,
            $"[Small] Deploy fix for {Name}",
            "Remove maximum of 2 defects",
            $"Your team notices defects in the deployed feature {Name}. You can fix them for a small quantity. This task can only be done once. You should look for another way to improve your product quality if defects keep happening.",
            new Requirement("DefectCount", () => ProductionDefectCount > 0),
            new List<Requirement>() {new Requirement("This task has no prerequisites", () => true)},
            new List<Feature>{new Feature("Fix 2 defects", TotalFeatureEffort * 0.5f, 0.5f)},
            () => ProductionDefectCount -= 2
        ), this);
        MainGameSceneManager.GetInstance.RegisterFeatureFixTasks(SimpleTask.CreateTaskEmployeeManaged(
            SimpleTask.TaskNature.Improve,
            $"Improve product {Name}",
            "Add 20% to product quality",
            $"After evaluating the performance of the deployed feature {Name}. Your team has formulated some plan to improve it even more. Some suggestions are nitpicking but some are important. You should look at your current product quality to see if it is worth the upgrade.",
            new Requirement("ViewQuality", () => GameManager.GetInstance.DisplayEndProductQuality && Deployed && EndProductQuality <= 0.996f),
            new List<Requirement>() {new Requirement("Be able to view product quality", () => GameManager.GetInstance.DisplayEndProductQuality)},
            new List<Feature>{new Feature("Improve deployed task", TotalFeatureEffort * (1f - EndProductQuality * 0.5f),  Mathf.Clamp((1f - EndProductQuality) / (0.3f), 0.01f, 0.99f))},
            () => {
                EndProductQualityBonus += 0.2f;
            }
        ), this);
    }
}
