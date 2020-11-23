using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeTask : ITask {
    public string Name { get; set; }
    public string EffectDescription { get; set; }
    public Requirement ShowRequirement { get; private set; }
    public List<Requirement> UnlockRequirements { get; private set; }
    public List<Feature> Features { get; set; }
    public void TickDay() {
        throw new NotImplementedException();
    }

    public bool Compulsory => false;
    public bool Assigned { get; set; }
    public bool Completed { get; set; }
    public event EventHandler OnTaskCompleted;

    public UpgradeTask(string name, string effectDescription, Requirement showRequirement, List<Requirement> unlockRequirements, List<Feature> features) {
        Name = name;
        EffectDescription = effectDescription;
        ShowRequirement = showRequirement;
        UnlockRequirements = unlockRequirements;
        Features = features;
    }
}
