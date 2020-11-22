using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStatManager : IGameStat {

    public float DayPassed { get; private set; } = 0;
    public List<GameStatSnapshot> Snapshots { get; } = new List<GameStatSnapshot>();

    public List<AvailabilityReport> AvailabilityReports { get; } = new List<AvailabilityReport>(); // Contains all (M)TTR, (M)TTF values and availability
    public List<float> LeadTimes { get;  } = new List<float>();

    public int TotalChangesToProduction { get; set; } = 0;
    public int FailedChangesToProduction { get; set; } = 0;

    public int TotalBuildCount { get; set; } = 0;
    public int TotalTestCount { get; set; } = 0;
    public int BuildFailureCount { get; set; } = 0;
    public int TestFailureCount { get; set; } = 0;
    public int TotalProductionDefects { get; set; } = 0;


    public float TaskCompletionPercentage =>
        CompletedTasks.Where(x => x.Compulsory).Sum(x => x.Features.Sum(y => y.Effort * y.Difficulty)) /
        totalTaskEffort;

    public float UserSatisfaction => GameManager.GetInstance.DeployedServices.Sum(x => x.TotalSatisfaction);

    public float MTTR => AvailabilityReport.CalculateMTTR(AvailabilityReports);
    public float MTTF => AvailabilityReport.CalculateMTTF(AvailabilityReports);
    public float ProductionAvailability => AvailabilityReport.CalculateAvailability(AvailabilityReports);
    public float AverageLeadTime => LeadTimes.Sum() / LeadTimes.Count;
    public float DeploymentFrequency => ((float) TotalChangesToProduction - FailedChangesToProduction) / DayPassed;
    public float ChangeFailPercentage => (float) FailedChangesToProduction / TotalChangesToProduction;
    public float BuildFailurePercentage => (float) BuildFailureCount / TotalBuildCount;
    public float TestFailurePercentage => (float) TestFailureCount / TotalTestCount;
    public int UnfixedProductionDefects => GameManager.GetInstance.DeployedServices.Sum(x=>x.ProductionDefectCount);
    public float DefectFrequency => (float) TotalProductionDefects / DayPassed;
    public TeamKnowledge TeamKnowledgeSnapshot { get; private set; }

    public IEnumerable<ITask> CompletedTasks => GameManager.GetInstance.Tasks.Where(x => x.Completed);

    private readonly float totalTaskEffort = TaskFactory.DefaultList.Where(x => x.Compulsory)
        .Sum(x => x.Features.Sum(y => y.Effort * y.Difficulty));

    public void EndDay() {
        DayPassed += 1;
        TeamKnowledgeSnapshot = new TeamKnowledge(GameManager.GetInstance.CurrentTeam.KnowledgeSnapshot);
        Snapshots.Add(new GameStatSnapshot(this));
    }
}

