using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DeployableService {
    void TickDay();
    bool Deployed { get; }
}
