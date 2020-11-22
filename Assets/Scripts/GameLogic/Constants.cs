using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {
    // Coding effort scaling
    public static float GlobalEffortFactor =>
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 450f : 500f;
    // Test effort scaling
    public static float GlobalTestEffortFactor =>
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 120f : 200f;
    public static float MaxTestEffortDifficultlyScaling = 0.5f;
    // How much of the total effort is the testing effort
    public const float GlobalReleaseFactor = 500f;
    public const float UnitTestEffortPercentage = 0.2f;
    // How much of the total feature effort is the release effort
    public const float ReleaseEffortPercentage = 0.2f;
    // Team knowledge progression constants
    public const float KnowledgeBonusFactor = 0.05f;
    public const float KnowledgeAwardFactor = 0.002f;
    // Build and test probabilities
    public static float LeastBuildSuccessProbability =>
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 0.3f : 0.2f;
    public static float LeastTestPassProbability =>
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 0.4f : 0.3f;
    public const float MaxTestProbabilityBonusFromTeam = 0.2f;
    // Build and test penalty
    public const float BuildFailEffortPenalty = 0.2f;
    public const float TestFailEffortPenalty = 0.2f;
    public const float TestFailTestEffortPenalty = 0.1f;
    // Feature quality bonus
    public const float TesterSkillPointBonus = 0.1f;
    // Difficulty of feature vs required skills points
    public static float MaxRequiredExperiencePoints =>
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 1.6f : 1.8f;
    public static float MaxRequiredTestingPoints => 
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 2f : 2.4f;

    // Employee working for more than a task
    public const float WorkMultiplePenalty = 0.1f;

    // Deployment
    public const float LeastDeploySuccessProbability = 0.4f; // The minimum probability a deployment is successful
    public const float LeastDeployedServiceReliability = 0.6f; // The minimum probability a deployed service will introduce a defect in a day
    // Production
    public const float BaseProductionSatisfactionValue = 80;
    public const float BaseProductionQuality = 0.98f;
    public const float MaxProductionQualityPunishment = 0.15f;
    public const float ProductionQualityPunishmentPerDefect = 0.005f;
    public const float MaxDowntimeReductionBonus = 0.5f;
    // Deployed Services
    public const float MaxDeployableSatisfactionPunishment = 0.8f;
    public const float DeployableSatisfactionPunishmentPerDefect = 0.08f;
}

