using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvailabilityReport {
    public float TimeToRepair { get; set; }
    public float TimeToFailure { get; set; }

    public static float CalculateMTTR(List<AvailabilityReport> outages) {
        if (!outages.Any()) return 0;
        return outages.Sum(x => x.TimeToRepair) / outages.Count();
    }

    public static float CalculateMTTF(List<AvailabilityReport> outages) {
        if (!outages.Any()) return 0;
        return outages.Sum(x => x.TimeToFailure) / outages.Count();
    }

    public static float CalculateAvailability(List<AvailabilityReport> outages) {
        if (!outages.Any()) return 1;
        var MTTF = CalculateMTTF(outages);
        var MTTR = CalculateMTTR(outages);
        return MTTF / (MTTF + MTTR);
    }

    public AvailabilityReport(float timeToRepair, float timeToFailure) {
        TimeToRepair = timeToRepair;
        TimeToFailure = timeToFailure;
    }
}
