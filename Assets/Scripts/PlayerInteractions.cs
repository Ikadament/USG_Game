using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    bool inConversation = false;
    bool playerInZone;

    public GameObject npcCube;
    private NpcScript npcScript;

    //PlayerControls ref
    private PlayerControls playerControls;

    //Camera ref
    public Camera cam;
    private CameraZoom cameraZoom;

    //E button ref
    public GameObject eButton;
    private Vector2 eButtonOffset = new Vector2(0, 1f);

    private void Start()
    {
        npcScript = npcCube.GetComponent<NpcScript>();
        playerControls = this.GetComponent<PlayerControls>();
        cameraZoom = cam.GetComponent<CameraZoom>();
        eButton = GameObject.Find("E button");

        eButton.SetActive(false);
        eButtonToNpcPos();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        if (inConversation)
        {
            DialogueBoxController.instance.SkipLine();

            FindObjectOfType<AudioManager>().Play("pngTalk2");
        }
        else
        {
            if (playerInZone)
            {
                npcScript.PlayerInteracted();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Npc"))
        {
            playerInZone = true;
            eButton.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Npc"))
        {
            playerInZone = false;

            if(eButton != null)
            {
                eButton.SetActive(false);
            }
            
        }
    }

    void eButtonToNpcPos()
    {
        eButton.transform.position = (Vector2)npcCube.transform.position + eButtonOffset;
    }

    void JoinConversation()
    {
        inConversation = true;
        playerControls.FreezePlayerMovement();
        cameraZoom.MakeCameraZoom();
    }

    void LeaveConversation()
    {
        inConversation = false;
        playerControls.DefrostPlayerMovement();
        cameraZoom.MakeCameraDezoom();
    }

    private void OnEnable()
    {
        DialogueBoxController.OnDialogueStarted += JoinConversation;
        DialogueBoxController.OnDialogueEnded += LeaveConversation;
    }

    private void OnDisable()
    {
        DialogueBoxController.OnDialogueStarted -= JoinConversation;
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }

    
}
