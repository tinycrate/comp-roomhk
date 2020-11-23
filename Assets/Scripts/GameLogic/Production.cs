using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Production : IDeployable {
    public bool Deployed { get; } = true;
    public int ProductionDefectCount => 0;
    public event EventHandler<Outage> OnProductionDowntime;
    public float LastDowntimeRecoveredDay { get; set; } = 0;

    public float Quality => Constants.BaseProductionQuality - Mathf.Min(
                                Constants.MaxProductionQualityPunishment,
                                Constants.ProductionQualityPunishmentPerDefect *
                                GameManager.GetInstance.StatManager.UnfixedProductionDefects
                            );

    public float ScheduledDowntime { get; set; } = 0f;

    public void TickDay() {
        if (ScheduledDowntime >= Mathf.Epsilon) {
            IntroduceDowntime(ScheduledDowntime, "Scheduled Downtime");
            ScheduledDowntime = 0f;
            return;
        }
        if (Random.value > Quality) {
            var downtime = (1f - Quality) * (1f - Constants.MaxDowntimeReductionBonus *
                                             GameManager.GetInstance.CurrentTeam.TeamOperationKnowledge);
            IntroduceDowntime(downtime, "Unplanned Downtime");
        } else {
            TotalSatisfaction += Constants.BaseProductionSatisfactionValue * Mathf.Max(
                                     0,
                                     (Quality - Constants.MinimumRequiredProductionQuality) /
                                     (1f - Constants.MinimumRequiredProductionQuality)
                                 );
        }
    }

    private void IntroduceDowntime(float downtime, string description) {
        var outages = new Outage(
            GameManager.GetInstance.StatManager.DayPassed,downtime,
            GameManager.GetInstance.StatManager.DayPassed - LastDowntimeRecoveredDay,
            description
        );
        LastDowntimeRecoveredDay = GameManager.GetInstance.StatManager.DayPassed + downtime;
        GameManager.GetInstance.StatManager.Outages.Add(outages);
        OnProductionDowntime?.Invoke(this, outages);
    }

    public event EventHandler<float> OnSatisfactionChange;

    private float totalSatisfaction = 0;
    public float TotalSatisfaction {
        get => totalSatisfaction;
        set {
            var diff = value - totalSatisfaction;
            totalSatisfaction = value;
            if (diff < Mathf.Epsilon) {
                OnSatisfactionChange?.Invoke(this, diff);
            }
        }
    }
}

