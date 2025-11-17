using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class GameStateScript : MonoBehaviour
{
    public static GameStateScript instance;
    public bool isOrange;
    public bool isCreated;

    //Timer

    public TextMeshProUGUI timerText; // assigne dans l'inspecteur
    public Animator timerAnimator;

    public float timer = 0f;
    public float totalTimer;
    private bool isRunning = true;
    private bool hasDisplayed = false;

    public bool levelLoading;

    public float waitStartDisplayTime;
    public float waitDisplayTime;

    public int nbOfGold = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
        }

        // Des que la condition est remplie une fois, on affiche le timer
        if (levelLoading && !hasDisplayed)
        {
            isRunning = false;
            StartCoroutine(DisplayTimer());
        }
    }
    IEnumerator DisplayTimer()
    {
        yield return new WaitForSeconds(waitStartDisplayTime);

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);
        int milliseconds = Mathf.FloorToInt((timer * 100F) % 100F);

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
        timerText.gameObject.SetActive(true); // Affiche le texte
        hasDisplayed = true;

        timerAnimator.SetBool("timerDisplayOn", true);
        yield return new WaitForSeconds(waitDisplayTime);

        HideTimer();
    }

    public void DisplayTotalTimer(TextMeshProUGUI totalTimerText)
    {
        int minutes = Mathf.FloorToInt(totalTimer / 60F);
        int seconds = Mathf.FloorToInt(totalTimer % 60F);
        int milliseconds = Mathf.FloorToInt((totalTimer * 100F) % 100F);

        totalTimerText.text = $"Total Time {minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    void HideTimer()
    {
        totalTimer += timer;
        timer = 0f;
        timerText.gameObject.SetActive(false);

        isRunning = true;
        hasDisplayed = false;
    }
}
