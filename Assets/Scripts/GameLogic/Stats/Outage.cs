using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Outage {
    public string OutageName { get; set; }
    public float ReportDate { get; set; }
    public float TimeToRepair { get; set; }
    public float TimeToFailure { get; set; }

    public static float CalculateMTTR(List<Outage> outages) {
        if (!outages.Any()) return 0;
        return outages.Sum(x => x.TimeToRepair) / outages.Count();
    }

    public static float CalculateMTTF(List<Outage> outages, float currentDay = -1) {
        if (!outages.Any()) return 0;
        var lastFailuresDay = outages.Sum(x => x.TimeToFailure + x.TimeToRepair);
        if (currentDay < 0) currentDay = lastFailuresDay;
        return (outages.Sum(x => x.TimeToFailure) + (currentDay - lastFailuresDay)) / outages.Count();
    }

    public static float CalculateAvailability(List<Outage> outages, float currentDay = -1) {
        if (!outages.Any()) return 1;
        var MTTF = CalculateMTTF(outages, currentDay);
        var MTTR = CalculateMTTR(outages);
        return MTTF / (MTTF + MTTR);
    }

    public Outage(float reportDate, float timeToRepair, float timeToFailure, string outageName = "Unplanned Downtime") {
        ReportDate = reportDate;
        TimeToRepair = timeToRepair;
        TimeToFailure = timeToFailure;
        OutageName = outageName;
    }
}
