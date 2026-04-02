using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoteModule : MonoBehaviour
{
    public Slider slider;

    public TMP_InputField inputField;

    public TMP_Text inputText;

    public TMP_Text voteText;

    public Button xButton;

    private void Awake()
    {
        inputText.gameObject.SetActive(false);
        voteText.gameObject.SetActive(false);
    }

    public void ChangeMode(bool isEdit)
    {
        inputText.gameObject.SetActive(!isEdit);
        voteText.gameObject.SetActive(!isEdit);
        xButton.gameObject.SetActive(isEdit);
        inputField.gameObject.SetActive(isEdit);
    }

    public void SetValue(float sliderValue, int voteValue)
    {
        slider.value = sliderValue;
        voteText.text = voteValue.ToString();
    }
}
