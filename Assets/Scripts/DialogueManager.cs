using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private TMPro.TextMeshProUGUI sentenceText;

    private Queue<string> sentences;

    void Start ()
    {
        sentences = new Queue<string>();
        nameText.text = "";     // Clear at first
        sentenceText.text = ""; // Clear at first
    }

    public void StartDialogue (Dialogue dialogue)
    {
        nameText.text = dialogue.name;

        sentences.Clear();
        foreach (string s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }

        NextSentence();
    }

    public void NextSentence ()
    {
        if (sentences.Count == 0)    // the end of sentences
        {
            return; // maybe add something later
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();    // in case last sentence is unfinished
        StartCoroutine(TypeAnimation(sentence));
    }

    IEnumerator TypeAnimation (string sentence)
    {
        sentenceText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            sentenceText.text += letter;
            yield return null;
        }
    }
}