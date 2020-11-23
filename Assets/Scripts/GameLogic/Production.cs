﻿using System;
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

    public void TickDay() {
        if (Random.value > Quality) {
            var downtime = (1f - Quality) * (1f - Constants.MaxDowntimeReductionBonus *
                                             GameManager.GetInstance.CurrentTeam.TeamOperationKnowledge);
            var outages = new Outage(
                GameManager.GetInstance.StatManager.DayPassed,downtime,
                GameManager.GetInstance.StatManager.DayPassed - LastDowntimeRecoveredDay
            );
            LastDowntimeRecoveredDay = GameManager.GetInstance.StatManager.DayPassed + downtime;
            GameManager.GetInstance.StatManager.Outages.Add(outages);
            OnProductionDowntime?.Invoke(this, outages);
        } else {
            TotalSatisfaction += Constants.BaseProductionSatisfactionValue * Mathf.Max(
                                     0,
                                     (Quality - Constants.MinimumRequiredProductionQuality) /
                                     (1f - Constants.MinimumRequiredProductionQuality)
                                 );
        }
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

