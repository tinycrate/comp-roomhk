﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Feature {
    public enum State {
        Idle,
        Coding,
        BugFixing,
        Testing,
        Merged
    }
    public string Name { get; private set; }
    public float Effort { get;}
    public float RemainingEffort { get; private set; }
    public float UnitTestEffort => Effort * Constants.UnitTestEffortPercentage * (1f + Difficulty * Constants.MaxTestEffortDifficultlyScaling);
    public float RemainingUnitTestEffort { get; private set; }
    public bool BuildFailed { get; private set; } = false;
    public bool TestFailed { get; private set; } = false;
    public float Difficulty { get; private set; }
    public float BaseBuildSuccessProbability => Constants.LeastBuildSuccessProbability + (1f - Constants.LeastBuildSuccessProbability) * (1f - Difficulty);
    public float BaseTestSuccessProbability => Constants.LeastTestPassProbability + (1f - Constants.LeastTestPassProbability) * (1f - Difficulty);
    public float EndProductQuality { get; private set; } = 0;

    public bool RequireCoding =>
        CurrentState == State.Idle || CurrentState == State.Coding || CurrentState == State.BugFixing;

    public bool RequireTesting => CurrentState == State.Testing;

    public delegate void OnStateChangedEventHandler(object sender, State oldState);
    public event OnStateChangedEventHandler OnStateChanged;
    public event EventHandler OnBuildFailed;
    public event EventHandler OnTestFailed;

    private readonly HashSet<Employee> coders = new HashSet<Employee>();
    private readonly HashSet<Employee> testers = new HashSet<Employee>();

    public State CurrentState {
        get {
            if (Mathf.Approximately(Effort, RemainingEffort)) {
                return State.Idle;
            }
            if (RemainingEffort <= Mathf.Epsilon) {
                return RemainingUnitTestEffort <= Mathf.Epsilon ? State.Merged : State.Testing;
            }
            return  BuildFailed || TestFailed  ? State.BugFixing : State.Coding;
        }
    }

    public Feature(string name, float effort, float difficulty) {
        Name = name;
        Effort = effort;
        Difficulty = difficulty;
        RemainingEffort = Effort;
        RemainingUnitTestEffort = UnitTestEffort;
    }

    // Returns the amount of consumed effort
    public float Code(float codingEffort, Employee employee) {
        coders.Add(employee);
        var oldState = CurrentState;
        if (!RequireCoding) {
            Debug.LogError($"Feature {Name}: Attempts to work on code on a feature that does not require coding.");
            return 0;
        }
        if (codingEffort <= Mathf.Epsilon) {
            return 0;
        }
        var consumedEffort = Mathf.Min(codingEffort, RemainingEffort);
        RemainingEffort -= consumedEffort;
        if (RemainingEffort <= Mathf.Epsilon) {
            var buildSuccessBonus = Mathf.Min(1f, coders.Sum(x => x.Experience) / (Constants.MaxRequiredExperiencePoints * Difficulty));
            var advanceProbability = BaseBuildSuccessProbability + (1 - BaseBuildSuccessProbability) * buildSuccessBonus;
            GameManager.GetInstance.StatManager.TotalBuildCount += 1;
            if (Random.value > advanceProbability) {
                RemainingEffort = Effort * Constants.BuildFailEffortPenalty;
                BuildFailed = true;
                TestFailed = false;
                GameManager.GetInstance.StatManager.BuildFailureCount += 1;
                OnBuildFailed?.Invoke(this, EventArgs.Empty);
            } else {
                BuildFailed = false;
                TestFailed = false;
            }
        }
        if (CurrentState != oldState) {
            OnStateChanged?.Invoke(this, oldState);
        }
        return consumedEffort;
    }

    // Returns the amount of consumed effort
    public float Test(float testingEffort, Employee employee) {
        testers.Add(employee);
        var oldState = CurrentState;
        if (!RequireTesting) {
            Debug.LogError($"Feature {Name}: Attempts to work on test on a feature that does not require testing.");
            return 0;
        }
        if (testingEffort <= Mathf.Epsilon) {
            return 0;
        }
        var consumedEffort = Mathf.Min(testingEffort, RemainingUnitTestEffort);
        RemainingUnitTestEffort -= consumedEffort;
        if (RemainingUnitTestEffort <= Mathf.Epsilon) {
            var testSuccessBonus = Mathf.Min(
                1f,
                testers.Sum(x => x.TestingSkills) / (Constants.MaxRequiredTestingPoints * Difficulty) +
                Constants.MaxTestProbabilityBonusFromTeam *
                employee.AssignedTeam.TeamTestingKnowledge
            );
            var advanceProbability = BaseTestSuccessProbability + (1 - BaseTestSuccessProbability) * testSuccessBonus;
            GameManager.GetInstance.StatManager.TotalTestCount += 1;
            if (Random.value > advanceProbability) {
                RemainingUnitTestEffort = UnitTestEffort * Constants.TestFailTestEffortPenalty;
                RemainingEffort = Effort * Constants.TestFailEffortPenalty;
                TestFailed = true;
                BuildFailed = false;
                GameManager.GetInstance.StatManager.TestFailureCount += 1;
                OnTestFailed?.Invoke(this, EventArgs.Empty);
            } else {
                BuildFailed = false;
                TestFailed = false;
                FinishFeature();
            }
        }
        if (CurrentState != oldState) {
            OnStateChanged?.Invoke(this, oldState);
        }
        return consumedEffort;
    }

    private void FinishFeature() {
        var codersExperience = coders.Sum(x => x.Experience) / (Constants.MaxRequiredExperiencePoints * Difficulty);
        var unitTestQuality = testers.Sum(x => x.TestingSkills) * Constants.TesterSkillPointBonus;
        EndProductQuality = Mathf.Min(1, codersExperience + unitTestQuality);
    }

}