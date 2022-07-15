using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    public TextMeshProUGUI Desc;
    public Button Btn;
    public TextMeshProUGUI BtnTxt;

    public void Init(string desc, string btnTxt, Action action)
    {
        Desc.text = desc;
        BtnTxt.text = btnTxt;
        Btn .onClick.AddListener(() =>
        {
            action();
            Destroy(gameObject);
        });
    }

    public static void CreatePopup(string desc, string btnTxt, Action action)
    {
        var popup = Instantiate(Resources.Load("PopupCanvas") as GameObject);
        popup.GetComponent<PopupController>().Init(desc, btnTxt, action);
    }
}
