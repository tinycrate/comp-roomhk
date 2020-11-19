using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AbilityHexagon : MonoBehaviour {
    [Serializable]
    public class Stat {
        public Text DisplayText;
        public Image Triangle;
        public float Value;
        public String TextValue;
    }

    public Stat[] Stats = new Stat[6];

    public float FullSize = 100f;

    void Update() {
        SetDisplay();
    }

    void SetDisplay() {
        for (var i = 0; i < Stats.Length; i++) {
            if (Stats[i] == null || Stats[i].Triangle == null) continue;
            if (Stats[i].DisplayText != null) {
                Stats[i].DisplayText.text = Stats[i].TextValue;
            }
            Vector2 size = Stats[i].Triangle.rectTransform.sizeDelta;
            size.x = FullSize * Stats[i].Value;
            Stats[i].Triangle.rectTransform.sizeDelta = size;
            var pre = (i + Stats.Length - 1) % Stats.Length;
            size = Stats[pre].Triangle.rectTransform.sizeDelta;
            size.y = FullSize * Stats[i].Value;
            Stats[pre].Triangle.rectTransform.sizeDelta = size;
        }
    }
}
