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

    private List<Feature> assignedFeatures = new List<Feature>();

    public List<Feature> AssignedFeatures => assignedFeatures ?? (assignedFeatures = new List<Feature>());

    public Team AssignedTeam { get; set; }

    public Employee(String name, int cost, float efficiency, float experience, float testingSkills, float operationSkills, float releaseEngSkills, float automationSkills, Sprite image) {
        Name = name;
        Cost = cost;
        Efficiency = efficiency;
        Experience = experience;
        TestingSkills = testingSkills;
        OperationSkills = operationSkills;
        ReleaseEngSkills = releaseEngSkills;
        AutomationSkills = automationSkills;
        Image = image;
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
                    var consumedEffort = feature.Code(Mathf.Min(maxEffortPerFeature, remainingCodingEffort), this);
                    remainingCodingEffort -= consumedEffort * (1f + Constants.WorkMultiplePenalty * features.Count);
                    if (remainingCodingEffort <= Mathf.Epsilon) remainingCodingEffort = 0;
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
                    var consumedEffort = feature.Test(Mathf.Min(maxEffortPerFeature, remainingTestingEffort), this);
                    remainingTestingEffort -= consumedEffort * (1f + Constants.WorkMultiplePenalty * features.Count);
                    if (remainingTestingEffort <= Mathf.Epsilon) remainingTestingEffort = 0;
                }
            }
        }
    }
}
