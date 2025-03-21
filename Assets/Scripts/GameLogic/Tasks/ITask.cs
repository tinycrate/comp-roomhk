﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask {
    string Name { get; }
    List<Feature> Features { get; }
    void TickDay();
    bool Compulsory { get; }
    bool Assigned { get; set; }
    bool Completed { get; }
}
