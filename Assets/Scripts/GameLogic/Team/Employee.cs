using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Employee {
    public String Name;
    public int Cost;
    [Range(0, 1)] public float Efficiency;
    [Range(0, 1)] public float Experience;
    [Range(0, 1)] public float TestingSkills;
    [Range(0, 1)] public float OperationSkills;
    [Range(0, 1)] public float ReleaseEngSkills;
    [Range(0, 1)] public float AutomationSkills;
    public Sprite Image;
    public List<Feature> AssignedFeatures { get; } = new List<Feature>();
    public Team AssignedTeam { get; set; }

    public Employee(String name, int cost, float efficiency, float experience, float testingSkills, float operationSkills, float releaseEngSkills, float automationSkills) {
        Name = name;
        Cost = cost;
        Efficiency = efficiency;
        Experience = experience;
        TestingSkills = testingSkills;
        OperationSkills = operationSkills;
        ReleaseEngSkills = releaseEngSkills;
        AutomationSkills = automationSkills;
    }

    // Work on the assigned feature, should be called once per day
    public void Work() {
        if (AssignedTeam == null) {
            Debug.LogError($"The employee {Name} has no assigned team but is required to work.");
            return;
        }
        var remainingCodingEffort = Constants.GlobalEffortFactor * Efficiency * (1f + AssignedTeam.DevOpsKnowledge);
        var remainingTestingEffort = Constants.GlobalTestEffortFactor * TestingSkills * (1f + AssignedTeam.TeamTestingKnowledge);
        while (true) {
            var features = AssignedFeatures.Where(feature => feature.RequireCoding).ToList();
            if (features.Count <= 0) break;
            if (remainingCodingEffort <= Mathf.Epsilon) break;
            var maxEffortPerFeature = remainingCodingEffort / features.Count;
            foreach (var feature in features) {
                if (feature.RequireCoding) {
                    remainingCodingEffort -= feature.Code(Mathf.Min(maxEffortPerFeature, remainingCodingEffort), this);
                }
            }
        }
        while (true) {
            var features = AssignedFeatures.Where(feature => feature.RequireTesting).ToList();
            if (features.Count <= 0) break;
            if (remainingTestingEffort <= Mathf.Epsilon) break;
            var maxEffortPerFeature = remainingTestingEffort / features.Count;
            foreach (var feature in features) {
                if (feature.RequireTesting) {
                    remainingTestingEffort -= feature.Test(Mathf.Min(maxEffortPerFeature, remainingTestingEffort), this);
                }
            }
        }
    }
}
