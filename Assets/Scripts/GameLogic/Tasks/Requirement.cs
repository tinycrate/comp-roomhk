using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Requirement {

    public string RequirementName { get; set; }
    public Func<bool> Evaluate { get; set; }
    public Func<string> EvaluateStatus { get; set; }

    public Requirement(string requirementName, Func<bool> evaluate) {
        RequirementName = requirementName;
        Evaluate = evaluate;
        EvaluateStatus = () => Evaluate() ? "Satisfied" : "Not Satisfied";
    }

    public Requirement(string requirementName, Func<bool> evaluate, Func<string> status) {
        RequirementName = requirementName;
        Evaluate = evaluate;
        EvaluateStatus = status;
    }
}