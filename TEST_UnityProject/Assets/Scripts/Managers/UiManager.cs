using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance { get; private set; }

        public Button normalPuckBtn;
        public Button specialPuckBtn;
        public TextMeshProUGUI discLeftTxt;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            normalPuckBtn.onClick.AddListener(() =>AddPuck(PuckType.Normal));
            specialPuckBtn.onClick.AddListener(() =>AddPuck(PuckType.Special));
        }

        private void Start()
        {
            discLeftTxt.text = String.Format("{0} DISCS LEFT", PlayerManager.Instance.discLeft.ToString());
        }

        private void AddPuck(PuckType type)
        {
            PlayerManager.Instance.discLeft--;
            discLeftTxt.text = String.Format("{0} DISCS LEFT", PlayerManager.Instance.discLeft.ToString());
            if (PlayerManager.Instance.discLeft == 0)
            {
                normalPuckBtn.interactable = false;
                specialPuckBtn.interactable = false;
            }
        
            LevelManager.Instance.PlacePuck(type);
            if (type == PuckType.Special && !PlayerManager.Instance.hasUsedSpecialDisk)
            {
                specialPuckBtn.interactable = false;
                PlayerManager.Instance.hasUsedSpecialDisk = true;
            }
        }

        public void RestUiData()
        {
            normalPuckBtn.interactable = true;
            specialPuckBtn.interactable = true;
            discLeftTxt.text = String.Format("{0} DISCS LEFT", PlayerManager.Instance.discLeft.ToString());

        }
    }

    public enum PuckType{
        Normal,
        Special,
    }
}