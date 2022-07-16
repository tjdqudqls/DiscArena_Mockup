using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }
        public int discLeft = 5;

        public bool hasUsedSpecialDisk;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }


        /// <summary>
        /// Reset Player state.
        /// </summary>
        public void ResetPlayerState()
        {
            discLeft = 5;
            hasUsedSpecialDisk = false;
            UiManager.Instance.RestUiData();
        }
    }
}

