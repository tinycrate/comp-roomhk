using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils {
    public static float MoveTowardsProportion(float current, float target, float minDelta, float proportion) {
        return Mathf.MoveTowards(
            current,
            target,
            Mathf.Max(minDelta, Mathf.Abs(target - current) * proportion)
        );
    }

    public static String GetSceneNameByBuildIndex(int buildIndex) {
        return Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(buildIndex));
    }
}