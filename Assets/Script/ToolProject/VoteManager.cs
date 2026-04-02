using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteManager : MonoBehaviour
{
    public VoteModule module;

    public Transform target;

    public Button StartButton;

    public Button StopButton;

    public VoteResultPanel resultPanel;

    private int index = 1;

    [SerializeField]
    private List<VoteModule> moduleList = new List<VoteModule>();

    private List<int> valueList = new List<int>();

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            moduleList.Add(Instantiate(module, target));
            valueList.Add(0);

            if (i > 0)
            {
                moduleList[i].gameObject.SetActive(false);
            }
        }
    }

    public void AddModule()
    {
        for (int i = 0; i < 5; i++)
        {
            if(!moduleList[i].gameObject.activeSelf)
            {
                moduleList[i].gameObject.SetActive(true);
                index = (i + 1);
                return;
            }
        }
    }

    public void PopModule()
    {
        for (int i = 4; i >= 0; i--)
        {
            if (moduleList[i].gameObject.activeSelf)
            {
                moduleList[i].gameObject.SetActive(false);
                index--;
                return;
            }
        }
    }

    public void StartVote()
    {
        ResetData();
        foreach (var item in moduleList)
        {
            item.ChangeMode(false);
        }
        WebSocketManager.Instance.ConnectWebSocket();
        ChangeMode(true);
    }

    public void StopVote()
    {
        foreach (var item in moduleList)
        {
            item.ChangeMode(true);
        }
        WebSocketManager.Instance.DisConnectWebSocket();
        //
        CheckResult();
    }

    public void CheckResult()
    {
        int tempIndex = 0;
        for (int i = 0; i < 4; i++)
        {
            if(valueList[i] < valueList[i+1])
            {
                tempIndex = i + 1;
            }
        }
        resultPanel.SetValue(moduleList[tempIndex].inputText.text ,valueList[tempIndex].ToString());
    }

    private void ChangeMode(bool IsStart)
    {
        StartButton.gameObject.SetActive(!IsStart);
        StopButton.gameObject.SetActive(IsStart);
    }

    public void ResetVote()
    {
        ChangeMode(false);
        ResetData();
    }

    void Start()
    {
        WebSocketManager.Instance.Subscribe(ProcessVote);
    }

    private void ResetData()
    {
        for (int i = 0; i < 5; i++)
        {
            moduleList[i].SetValue(0, 0);
            valueList[i] = 0;
        }
    }
   

    void ProcessVote(BroadcastEvent e)
    {
        if (int.TryParse(e.message, out int result))
        {
            if(result <= index)
            {
                valueList[(result - 1)]++;
                UpdateUI();
            }
        }
        else
        {

        }

    }
    
   void UpdateUI()
   {
        int total = 0;

        for (int i = 0; i < index; i++)
        {
            total += valueList[i];
        }

       if (total == 0)
           return;

        for (int i = 0; i < index; i++)
        {
            moduleList[i].SetValue(((float)valueList[i] / total), valueList[i]);
        }
   }
}
