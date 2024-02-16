using System.Collections;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public MessageType character;
    public float timeOnScreen = 3f;

    TextMeshProUGUI text;
    string currentString;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }


    private void UpdateMessageBox(Message msg)
    {
        if (msg.type == character)
        {
            currentString = msg.text;
            StartCoroutine(AnimateText());
        }
    }

    IEnumerator AnimateText()
    {
        foreach (char c in currentString)
        {
            text.text += c;
            yield return null;
        }
        yield return new WaitForSeconds(timeOnScreen);
        text.text = "";
    }

    private void OnEnable()
    {
        MessageSystem.messageArrived += UpdateMessageBox;
    }

    private void OnDisable()
    {
        MessageSystem.messageArrived -= UpdateMessageBox;
    }
}
