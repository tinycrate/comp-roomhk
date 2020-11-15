using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class Employee {
    public String Name { get; private set; }
    public int Cost { get; private set; }
    public float Efficiency { get; private set; }
    public float Experience { get; private set; }
    public float TestingSkills { get; private set; }
    public float OperationSkills { get; private set; }
    public float ReleaseEngSkills { get; private set; }
    public float AutomationSkills { get; private set; }
    public List<Feature> AssignedFeature { get; } = new List<Feature>();
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
    private void Work() {
        if (AssignedTeam == null) {
            Debug.LogError($"The employee {Name} has no assigned team but is required to work.");
            return;
        }
        var remainingCodingEffort = Constants.GlobalEffortFactor * Efficiency * (1f + AssignedTeam.DevOpsKnowledge);
        var remainingTestingEffort = Constants.GlobalTestEffortFactor * TestingSkills * (1f + AssignedTeam.TeamTestingKnowledge);
        foreach (var feature in AssignedFeature.Where(feature => feature.CurrentState != Feature.State.Merged)) {
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

    public static List<Employee> GetDefaultList() {
        return new List<Employee> {
            new Employee("Peter", 6000, 0.8f, 0.5f, 0.7f, 0.7f, 0.4f, 0.5f),
            new Employee("Amy", 3000, 0.5f, 0.4f, 0.1f, 0.9f, 0.3f, 0.5f),
            new Employee("James", 2000, 0.3f, 0.5f, 0.4f, 0.5f, 0.4f, 0.5f),
            new Employee("Derek", 4000, 0.2f, 0.8f, 0.5f, 0.3f, 0.8f, 0.8f),
            new Employee("Evan", 5000, 0.2f, 0.9f, 0.8f, 0.8f, 0.6f, 0.2f)
        };
    }
}
