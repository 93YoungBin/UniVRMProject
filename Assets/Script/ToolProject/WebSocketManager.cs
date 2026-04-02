using NativeWebSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSocketManager : Singleton<WebSocketManager>
{
    public WebSocket webSocket;

    private event Action<BroadcastEvent> OnChatEvent;
    private event Action<BroadcastEvent> OnDonationEvent;

    void Start()
    {
        webSocket = new WebSocket("ws://localhost:3000");

        webSocket.OnOpen += () =>
        {
            Debug.Log("Connected");
        };

        webSocket.OnMessage += (bytes) =>
        {
            string msg = System.Text.Encoding.UTF8.GetString(bytes);

            BroadcastEvent e = JsonUtility.FromJson<BroadcastEvent>(msg);

            Dispatch(e);
        };
    }

    public void ConnectWebSocket()
    {
        Debug.Log("WebSocketConnect");
        webSocket.Connect();
    }

    public void DisConnectWebSocket()
    {
        Debug.Log("DisConnectWebSocket");
        webSocket.Close();
    }

    void HandleMessage(string msg)
    {
        // ¿¹: JSON ÆÄ½Ì
        // { "type": "vote", "option": "A" }
    }

    private async void OnApplicationQuit()
    {
        await webSocket.Close();
    }

    private void Dispatch(BroadcastEvent e)
    {
        switch (e.type)
        {
            case "chat":
                OnChatEvent?.Invoke(e);
                break;

            case "donation":
                OnDonationEvent?.Invoke(e);
                break;
        }
    }

    public void Subscribe(Action<BroadcastEvent> callback)
    {
        OnChatEvent -= callback;
        OnChatEvent += callback;
    }

    public void Unsubscribe(Action<BroadcastEvent> callback)
    {
        OnChatEvent -= callback;
    }

    public void Clear() => OnChatEvent = null;


}

[System.Serializable]
public class BroadcastEvent
{
    public string type;
    public string user;
    public string message;
    public int amount;
}