using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class MicroService : Task, DeployableService {
    public string Name { get; }
    public List<Feature> Features { get; } = new List<Feature>();
    public event EventHandler OnTaskCompleted;
    public bool Deployed { get; }

    public float EndProductQuality {
        get {
            if (!Deployed || Features.Count <= 0) return 0;
            if (Features.Any(x => x.CurrentState != Feature.State.Merged)) return 0;
            return Features.Sum(x => x.EndProductQuality) / Features.Count;
        }
    }

    public void TickDay() {
        if (!Deployed) return;
    }
}
