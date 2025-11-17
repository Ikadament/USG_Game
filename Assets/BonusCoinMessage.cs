using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusCoinMessage : MonoBehaviour
{
    private string gameStateName = "GameState";
    private GameObject gameState;
    private GameStateScript gameStateScript;

    public TextMeshProUGUI bonusMessage;
    public bool hasShownBonus = false;

    private void Start()
    {
        gameState = GameObject.Find(gameStateName);
        gameStateScript = gameState.GetComponent<GameStateScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasShownBonus && gameStateScript.nbOfGold == 10 && GameObject.Find("FinalGoldPlaceHolder") == null)
        {
            bonusMessage.gameObject.SetActive(true);
            hasShownBonus = true;
        }
    }
}
