using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DistanceSensorScript : MonoBehaviour
{
    public int distanceSensorTriggeredidx;
    public bool isTriggered = false;
    private RaycastHit CubeSensorBlocker;
    public GameObject LazerVisualiser;

    public GameObject Spotlight;

    private ConnectionScript mainConnectionScript;

    

    



    private void Start()
    {
        Spotlight = this.transform.parent.transform.Find("Spotlight").gameObject;
        mainConnectionScript = GameObject.Find("EventSystem").GetComponent<ConnectionScript>();
        
        if (SceneManager.GetActiveScene().name == "SeekerPlayerScene")
        {
            
            this.transform.parent.Find("Cylinder").gameObject.SetActive(false);
            StartCoroutine(CheckIfTriggeredDomoticz());
        }
        StartCoroutine(mainConnectionScript.ChangeDomoticzSwitchStatus(distanceSensorTriggeredidx, false));
    }


    //TODO: Complete. 
    // Function scales lazer. Current method: Hand-scale the lazer. Is only for looks.
    //public void DetermineLazerLength()
    //{
    //    Physics.Raycast(transform.position, transform.forward, hitInfo: out CubeSensorBlocker, 50, (1 << 11 | 1 << 12));
    //    LazerVisualiser.transform.localScale = new Vector3(LazerVisualiser.transform.localScale.x, CubeSensorBlocker.distance, LazerVisualiser.transform.localScale.z);
    //    LazerVisualiser.transform.position = new Vector3(LazerVisualiser.transform.position.x, LazerVisualiser.transform.position.y, CubeSensorBlocker.distance / 2);
    //    Debug.Log(CubeSensorBlocker.distance);
    //}

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "HiderPlayerScene")
        {
            CheckIfTriggeredInGame();
        }
    }

    public void CheckIfTriggeredInGame()
    {
        
    // Doing it this way is not efficient game design. See top comments for explaination.

    // Update Laser Sensor in Domoticz when player walks through sensor (so distance is less than normal)
    // Trigger DistanceSensorTriggered switch if ghost walked through sensor

        Physics.Raycast(transform.position, transform.forward, hitInfo: out CubeSensorBlocker, 50, (1 << 11 | 1 << 12));
        if ((isTriggered == false) && CubeSensorBlocker.transform.gameObject.name != "CubeSensorBlocker")
        {
            //StartCoroutine(mainConnectionScript.ChangeDomoticzSwitchStatus(distanceSensorTriggeredidx, true));
            // Calc scales up unity Units to realistic cm size
            // TODO: Does not scale correctly, rounds to meters
            // StartCoroutine(mainConnectionScript.ChangeDomoticzDistanceSensorDistance(distanceSensoridx, (Mathf.RoundToInt(CubeSensorBlocker.distance) / 2 * 100)));
            Debug.Log(CubeSensorBlocker.transform.gameObject.name);
            TriggerSensor();
        }
    }

    public void TriggerSensor()
    {
        isTriggered = true;
        Spotlight.SetActive(true);
        this.transform.parent.Find("Cylinder").gameObject.SetActive(false);
        StartCoroutine(mainConnectionScript.ChangeDomoticzSwitchStatus(distanceSensorTriggeredidx, true));
    }

    public IEnumerator CheckIfTriggeredDomoticz()
    {
        SwitchResult DistanceSensorSwitch = new SwitchResult();
        yield return StartCoroutine(mainConnectionScript.CheckDomoticzSwitch(distanceSensorTriggeredidx, DistanceSensorSwitch));
        // Needs to be dynamic! 
        //yield return new WaitForSeconds(1.0f);
        if (DistanceSensorSwitch.Data == "On")
        {
            Debug.Log("Lmao!");
            TriggerSensor();
        }


    }

    private void DisableSensorForSeeker()
    {

    }
}
