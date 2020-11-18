using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Developing : MonoBehaviour
{
    private const int SIZE = 3;
    private string[] tick_cross_string = { "✗", "✓" };
    public enum tick_cross_option { none, tick, cross};

    // Task Name
    [Header("Task Name")]
    public Text task_name;
    public string task_text;

    // Feature Name
    [Header("Task Name")]
    public Text[] feature;
    public string[] feature_text = new string[SIZE];

    // Deploy and Release
    [Header("Deploy and Release")]
    public Image release_bar;
    public Image deploy_bar;
    [Range(0, 1)] public float deploy_progress = 0;
    [Range(0, 1)] public float release_progress = 0;

    // Code progress
    [Header("Code")]
    public Text[] code = new Text[SIZE];
    [Range(0, 1)] public float[] code_progress = new float[SIZE];

    // Build progress
    [Header("Build")]
    public Text[] build = new Text[SIZE];
    public tick_cross_option[] build_progress = new tick_cross_option[SIZE];
    //public bool[] build_progress = new bool[SIZE];
    
    // Test progress
    [Header("Test")]
    public Text[] test = new Text[SIZE];
    [Range(0, 1)] public float[] test_progress = new float[SIZE];
    public bool[] test_pass = new bool[SIZE];


    // Merge progress
    [Header("Merge")]
    public Text[] merge = new Text[SIZE];
    public tick_cross_option[] merge_progress = new tick_cross_option[SIZE];
    //public bool[] merge_progress = new bool[SIZE];


    void OnValidate() {
        if (feature.Length != SIZE) { Array.Resize(ref feature, SIZE); }
        if (feature_text.Length != SIZE) { Array.Resize(ref feature_text, SIZE); }
        if (code.Length != SIZE) { Array.Resize(ref code, SIZE); }
        if (code_progress.Length != SIZE) { Array.Resize(ref code_progress, SIZE); }
        if (build.Length != SIZE) { Array.Resize(ref build, SIZE); }
        if (build_progress.Length != SIZE) { Array.Resize(ref build_progress, SIZE); }
        if (test.Length != SIZE) { Array.Resize(ref test, SIZE); }
        if (test_progress.Length != SIZE) { Array.Resize(ref test_progress, SIZE); }
        if (test_pass.Length != SIZE) { Array.Resize(ref test_pass, SIZE); }
        if (merge.Length != SIZE) { Array.Resize(ref merge, SIZE); }
        if (merge_progress.Length != SIZE) { Array.Resize(ref merge_progress, SIZE); }
    }

    // Update is called once per frame
    void Update()
    {
        // Deploy and Release
        deploy_bar.fillAmount = deploy_progress;
        release_bar.fillAmount = release_progress;

        // Task Name 
        task_name.text = task_text; //task name

        for (int i = 0; i < 3; i++)
        {
            code[i].text = (int)(code_progress[i] * 100) + "%"; //code

            if (test_pass[i] == false) { test[i].text = (int)(test_progress[i] * 100) + "%"; }
            else { test[i].text = tick_cross_string[0]; } //test

            if (build_progress[i] == tick_cross_option.cross) { build[i].text = tick_cross_string[0]; }
            else if (build_progress[i] == tick_cross_option.tick) { build[i].text = tick_cross_string[1]; }
            else if (build_progress[i] == tick_cross_option.none) { build[i].text = " "; } //build

            if (merge_progress[i] == tick_cross_option.cross) { merge[i].text = tick_cross_string[0]; }
            else if (merge_progress[i] == tick_cross_option.tick) { merge[i].text = tick_cross_string[1]; }
            else if (merge_progress[i] == tick_cross_option.none) { merge[i].text = " "; } //merge

            feature[i].text = feature_text[i]; //feature name
        }

    }
}
