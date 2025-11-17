using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalTimer : MonoBehaviour
{
    private string gameStateName = "GameState";
    private GameObject gameState;
    private GameStateScript gameStateScript;
    private TextMeshProUGUI thisText;


    private void Awake()
    {
        gameState = GameObject.Find(gameStateName);
        gameStateScript = gameState.GetComponent<GameStateScript>();

        //moi
        thisText = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameStateScript.DisplayTotalTimer(thisText);
    }
    
}
