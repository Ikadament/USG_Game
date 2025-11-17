using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour
{
    [SerializeField] public bool firstInteraction = true;
    [SerializeField] int repeatStartPosition;

    public DialogueAsset firstDialogueAsset;
    public DialogueAsset repeatDialogueAsset;


    //EN GROS, le getter s'active a chaque fois que la variable est utilisee ou mentionnee, si c'est la premiere fois alors firstInteraction est mis false
    //et il retourne 0, ce qui fait que dans la boucle de dialogue ca va du dialogue 0 a x, et la prochaine fois alors il va retourner repeatStartPosition qui est mis a 0 dans l'inspecteur,
    //ce qui fait que le dialogue recommence toujours a l'indice 0 et simule tout les dialogues puis se remet a 0 et ainsi de suite.

    [HideInInspector]
    public int StartPosition
    {
        get
        {
            if (firstInteraction)
            {
                firstInteraction = false;
                return 0;
            }
            else
            {
                return repeatStartPosition;
            }
        }
    }

    public void PlayerInteracted()
    {
        DialogueBoxController.instance.DialoguePanelToPlayerPos(transform.position);

        if(firstInteraction)
        {
            DialogueBoxController.instance.StartDialogue(firstDialogueAsset.dialogue, StartPosition);

            FindObjectOfType<AudioManager>().Play("pngTalk1");
        } 
        else
        {
            DialogueBoxController.instance.StartDialogue(repeatDialogueAsset.dialogue, StartPosition);

            FindObjectOfType<AudioManager>().Play("pngTalk1");
        }
    }
}
