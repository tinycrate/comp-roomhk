using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStatSnapshot : IGameStat {
    public float TaskCompletionPercentage { get; set; }
    public float MTTR { get; set; }
    public float MTTF { get; set; }
    public float ProductionAvailability { get; set; }
    public float AverageLeadTime { get; set; }
    public float DeploymentFrequency { get; set; }
    public float ChangeFailPercentage { get; set; }
    public float UserSatisfaction { get; set; }
    // public int TotalBuildCount { get; set; }
    // public int TotalTestCount { get; set; }
    public int BuildFailureCount { get; set; }
    public int TestFailureCount { get; set; }
    public float BuildFailurePercentage { get; set; }
    public float TestFailurePercentage { get; set; }
    public int TotalProductionDefects { get; set; }
    public int UnfixedProductionDefects { get; set; }
    public float DefectFrequency { get; set; }
    public TeamKnowledge TeamKnowledgeSnapshot { get; set; }

    public GameStatSnapshot(IGameStat gameStat) {
        TaskCompletionPercentage = gameStat.TaskCompletionPercentage;
        MTTR = gameStat.MTTR;
        MTTF = gameStat.MTTF;
        ProductionAvailability = gameStat.ProductionAvailability;
        AverageLeadTime = gameStat.AverageLeadTime;
        DeploymentFrequency = gameStat.DeploymentFrequency;
        ChangeFailPercentage = gameStat.ChangeFailPercentage;
        UserSatisfaction = gameStat.UserSatisfaction;
        // TotalBuildCount = gameStat.TotalBuildCount;
        // TotalTestCount = gameStat.TotalTestCount;
        BuildFailureCount = gameStat.BuildFailureCount;
        TestFailureCount = gameStat.TestFailureCount;
        BuildFailurePercentage = gameStat.BuildFailurePercentage;
        TestFailurePercentage = gameStat.TestFailurePercentage;
        TotalProductionDefects = gameStat.TotalProductionDefects;
        UnfixedProductionDefects = gameStat.UnfixedProductionDefects;
        DefectFrequency = gameStat.DefectFrequency;
        TeamKnowledgeSnapshot = gameStat.TeamKnowledgeSnapshot;
    }
}