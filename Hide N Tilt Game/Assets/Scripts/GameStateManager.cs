using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameObject CanvasPlayerUI;
    public TurnMapScript MainTurnMapScript;
    public ConnectionScript MainConnectionScript;

    private int gameStartedSwitchidx = 59;

    public int hiderIsCaughtSwitchidx = 60;
    private int hiderInGameSwitchidx = 61;

    public int secondsToHide = 5;
    public int secondsToFind = 3;

    public bool hiderIsHidden = false;
    public bool hiderIsFound = false;


    // Start is called before the first frame update
    void Start()
    {

        MainConnectionScript = GameObject.Find("EventSystem").GetComponent<ConnectionScript>();
        MainTurnMapScript = GameObject.Find("Map").GetComponent<TurnMapScript>();
        CanvasPlayerUI = GameObject.Find("CanvasPlayerUI");
        for (int i = 59; i <= 61; i++)
        {
            StartCoroutine(MainConnectionScript.ChangeDomoticzSwitchStatus(i, false));
        }
        WaitForGameToStart(SceneManager.GetActiveScene().name);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WaitForGameToStart(string hiderOrSeeker)
    {
        SwitchResult GameStartedSwitch = new SwitchResult();
        if (hiderOrSeeker == "HiderPlayerScene")
        {
            MainTurnMapScript.canTurn = false;
            CanvasPlayerUI.transform.Find("WaitingForStartHider").gameObject.SetActive(true);
            StartCoroutine(CheckIfGameStarted(GameStartedSwitch));
        }

        if (hiderOrSeeker == "SeekerPlayerScene")
        {
            // disable locking cursor to mid of screen
            Cursor.lockState = CursorLockMode.Confined;
            // Disable movement
            GameObject SeekerPlayerGameObject = GameObject.Find("Seeker");
            SeekerPlayerGameObject.GetComponent<PlayerMovementScript>().CanMove = false;

            // Activate UI with a button. If button pressed: excecute SeekerInitiatedStartGame();
            CanvasPlayerUI.transform.Find("WaitingForStartSeeker").gameObject.SetActive(true);
        }
    }

    public IEnumerator CheckIfGameStarted(SwitchResult GameStartedSwitch)
    {
        while (GameStartedSwitch.Data != "On")
        {
            yield return StartCoroutine(MainConnectionScript.CheckDomoticzSwitch(gameStartedSwitchidx, GameStartedSwitch));

        }
        StartHiderHidingFase();
    }


    public void SeekerInitiatedStartGAme()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CanvasPlayerUI.transform.Find("WaitingForStartSeeker").gameObject.SetActive(false);
        StartCoroutine(MainConnectionScript.ChangeDomoticzSwitchStatus(gameStartedSwitchidx, true));
        StartCoroutine(StartSeekerWaitingFase());
    }

    public void StartHiderHidingFase()
    {
        CanvasPlayerUI.transform.Find("WaitingForStartHider").gameObject.SetActive(false);
        CanvasPlayerUI.transform.Find("HideTimerHider").gameObject.SetActive(true);
        MainTurnMapScript.canTurn = true;
        StartCoroutine(CheckIfHiddenInHidingFase());
    }

    // I don't know how, I don't know why, but this part has a particular code stink.

    // Runs a timer. If player hides during the timer, run other method. If
    // failed to hide within timer, run a method which makes the player 
    public IEnumerator CheckIfHiddenInHidingFase()
    {
        // GameObject which is a collection of UI elements for the hiding timer. Is a child of a general collection of UI elements.
        GameObject HideTimer = CanvasPlayerUI.transform.Find("HideTimerHider").gameObject;

        // Is the script component of the UI that handles changing of HideTimer UI text.
        CountdownUIScript HideTimerCountdown = HideTimer.GetComponent<CountdownUIScript>();

        for (int i = secondsToHide; i > 0; i--)
        {
            // Changes text in UI.
            HideTimerCountdown.ChangeCountdownNumber("You have ", i, " seconds to hide!");

            yield return new WaitForSecondsRealtime(1.0f);

            // Ends timer is player hides so player doesnt have to wait for the timer to run out
            if (hiderIsHidden) { i = 0; }
        }
        if (hiderIsHidden)
        {
            EndHiderHidingFaseHidden();
        }
        else
        {
            EndHiderHidingFaseTimerRanOut();
        }
    }


    // If timer went out without the ghost being hidden, lose game for Ghost. Gets called from timer
    public void EndHiderHidingFaseTimerRanOut()
    {
        Debug.Log("EndHiderHidingFaseTimeRanOut");
        EndHiderHidingFase();
        HiderLost("NotHiddenInTime");
    }

    // If ghost has hidden within before timer runs out, continue game
    public void EndHiderHidingFaseHidden()
    {
        EndHiderHidingFase();
        StartCoroutine(StartHiderWaitingFase());
    }

    public void EndHiderHidingFase()
    {
        MainTurnMapScript.canTurn = false;
        CanvasPlayerUI.transform.Find("HideTimerHider").gameObject.SetActive(false);
        // Need a continous check if hider is still in-game

    }

    public IEnumerator StartHiderWaitingFase()
    {
        SwitchResult HiderIsCaughtSwitch = new SwitchResult();
        HiderIsCaughtSwitch.Data = "Off";
        // GameObject which is a collection of UI elements for the hiding timer. Is a child of a general collection of UI elements.
        GameObject RoundTimer = CanvasPlayerUI.transform.Find("RoundTimer").gameObject;
        RoundTimer.SetActive(true);

        // Is the script component of the UI that handles changing of HideTimer UI text.
        CountdownUIScript RoundTimerCountdown = RoundTimer.GetComponent<CountdownUIScript>();

        for (int i = secondsToFind; i > 0; i--)
        {
            // Changes text in UI.
            RoundTimerCountdown.ChangeCountdownNumber("The seeker has ", i, " seconds to left");
            yield return new WaitForSecondsRealtime(1.0f);
            StartCoroutine(MainConnectionScript.CheckDomoticzSwitch(hiderIsCaughtSwitchidx, HiderIsCaughtSwitch));
            if (HiderIsCaughtSwitch.Data == "On")
            {
                hiderIsFound = true;
            }

            // Ends timer is player hides so player doesnt have to wait for the timer to run out
            if (hiderIsFound) { i = 0; }
        }
        if (hiderIsFound)
        {
            HiderLost("HiderWasCaught");
        }
        else
        {
            HiderWon("TimeRanOut");
        }
        StopHiderWaintingFase();

    }

    public void StopHiderWaintingFase()
    {
        CanvasPlayerUI.transform.Find("RoundTimer").gameObject.SetActive(false);
    }

    public IEnumerator StartSeekerWaitingFase()
    {


        // GameObject which is a collection of UI elements for the hiding timer. Is a child of a general collection of UI elements.
        GameObject HideTimer = CanvasPlayerUI.transform.Find("HideTimerSeeker").gameObject;
        HideTimer.SetActive(true);
        // Is the script component of the UI that handles changing of HideTimer UI text.
        CountdownUIScript HideTimerCountdown = HideTimer.GetComponent<CountdownUIScript>();

        for (int i = secondsToHide; i > 0; i--)
        {
            // Changes text in UI.
            HideTimerCountdown.ChangeCountdownNumber("Giving hider ", i, " seconds to hide...");

            yield return new WaitForSecondsRealtime(1.0f);
            Debug.Log(hiderIsHidden);
            if (hiderIsHidden)
            {
                i = 0;
            }
            // Ends timer is player hides so player doesnt have to wait for the timer to run out
        }
        HideTimer.SetActive(false);
        StopSeekerWaitingFase();
    }

    // Checks if hider still in game & hiding
    public void StopSeekerWaitingFase()
    {
        if (hiderIsHidden)
        {
            StartCoroutine(StartSeekerSeekingFase());
        }
        else
        {
            SeekerWon("HiderNotHidden");
        }
    }

    public IEnumerator StartSeekerSeekingFase()
    {
        // enable movement
        GameObject SeekerPlayerGameObject = GameObject.Find("Seeker");
        SeekerPlayerGameObject.GetComponent<PlayerMovementScript>().CanMove = true;

        SwitchResult HiderIsCaughtSwitch = new SwitchResult();
        HiderIsCaughtSwitch.Data = "Off";
        // GameObject which is a collection of UI elements for the hiding timer. Is a child of a general collection of UI elements.
        GameObject RoundTimer = CanvasPlayerUI.transform.Find("RoundTimer").gameObject;
        RoundTimer.SetActive(true);

        // Is the script component of the UI that handles changing of HideTimer UI text.
        CountdownUIScript RoundTimerCountdown = RoundTimer.GetComponent<CountdownUIScript>();

        for (int i = secondsToFind; i > 0; i--)
        {
            // Changes text in UI.
            RoundTimerCountdown.ChangeCountdownNumber("You have ", i, "seconds to catch the hider!");
            yield return new WaitForSecondsRealtime(1.0f);
            StartCoroutine(MainConnectionScript.CheckDomoticzSwitch(hiderIsCaughtSwitchidx, HiderIsCaughtSwitch));
            if (HiderIsCaughtSwitch.Data == "On")
            {
                hiderIsFound = true;
            }

            // Ends timer is player hides so player doesnt have to wait for the timer to run out
            if (hiderIsFound) { i = 0; }
        }
        if (hiderIsFound)
        {
            StartCoroutine(MainConnectionScript.ChangeDomoticzSwitchStatus(hiderIsCaughtSwitchidx, true));
            SeekerWon("HiderWasCaught");

        }
        else
        {
            SeekerLost("TimeRanOut");
        }
        StopSeekerSeekingFase();
    }

    public void StopSeekerSeekingFase()
    {
        CanvasPlayerUI.transform.Find("RoundTimer").gameObject.SetActive(false);
    }

    public void SeekerLost(string reasonForLosing)
    {
        // Possible reasons for losing: "TimeRanOut"
        GameObject SeekerLostGameObject = CanvasPlayerUI.transform.Find("SeekerLost").gameObject;
        SeekerLostGameObject.SetActive(true);
        SeekerLostGameObject.transform.Find(reasonForLosing).gameObject.SetActive(true);
    }

    public void SeekerWon(string reasonForWinning)
    {
        // Possible reasons for winning: "HiderNotHidden", "HiderWasCaught"
        GameObject SeekerWonGameObject = CanvasPlayerUI.transform.Find("SeekerWon").gameObject;
        SeekerWonGameObject.SetActive(true);
        SeekerWonGameObject.transform.Find(reasonForWinning).gameObject.SetActive(true);
    }

    public void HiderLost(string reasonForLosing)
    {

        MainConnectionScript.ChangeDomoticzSwitchStatus(hiderInGameSwitchidx, false);
        // Possible reasons for losing: "NotHiddenInTime", "HiderWasCaught"
        GameObject HiderLostGameObject = CanvasPlayerUI.transform.Find("HiderLost").gameObject;
        HiderLostGameObject.SetActive(true);
        HiderLostGameObject.transform.Find(reasonForLosing).gameObject.SetActive(true);
    }

    public void HiderWon(string reasonForWinning)
    {
        // Possible reasons for winning: "TimeRanOut"
        GameObject HiderWonGameObject = CanvasPlayerUI.transform.Find("HiderWon").gameObject;
        HiderWonGameObject.SetActive(true);
        HiderWonGameObject.transform.Find(reasonForWinning).gameObject.SetActive(true);
    }
}
