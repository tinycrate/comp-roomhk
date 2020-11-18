using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Production : IDeployable {
    public bool Deployed { get; } = true;
    public int ProductionDefectCount { get; set; } = 0;

    public float Quality { get; private set; } = 0.95f;

    public void TickDay() {
        throw new NotImplementedException();
    }

    public float TotalSatisfaction { get; set; } = 0;
}

