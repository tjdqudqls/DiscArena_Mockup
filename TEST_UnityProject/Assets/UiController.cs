using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Button NormalPuckBtn;
    public Button SpecialPuckBtn;

    private void Awake()
    {
        NormalPuckBtn.onClick.AddListener(() =>AddPuck(PuckType.NORMAL));
        SpecialPuckBtn.onClick.AddListener(() =>AddPuck(PuckType.SPECIAL));

    }


    private void AddPuck(PuckType type)
    {
        PlayerManager.Instance.DiscLeft--;
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