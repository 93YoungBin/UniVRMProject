using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class ChatDisplay : MonoBehaviour
{
    public static ChatDisplay Instance;

    public TextMeshProUGUI chatText;

    public int maxMessages = 5;

    private Queue<string> messages = new Queue<string>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        WebSocketManager.Instance.Subscribe(ShowChat);
    }

    void ShowChat(BroadcastEvent e)
    {
        Color userColor = Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.8f, 1f);
        string colorHex = ColorUtility.ToHtmlStringRGB(userColor);

        string line = $"<color=#{colorHex}>{e.user}</color> : {e.message}";

        messages.Enqueue(line);

        if (messages.Count > maxMessages)
            messages.Dequeue();

        RefreshChat();
    }

    void RefreshChat()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var msg in messages)
        {
            sb.AppendLine(msg);
        }

        chatText.text = sb.ToString();
    }
}