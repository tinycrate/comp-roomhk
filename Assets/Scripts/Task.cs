using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Task {
    String Name { get; }
    List<Feature> Features { get; }
    event EventHandler OnTaskCompleted;
}
