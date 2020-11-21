using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask {
    string Name { get; }
    List<Feature> Features { get; }
    bool Compulsory { get; }
    bool Completed { get; }
    event EventHandler OnTaskCompleted;
}
