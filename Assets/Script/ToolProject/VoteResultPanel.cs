using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoteResultPanel : MonoBehaviour
{
    public TMP_Text ResultText;

    public TMP_Text ResultIndex;

    public Button ResetButton;

    public void SetValue(string resultText, string resultIndex)
    {
        ResultText.text = resultText;
        ResultIndex.text = resultIndex;
        gameObject.SetActive(true);
    }

}
