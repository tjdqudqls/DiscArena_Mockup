using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class PopupController : MonoBehaviour
    {
        public TextMeshProUGUI desc;
        public Button btn;
        public TextMeshProUGUI btnTxt;

        public void Init(string description, string btnText, Action action)
        {
            this.desc.text = description;
            this.btnTxt.text = btnText;
            btn .onClick.AddListener(() =>
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
}
