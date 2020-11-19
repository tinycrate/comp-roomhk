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
    public Image Image;
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
        foreach (var feature in AssignedFeatures.Where(feature => feature.CurrentState != Feature.State.Merged)) {
            while (feature.RequireCoding || feature.RequireTesting) {
                if (feature.RequireCoding && remainingCodingEffort <= Mathf.Epsilon) {
                    break;
                }
                if (feature.RequireTesting && remainingTestingEffort <= Mathf.Epsilon) {
                    break;
                }
                if (feature.RequireCoding) {
                    remainingCodingEffort = feature.Code(remainingCodingEffort, this);
                } else if (feature.RequireTesting) {
                    remainingTestingEffort = feature.Test(remainingTestingEffort, this);
                }
            }
        }
    }
}
