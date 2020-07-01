using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownUIScript : MonoBehaviour
{
    public TextMeshProUGUI GUITextToChange;

    private void OnEnable()
    {
        GUITextToChange = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
    }


    public void ChangeCountdownNumber(string stringBeforeNumber, int numberToChangeTo, 
                                      string stringAfterNumber)
    {
        GUITextToChange.text =
            stringBeforeNumber + numberToChangeTo.ToString() + stringAfterNumber;
    }
}
