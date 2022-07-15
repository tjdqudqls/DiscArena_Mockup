using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    public Button NormalPuckBtn;
    public Button SpecialPuckBtn;
    public TextMeshProUGUI DiscLeftTxt;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        NormalPuckBtn.onClick.AddListener(() =>AddPuck(PuckType.NORMAL));
        SpecialPuckBtn.onClick.AddListener(() =>AddPuck(PuckType.SPECIAL));
        DiscLeftTxt.text = String.Format("{0} DISCS LEFT", PlayerManager.Instance.DiscLeft.ToString());
    }


    private void AddPuck(PuckType type)
    {
        PlayerManager.Instance.DiscLeft--;
        DiscLeftTxt.text = String.Format("{0} DISCS LEFT", PlayerManager.Instance.DiscLeft.ToString());
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

    public void RestUiData()
    {
        NormalPuckBtn.interactable = true;
        SpecialPuckBtn.interactable = true;
        DiscLeftTxt.text = String.Format("{0} DISCS LEFT", PlayerManager.Instance.DiscLeft.ToString());

    }
}

public enum PuckType{
    NORMAL,
    SPECIAL,
}