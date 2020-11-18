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
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 60f : 100f;
    // How much of the total effort is the testing effort
    public const float GlobalReleaseFactor = 500f;
    public const float UnitTestEffortPercentage = 0.2f;
    // How much of the total feature effort is the release effort
    public const float ReleaseEffortPercentage = 0.1f;
    // Team knowledge progression constants
    public const float KnowledgeBonusFactor = 0.05f;
    public const float KnowledgeAwardFactor = 0.002f;
    // Build and test probabilities
    public static float LeastBuildSuccessProbability =>
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 0.4f : 0.3f;
    public static float LeastTestPassProbability =>
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 0.5f : 0.4f;
    public const float MaxTestProbabilityBonusFromTeam = 0.2f;
    // Build and test penalty
    public const float BuildFailEffortPenalty = 0.2f;
    public const float TestFailEffortPenalty = 0.2f;
    public const float TestFailTestEffortPenalty = 0.1f;
    // Feature quality bonus
    public const float TesterSkillPointBonus = 0.1f;
    // Difficulty of feature vs required skills points
    public static float MaxRequiredExperiencePoints =>
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 2f : 2.2f;
    public static float MaxRequiredTestingPoints => 
        (GameManager.GetInstance.SelectedSourceControl == GameManager.SourceControlMode.GitFlow) ? 2.5f : 2.8f;
    // Core Service
    public const float CoreFeatureSatisfactionValue = 80;
    // Deployment
    public const float LeastDeploySuccessProbability = 0.4f; // The minimum probability a deployment is successful
    public const float LeastDeployedServiceReliability = 0.6f; // The minimum probability a deployed service will introduce a defect in a day
}

