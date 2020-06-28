using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ContactSender : MonoBehaviour {
    [SerializeField] TMPro.TMP_InputField titleF;
    [SerializeField] TMPro.TextMeshProUGUI title;
    [SerializeField] TMPro.TMP_InputField contentF;
    [SerializeField] TMPro.TextMeshProUGUI content;
    [SerializeField] Button SendButton;

    const string urlGFormBase = "https://docs.google.com/forms/d/e/1FAIpQLSf6XdKyrxxpRu9h_fTHwPq9mpNK2Dva1zLRANrwzE2tn6917w/";
    const string EntryID_title = "entry.1578565729";
    const string EntryID_content = "entry.183305935";
    
    void Start ()
    {
        SendButton.onClick.AddListener(Send);
    }

    void Send ()
    {
        StartCoroutine(SendGFormData());
    }

    void ClearWords ()
    {
        titleF.text = "";
        contentF.text = "";
    }

    IEnumerator SendGFormData ()
    {
        WWWForm form = new WWWForm();
        form.AddField(EntryID_title, title.text);
        form.AddField(EntryID_content, content.text);
        string urlGFormResponse = urlGFormBase + "formResponse";
        using ( UnityWebRequest www = UnityWebRequest.Post(urlGFormResponse, form) ) {
            yield return www.SendWebRequest();
        }
    }
}