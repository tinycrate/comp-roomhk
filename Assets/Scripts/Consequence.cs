using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Consequence : MonoBehaviour
{
    public Text consequence_text;
    [SerializeField] private string consequence_input;
    // Start is called before the first frame update
    void Start()
    {
        consequence_input = "Lorem ipsum dolor sit amet, at primis nostrum fabellas pri, \\" +
            "ea delectus placerat disputationi sit, ne vel novum iisque euismod. Saperet torquatos ut sed. \\" +
            "Ex vim purto platonem. Et pri feugiat similique, iudico volutpat assentior ut mei. Mea ut \\" +
            "nominati reformidans, per adhuc deleniti eu, vix no dico delectus erroribus.";
    }

    // Update is called once per frame
    void Update()
    {
        consequence_text.text = consequence_input;
    }
}
