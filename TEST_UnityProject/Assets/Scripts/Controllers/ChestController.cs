using Interfaces;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class ChestController : MonoBehaviour, IEnemy
    {
        public HealthBarController HealthBarController => healthBarController;

        public ParticleSystem Fire => null;

        public float MaxHp => maxHp;
        public HealthBarController healthBarController;
        public float maxHp = 2;
        public void Awake()
        {
            HealthBarController.SetMaxHealth(MaxHp);
        }

        // Start is called before the first frame update
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PuckController puck) && !puck.isGhost)
            {
                OnDamage(puck.damage);
                if (HealthBarController.currentHp <= 0)
                {
                    OnDeath();
                }
            }
        }


        public void OnDamage(float dmg)
        {
            CameraController.Instance.OnDamage();
            HealthBarController.OnDamage(dmg);
        }

        public void OnDeath()
        {
            PhysicsSceneManager.Instance.DestroyEnemy(gameObject);
            LevelManager.Instance.ClearedLevel();
        }
    }
}
