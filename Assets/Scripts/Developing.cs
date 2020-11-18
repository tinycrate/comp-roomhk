using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Developing : MonoBehaviour
{
    private const int SIZE = 3;
    private string[] tick_cross_option = { "✗", "✓" };

    // Task Name
    [Header("Task Name")]
    public Text task_name;
    [SerializeField] private string task_text;

    // Feature Name
    [Header("Task Name")]
    public Text[] feature;
    [SerializeField] private string[] feature_text = new string[SIZE];

    // Deploy and Release
    [Header("Deploy and Release")]
    public Image release_bar;
    public Image deploy_bar;
    [Range(0, 1)] [SerializeField] private float deploy_progress;
    [Range(0, 1)] [SerializeField] private float release_progress;

    // Code progress
    [Header("Code")]
    public Text[] code = new Text[SIZE];
    [Range(0, 100)] [SerializeField] private int[] code_progress = new int[SIZE];

    // Build progress
    [Header("Build")]
    public Text[] build = new Text[SIZE];
    [SerializeField] private bool[] build_progress = new bool[SIZE];
    
    // Test progress
    [Header("Test")]
    public Text[] test = new Text[SIZE];
    [Range(0, 100)] [SerializeField] private int[] test_progress = new int[SIZE];
    [SerializeField] private bool[] test_pass = new bool[SIZE];


    // Merge progress
    [Header("Merge")]
    public Text[] merge = new Text[SIZE];
    [SerializeField] private bool[] merge_progress = new bool[SIZE];


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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++) {
            code[i].text = "0%";
            test[i].text = "0%";
            build_progress[i] = false;
            merge_progress[i] = false;
            test_pass[i] = true;
        }
        task_text = "default task name";
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
            code[i].text = code_progress[i] + "%"; //code

            if (test_pass[i] == true) { test[i].text = test_progress[i] + "%"; }
            else { test[i].text = tick_cross_option[0]; } //test

            if (build_progress[i] == false) { build[i].text = tick_cross_option[0]; }
            else { build[i].text = tick_cross_option[1]; } //build

            if (merge_progress[i] == false) { merge[i].text = tick_cross_option[0]; }
            else { merge[i].text = tick_cross_option[1]; } //merge

            feature[i].text = feature_text[i]; //feature name
        }

    }
}
