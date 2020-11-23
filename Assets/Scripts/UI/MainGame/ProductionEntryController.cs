using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ProductionEntryController : MonoBehaviour {

    public Text TaskTypeText;
    public Text UptimeText;
    public Text SatisfactionText;
    public Text QualityText;

    private float displayingSatisfaction = 0;
    private float totalSatisfaction = 0;

    void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            MainGameSceneManager.GetInstance.ShowProductionView();
        });
        UpdateTaskAfterDayTick();
        GameManager.GetInstance.AfterDayTick += AfterDayTick;
        if (IsDebugging) {
            Debug.LogWarning("Debug mode activated: Production quality will always be shown");
        }
    }

    private void AfterDayTick(object sender, EventArgs e) {
        UpdateTaskAfterDayTick();
    }

    void OnDestroy() {
        GameManager.GetInstance.AfterDayTick -= AfterDayTick;
    }

    void Update() {
        displayingSatisfaction = Utils.MoveTowardsProportion(
            displayingSatisfaction,
            totalSatisfaction,
            2f,
            Time.deltaTime / 0.15f
        );
        SatisfactionText.text = $"Satisfaction: {displayingSatisfaction:0}";
    }

    private void UpdateTaskAfterDayTick() {
        var production = GameManager.GetInstance.ProductionService;
        if (production == null) return;
        totalSatisfaction = GameManager.GetInstance.StatManager.UserSatisfaction;
        if (GameManager.GetInstance.DisplayEndProductQuality || IsDebugging) {
            QualityText.text =
                $"Quality: {Mathf.RoundToInt(production.Quality * 100)}/100";
        } else {
            QualityText.text = $"Quality: ???/100";
        }
        if (IsDebugging) {
            QualityText.text += "\n(Debug mode)";
        }
        UptimeText.text = $"Uptime: {GameManager.GetInstance.StatManager.ProductionAvailability * 100:0.####}%";
    }

    private bool IsDebugging => GameManager.GetInstance.IsDebugging;
}
