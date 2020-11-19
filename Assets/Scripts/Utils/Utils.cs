using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public static float MoveTowardsProportion(float current, float target, float minDelta, float proportion) {
        return Mathf.MoveTowards(
            current,
            target,
            Mathf.Max(minDelta, Mathf.Abs(target - current) * proportion)
        );
    }
}