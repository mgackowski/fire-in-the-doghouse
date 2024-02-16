using System;
using TMPro;
using UnityEngine;

public class MessageBox : MonoBehaviour
{

    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }


    private void UpdateMessageBox(Message msg)
    {
        if (msg.type == MessageType.SYSTEM)
        {
            text.text += "\n" + msg.text;
        }
    }

    private void OnEnable()
    {
        MessageSystem.messageArrived += UpdateMessageBox;
    }
}
