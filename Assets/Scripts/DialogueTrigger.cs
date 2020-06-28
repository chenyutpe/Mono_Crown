using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject DialogueBox;
    public Dialogue dialogue;

    void Start () // temporary method
    {
        StartCoroutine(AutoStart());
    }

    public void TriggerDialogue ()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    IEnumerator AutoStart () // temporary method
    {
        DialogueBox.SetActive(true);
        yield return new WaitForSeconds(1);
        TriggerDialogue();
    }
}