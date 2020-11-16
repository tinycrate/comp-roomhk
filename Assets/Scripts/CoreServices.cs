using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CoreServices : MonoBehaviour, DeployableService {
    public bool Deployed { get; } = true;

    public void TickDay() {
        throw new NotImplementedException();
    }
}

