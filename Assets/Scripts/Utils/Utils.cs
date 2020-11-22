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

    public static string GetSceneNameByBuildIndex(int buildIndex) {
        return Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(buildIndex));
    }

    public static string SplitCamelCase(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "(?<=[a-z])([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
    }
}