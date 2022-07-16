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

        /// <summary>
        /// Populate PopupData
        /// </summary>
        /// <param name="description"></param>
        /// <param name="btnText"></param>
        /// <param name="action"></param>
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

        /// <summary>
        /// Instantiate PopupUI.
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="btnTxt"></param>
        /// <param name="action"></param>
        public static void CreatePopup(string desc, string btnTxt, Action action)
        {
            var popup = Instantiate(Resources.Load("PopupCanvas") as GameObject);
            popup.GetComponent<PopupController>().Init(desc, btnTxt, action);
        }
    }
}
