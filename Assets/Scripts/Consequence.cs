using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Consequence : MonoBehaviour
{
    public Text consequence_text;
    public string consequence_input;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        consequence_text.text = consequence_input;
    }
}
