using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeployable {
    void TickDay();
    float TotalSatisfaction { get; set; }
    event EventHandler<float> OnSatisfactionChange;
    bool Deployed { get; }
    int ProductionDefectCount { get; }
}
