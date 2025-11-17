using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class DialogueBoxController : MonoBehaviour
{
    public static DialogueBoxController instance;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] RectTransform dialoguePanel;
    [SerializeField] RectTransform canvasRectT;

    public Vector2 dialoguePanelOffset = new Vector2(0, 1.5f);

    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;
    bool skipLineTriggered;

    //PlayerControls ref
    public GameObject player;
    private PlayerControls playerControls;

    private void Start()
    {
        playerControls = player.GetComponent<PlayerControls>();
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void DialoguePanelToPlayerPos(Vector2 npcPos)
    {
        //Erreur : J'ai fait comme si la dialogue Box faisait partie de l'UI fixe qui change en fonction de la taille de l'ecran

        //Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, npcPos);
        //screenPosition.y += dialoguePanelOffset; (dialoguePanelOffset etait un float de 90f)
        //dialoguePanel.anchoredPosition = screenPosition - canvasRectT.sizeDelta / 2f;

        dialoguePanel.position = npcPos + dialoguePanelOffset;
    }

    public void StartDialogue(string[] dialogue, int startPosition)
    {
        dialoguePanel.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogue,startPosition));
    }

    IEnumerator RunDialogue(string[] dialogue, int startPosition)
    {
        skipLineTriggered = false;
        OnDialogueStarted?.Invoke();

        for (int i = startPosition; i < dialogue.Length; i++)
        {
            dialogueText.text = dialogue[i];
            while (skipLineTriggered == false)
            {
                yield return null;
            }
            skipLineTriggered = false;
        }

        OnDialogueEnded?.Invoke();
        dialoguePanel.gameObject.SetActive (false);
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }
}
