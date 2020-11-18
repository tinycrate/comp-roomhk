using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamKnowledge {
    public float TeamTestingKnowledge { get; set; } = 0.01f;
    public float TeamOperationKnowledge { get; set; } = 0.01f;
    public float TeamReleaseEngKnowledge { get; set; } = 0.01f;
    public float TeamAutomationKnowledge { get; set; } = 0.01f;

    public float DevOpsKnowledge => 0.25f * TeamTestingKnowledge + 0.25f * TeamOperationKnowledge +
                                    0.25f * TeamReleaseEngKnowledge + 0.25f * TeamAutomationKnowledge;

    public TeamKnowledge() { }
    public TeamKnowledge(TeamKnowledge knowledge) {
        TeamTestingKnowledge = knowledge.TeamTestingKnowledge;
        TeamOperationKnowledge = knowledge.TeamOperationKnowledge;
        TeamReleaseEngKnowledge = knowledge.TeamReleaseEngKnowledge;
        TeamAutomationKnowledge = knowledge.TeamAutomationKnowledge;
    }
}