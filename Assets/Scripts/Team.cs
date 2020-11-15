using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Team {

    // Determine how much knowledge bonus is awarded each day
    public float BonusFactor { get; set; } = Constants.KnowledgeBonusFactor;
    public float KnowledgeAwardFactor { get; set; } = Constants.KnowledgeAwardFactor;

    public float TeamTestingKnowledge { get; private set; } = 0.01f;
    public float TeamOperationKnowledge { get; private set; } = 0.01f;
    public float TeamReleaseEngKnowledge { get; private set; } = 0.01f;
    public float TeamAutomationKnowledge { get; private set; } = 0.01f;


    public float DevOpsKnowledge => 0.25f * TeamTestingKnowledge + 0.25f * TeamOperationKnowledge +
                                    0.25f * TeamReleaseEngKnowledge + 0.25f * TeamAutomationKnowledge;

    public List<Employee> Members { get; } = new List<Employee>();
    

    // Gives bonus based on the team knowledge, should be called once per day
    public void TickBonus() {
        var knowledge = new[] {TeamTestingKnowledge, TeamOperationKnowledge, TeamReleaseEngKnowledge, TeamAutomationKnowledge};
        Array.Sort(knowledge);
        var knowledgeBonus = knowledge[0] * 0.35f + knowledge[1] * 0.35f + knowledge[2] * 0.15f + knowledge[3] * 0.15f;
        TeamTestingKnowledge += Members.Select(x => x.TestingSkills).Sum() * KnowledgeAwardFactor;
        TeamOperationKnowledge += Members.Select(x => x.OperationSkills).Sum() * KnowledgeAwardFactor;
        TeamReleaseEngKnowledge += Members.Select(x => x.ReleaseEngSkills).Sum() * KnowledgeAwardFactor;
        TeamAutomationKnowledge += Members.Select(x => x.AutomationSkills).Sum() * KnowledgeAwardFactor;
        TeamTestingKnowledge *= 1f + knowledgeBonus * BonusFactor;
        TeamOperationKnowledge *= 1f + knowledgeBonus * BonusFactor;
        TeamReleaseEngKnowledge *= 1f + knowledgeBonus * BonusFactor;
        TeamAutomationKnowledge *= 1f + knowledgeBonus * BonusFactor;
        TeamTestingKnowledge = Mathf.Clamp(TeamTestingKnowledge, 0.01f, 1f);
        TeamOperationKnowledge = Mathf.Clamp(TeamOperationKnowledge, 0.01f, 1f);
        TeamReleaseEngKnowledge = Mathf.Clamp(TeamReleaseEngKnowledge, 0.01f, 1f);
        TeamAutomationKnowledge = Mathf.Clamp(TeamAutomationKnowledge, 0.01f, 1f);
    }

    public void DebugTeamInteractive() {
        Console.WriteLine($"Team: Test {TeamTestingKnowledge}, Oper {TeamOperationKnowledge}, Release {TeamReleaseEngKnowledge}, Automate {TeamAutomationKnowledge}");
        Console.WriteLine($"DevOps knowledge: {DevOpsKnowledge}");
        Console.WriteLine($"Members: Test {Members.Select(x => x.TestingSkills).Sum()}, Oper {Members.Select(x => x.OperationSkills).Sum()}, Release {Members.Select(x => x.ReleaseEngSkills).Sum()}, Automate {Members.Select(x => x.AutomationSkills).Sum()}");
    }

}

