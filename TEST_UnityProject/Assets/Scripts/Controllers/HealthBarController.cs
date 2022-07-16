using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class HealthBarController : MonoBehaviour
    {
        private float _maxHp = 100f;

        public float currentHp;

        
        public float CurrentHpPercentage
        {
            get
            {
                return currentHp / _maxHp;
            }
        }
        [SerializeField]
        private Image healthBar;
        // Start is called before the first frame update

        /// <summary>
        /// Set max health value of HealthBar.
        /// </summary>
        /// <param name="val"></param>
        public void SetMaxHealth(float val)
        {
            _maxHp = val;
            currentHp = val;
        }

        
        /// <summary>
        /// Subtracts value to current health and apply to UI.
        /// </summary>
        /// <param name="val"></param>
        public void OnDamage(float val)
        {
            currentHp -= val;
            currentHp = currentHp <= 0 ? 0 : currentHp;
            healthBar.fillAmount = currentHp / _maxHp;
        }

        
        /// <summary>
        /// Adds value to current health and apply to UI.
        /// </summary>
        /// <param name="val"></param>
        public void OnHeal(float val)
        {
            currentHp += val;
            currentHp = currentHp > _maxHp ? _maxHp : currentHp;
            healthBar.fillAmount = currentHp / _maxHp;
        }
    }
}
