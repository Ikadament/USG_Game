using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    private GameObject player;
    private PlayerControls playerControls;

    //public Image imageMask;
    private GameObject levelLoaderParent;

    private LevelLoader levelLoaderScript;

    private string gameStateName = "GameState";
    private GameObject gameState;
    private GameStateScript gameStateScript;

    private void Start()
    {
        //objectMask = GameObject.Find("Mask");
        levelLoaderParent = GameObject.Find("LevelLoader");
        player = GameObject.Find("Player");
        playerControls = player.GetComponent<PlayerControls>();

        levelLoaderScript = levelLoaderParent.GetComponent<LevelLoader>();

        gameState = GameObject.Find(gameStateName);
        gameStateScript = gameState.GetComponent<GameStateScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (GameObject.Find("GoldPlaceHolder") == null)
            {
                gameStateScript.nbOfGold += 1;
            }

            levelLoaderScript.LoadNextLevel();
            playerControls.rb.constraints = RigidbodyConstraints2D.FreezeAll;
            FindObjectOfType<AudioManager>().Play("flag");
        }
    }
}
