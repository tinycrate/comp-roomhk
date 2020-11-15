using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
    // Coding effort scaling
    public const float GlobalEffortFactor = 50;
    // Test effort scaling
    public const float GlobalTestEffortFactor = 10;
    // Team knowledge progression constants
    public const float KnowledgeBonusFactor = 0.05f;
    public const float KnowledgeAwardFactor = 0.002f;
    // Build and test probabilities
    public const float LeastBuildSuccessProbability = 0.3f;
    public const float LeastTestPassProbability = 0.4f;
    public const float MaxTestProbabilityBonusFromTeam = 0.2f;
    // Build and test penalty
    public const float BuildFailEffortPenalty = 0.2f;
    public const float TestFailEffortPenalty = 0.2f;
    // Feature quality bonus
    public const float TesterSkillPointBonus = 0.1f;
    // Difficulty of feature vs required skills points
    public const float MaxRequiredExperiencePoints = 2f;
    public const float MaxRequiredTestingPoints = 2.5f;
}

