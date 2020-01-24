
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{

    public List<Dialogue> dialogues;

    public void TriggerDialogue()
    {
        int index = Random.Range(0, dialogues.Count);
        GetComponent<Image>().sprite = dialogues[index].sprite;
        FindObjectOfType<DialogueManager>().StartDialogue(dialogues[index]);
    }

}