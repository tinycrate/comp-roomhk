using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts;

public class LineChartDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var chart = gameObject.GetComponent<LineChart>();
        chart.title.show = true;
        chart.title.text = "Line Chart Script Demo";
        for (int i = 0; i < 10; i++)
        {
            chart.AddXAxisData("x" + i);
            chart.AddData(0, Random.Range(10, 20));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
