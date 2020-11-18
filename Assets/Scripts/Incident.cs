using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Incident : MonoBehaviour
{
    private const int SIZE = 4;

    //Options
    [Header("Options")]
    public Text[] option = new Text[SIZE];
    public string[] option_text = new string[SIZE];

    void OnValidate() {
        if (option.Length != SIZE) { Array.Resize(ref option, SIZE); }
        if (option_text.Length != SIZE) { Array.Resize(ref option_text, SIZE); }
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++) {
            option[i].text = option_text[i];
        }
    }
}
