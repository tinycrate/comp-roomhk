using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IGameStat {
    float TaskCompletionPercentage { get; }
    float MTTR { get; }
    float MTTF { get; }
    float ProductionAvailability { get; }
    float AverageLeadTime { get; }
    float DeploymentFrequency { get; }
    float ChangeFailPercentage { get; }
    float UserSatisfaction { get; }
    int TotalBuildCount { get; }
    int TotalTestCount { get; }
    int BuildFailureCount { get; }
    int TestFailureCount { get; }
    float BuildFailurePercentage { get; }
    float TestFailurePercentage { get; }
    int TotalProductionDefects { get; }
    int UnfixedProductionDefects { get; }
    float DefectFrequency { get; }
    TeamKnowledge TeamKnowledgeSnapshot { get; }
}