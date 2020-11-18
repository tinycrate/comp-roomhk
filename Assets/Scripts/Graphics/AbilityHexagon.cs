using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHexagon : MonoBehaviour
{

    [SerializeField]
    private Image[] m_triangles;
    [SerializeField]
    private float m_fullSize = 100f;
    [SerializeField]
    private float[] m_statusValues;

    void OnValidate() {
        if (m_statusValues.Length != m_triangles.Length) {
            return;
        }

        for (int i = 0; i < 6; i++) {
            SetValue (i, m_statusValues[i]);
        }
    }

    public void SetValue (int index, float value) {
        m_statusValues[index] = value;

        Vector2 size = m_triangles[index].rectTransform.sizeDelta;
        size.x = m_fullSize * value;
        m_triangles[index].rectTransform.sizeDelta = size;

        int pre = (index + m_triangles.Length - 1) % m_triangles.Length;
        size = m_triangles[pre].rectTransform.sizeDelta;
        size.y = m_fullSize * value;
        m_triangles[pre].rectTransform.sizeDelta = size;
    }
}
