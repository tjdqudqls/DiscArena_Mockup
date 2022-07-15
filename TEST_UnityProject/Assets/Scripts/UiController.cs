using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Button NormalPuckBtn;
    public Button SpecialPuckBtn;
    public TextMeshProUGUI DiscLeftTxt;
    private void Awake()
    {
        NormalPuckBtn.onClick.AddListener(() =>AddPuck(PuckType.NORMAL));
        SpecialPuckBtn.onClick.AddListener(() =>AddPuck(PuckType.SPECIAL));
        DiscLeftTxt.text = String.Format("{0} DISCS LEFT", PlayerManager.Instance.DiscLeft);
    }


    private void AddPuck(PuckType type)
    {
        PlayerManager.Instance.DiscLeft--;
        DiscLeftTxt.text = String.Format("{0} DISCS LEFT", PlayerManager.Instance.DiscLeft);
        if (PlayerManager.Instance.DiscLeft == 0)
        {
            NormalPuckBtn.interactable = false;
            SpecialPuckBtn.interactable = false;
        }
        
        LevelManager.Instance.PlacePuck(type);
        if (type == PuckType.SPECIAL && !PlayerManager.Instance.hasUsedSpecialDisk)
        {
            SpecialPuckBtn.interactable = false;
            PlayerManager.Instance.hasUsedSpecialDisk = true;
        }
    }

    private void LateUpdate()
    {
        //check if Special puck is used. and disable
    }
}

public enum PuckType{
    NORMAL,
    SPECIAL,
}