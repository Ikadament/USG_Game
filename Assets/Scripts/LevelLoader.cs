using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private Animator loaderAnimator;
    private GameObject imageTransition;

    public float transitionTime = 1f;

    //Gamestate finder
    public GameObject gameStatePrefab;
    private string gameStateName = "GameState";
    public bool gameStateIsCreated = false;
    private GameStateScript gameStateScript;    

    private void Start()
    {
        imageTransition = GameObject.Find("Image");
        loaderAnimator = imageTransition.GetComponent<Animator>();
        GameObject gameState = GameObject.Find(gameStateName);
        gameStateScript = gameState.GetComponent<GameStateScript>();
    }


    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        loaderAnimator.SetTrigger("StartLoad");

        GameStateScript.instance.levelLoading = true;
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
        GameStateScript.instance.levelLoading = false;

    }
}
