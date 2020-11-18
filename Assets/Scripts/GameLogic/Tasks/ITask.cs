using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask {
    String Name { get; }
    List<Feature> Features { get; }

    bool Completed { get; }
    event EventHandler OnTaskCompleted;
}
