using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Team {

    // Determine how much knowledge bonus is awarded each day
    public float BonusFactor { get; set; } = Constants.KnowledgeBonusFactor;
    public float KnowledgeAwardFactor { get; set; } = Constants.KnowledgeAwardFactor;

    private readonly TeamKnowledge teamKnowledge = new TeamKnowledge();
    public TeamKnowledge KnowledgeSnapshot => new TeamKnowledge(teamKnowledge);
    public float TeamTestingKnowledge {
        get => teamKnowledge.TeamTestingKnowledge;
        private set => teamKnowledge.TeamTestingKnowledge = value;
    }
    public float TeamOperationKnowledge {
        get => teamKnowledge.TeamOperationKnowledge;
        private set => teamKnowledge.TeamOperationKnowledge = value;
    }
    public float TeamReleaseEngKnowledge {
        get => teamKnowledge.TeamReleaseEngKnowledge;
        private set => teamKnowledge.TeamReleaseEngKnowledge = value;
    }
    public float TeamAutomationKnowledge {
        get => teamKnowledge.TeamAutomationKnowledge;
        private set => teamKnowledge.TeamAutomationKnowledge = value;
    }
    public float DevOpsKnowledge => teamKnowledge.DevOpsKnowledge;

    private readonly List<Employee> members = new List<Employee>();
    public List<Employee> CurrentMembers => new List<Employee>(members);
    

    // Gives bonus based on the team knowledge, should be called once per day
    public void TickBonus() {
        var knowledge = new[] {TeamTestingKnowledge, TeamOperationKnowledge, TeamReleaseEngKnowledge, TeamAutomationKnowledge};
        Array.Sort(knowledge);
        var knowledgeBonus = knowledge[0] * 0.35f + knowledge[1] * 0.35f + knowledge[2] * 0.15f + knowledge[3] * 0.15f;
        TeamTestingKnowledge += members.Select(x => x.TestingSkills).Sum() * KnowledgeAwardFactor;
        TeamOperationKnowledge += members.Select(x => x.OperationSkills).Sum() * KnowledgeAwardFactor;
        TeamReleaseEngKnowledge += members.Select(x => x.ReleaseEngSkills).Sum() * KnowledgeAwardFactor;
        TeamAutomationKnowledge += members.Select(x => x.AutomationSkills).Sum() * KnowledgeAwardFactor;
        TeamTestingKnowledge *= 1f + knowledgeBonus * BonusFactor;
        TeamOperationKnowledge *= 1f + knowledgeBonus * BonusFactor;
        TeamReleaseEngKnowledge *= 1f + knowledgeBonus * BonusFactor;
        TeamAutomationKnowledge *= 1f + knowledgeBonus * BonusFactor;
        TeamTestingKnowledge = Mathf.Clamp(TeamTestingKnowledge, 0.01f, 1f);
        TeamOperationKnowledge = Mathf.Clamp(TeamOperationKnowledge, 0.01f, 1f);
        TeamReleaseEngKnowledge = Mathf.Clamp(TeamReleaseEngKnowledge, 0.01f, 1f);
        TeamAutomationKnowledge = Mathf.Clamp(TeamAutomationKnowledge, 0.01f, 1f);
    }

    public void AddMember(Employee employee) {
        employee.AssignedTeam?.RemoveMember(employee);
        members.Add(employee);
        employee.AssignedTeam = this;
    }

    public void RemoveMember(Employee employee) {
        // Tries to remove employee
        if (!members.Remove(employee)) {
            Debug.LogError($"Team: Failed to remove {employee.Name}, employee not in team,");
        }
        if (employee.AssignedTeam == this) {
            employee.AssignedTeam = null;
        } else {
            Debug.LogError($"Inconsistency detected: Employee claims to be assigned to a different team. The employee will continue to reference that team");
        }
    }

    public void DebugTeamInteractive() {
        Console.WriteLine($"Team: Test {TeamTestingKnowledge}, Oper {TeamOperationKnowledge}, Release {TeamReleaseEngKnowledge}, Automate {TeamAutomationKnowledge}");
        Console.WriteLine($"DevOps knowledge: {DevOpsKnowledge}");
        Console.WriteLine($"Members: Test {members.Select(x => x.TestingSkills).Sum()}, Oper {members.Select(x => x.OperationSkills).Sum()}, Release {members.Select(x => x.ReleaseEngSkills).Sum()}, Automate {members.Select(x => x.AutomationSkills).Sum()}");
    }

}

