using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleTask : ITask {
    public enum TaskNature {
        Upgrade,
        Fix,
        Improve,
        Custom
    }

    public enum TaskCompletionMethod {
        Instant, // Just accept if the requirements are met
        EmployeeManaged, // Completes the Features list like normal (build => test => deploy)
        CustomManaged // Assign employee via AssignEmployee and calculate from OnEmployeeWork()
    }

    public readonly Dictionary<string, object> ObjectState = new Dictionary<string, object>();
    public readonly Dictionary<string, int> IntState = new Dictionary<string, int>();
    public readonly Dictionary<string, float> FloatState = new Dictionary<string, float>();
    public readonly Dictionary<string, string> StringState = new Dictionary<string, string>();
    public string EffectDescription { get; set; }
    public string TaskDescription { get; set; }
    public Requirement ShowRequirement { get; private set; }
    public List<Requirement> UnlockRequirements { get; private set; }
    public Action OnTaskComplete { get; set; }

    public event EventHandler TaskCompleted;

    public TaskNature Nature { get; set; }

    public bool CustomCompletionRequiresEmployees { get; set; }

    public float CustomProgress { get; set; } = 0;

    public float Progress {
        get {
            if (Completed) return 1;
            if (CompletionMethod == TaskCompletionMethod.EmployeeManaged) {
                return 1f - Features.Sum(x => x.RemainingEffort + x.RemainingUnitTestEffort) /
                       Features.Sum(x => x.Effort + x.UnitTestEffort);
            }
            return CustomProgress;
        }
    }

    public bool Accepted { get; private set; } = false;

    public List<Employee> AssignedEmployees { get; set; } = new List<Employee>();

    // Returns false when the task is not completed, true when the task is finished
    public Func<SimpleTask, bool> OnWork { get; set; }

    public TaskCompletionMethod CompletionMethod { get; private set; }
    public bool Shown { get; private set; }
    public bool Unlocked => UnlockRequirements.All(x => x.Evaluate());

    public string UnlockRequirementStatus => UnlockRequirements.Aggregate(
        "",
        (s, requirement) => s + $"\n{requirement.RequirementName}: {requirement.EvaluateStatus()}"
    );

    public string Name { get; set; }
    public List<Feature> Features { get; set; }

    public void TickDay() {
        if (!Shown && ShowRequirement.Evaluate()) {
            MainGameSceneManager.GetInstance.ShowTaskOnTaskList(this);
            Shown = true;
        }
        if (Accepted && !Completed) {
            OnExecute();
        }
    }

    public bool Compulsory => false;

    private bool assigned = false;
    public bool Assigned {
        get => CompletionMethod == TaskCompletionMethod.CustomManaged ? AssignedEmployees.Any() : assigned;
        set {
            if (CompletionMethod != TaskCompletionMethod.CustomManaged) assigned = value;
        }
    }

    public bool Completed { get; private set; }

    public void AcceptAndExecuteTask() {
        Accepted = true;
        if (CompletionMethod == TaskCompletionMethod.Instant) {
            OnTaskComplete?.Invoke();
            Completed = true;
            return;
        }
        OnExecute();
    }

    private void OnExecute() {
        if (CompletionMethod == TaskCompletionMethod.EmployeeManaged &&
            !Features.TrueForAll(x => x.CurrentState == Feature.State.Merged)) {
            return;
        }
        if (CompletionMethod == TaskCompletionMethod.CustomManaged && !OnWork(this)) {
            return;
        }
        OnTaskComplete.Invoke();
        Completed = true;
        TaskCompleted?.Invoke(this, EventArgs.Empty);
    }

    public static SimpleTask CreateTaskEmployeeManaged(
        TaskNature nature,
        string name,
        string effectDescription,
        string taskDescription,
        Requirement showRequirement,
        List<Requirement> unlockRequirements,
        List<Feature> features,
        Action onTaskComplete
    ) {
        return new SimpleTask {
            Nature = nature,
            Name = name,
            EffectDescription = effectDescription,
            TaskDescription = taskDescription,
            ShowRequirement = showRequirement,
            UnlockRequirements = unlockRequirements,
            Features = features,
            OnTaskComplete = onTaskComplete,
            CompletionMethod = TaskCompletionMethod.EmployeeManaged
        };
    }

    public static SimpleTask CreateTaskCustom(
        TaskNature nature,
        string name,
        string effectDescription,
        string taskDescription,
        Requirement showRequirement,
        List<Requirement> unlockRequirements,
        Action onTaskComplete,
        Func<SimpleTask, bool> onWork,
        bool requiresEmployees
    ) {
        return new SimpleTask {
            Nature = nature,
            Name = name,
            EffectDescription = effectDescription,
            TaskDescription = taskDescription,
            ShowRequirement = showRequirement,
            UnlockRequirements = unlockRequirements,
            Features = new List<Feature>(),
            OnTaskComplete = onTaskComplete,
            OnWork = onWork,
            CustomCompletionRequiresEmployees = requiresEmployees,
            CompletionMethod = TaskCompletionMethod.CustomManaged
        };
    }

    public static SimpleTask CreateTaskInstant(
        TaskNature nature,
        string name,
        string effectDescription,
        string taskDescription,
        Requirement showRequirement,
        List<Requirement> unlockRequirements,
        Action onTaskComplete
    ) {
        return new SimpleTask {
            Nature = nature,
            Name = name,
            EffectDescription = effectDescription,
            TaskDescription = taskDescription,
            ShowRequirement = showRequirement,
            UnlockRequirements = unlockRequirements,
            Features = new List<Feature>(),
            OnTaskComplete = onTaskComplete,
            CompletionMethod = TaskCompletionMethod.Instant
        };
    }
}