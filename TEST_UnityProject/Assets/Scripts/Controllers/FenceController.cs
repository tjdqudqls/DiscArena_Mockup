using Interfaces;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class FenceController : MonoBehaviour, IEnemy
    {
        public HealthBarController HealthBarController => healthBarController;

        public ParticleSystem Fire => fire;

        public float MaxHp => maxHp;
        public HealthBarController healthBarController;
        public ParticleSystem fire;
        public float maxHp;
        public void Awake()
        {
            HealthBarController.SetMaxHealth(MaxHp);
            Fire.Stop();
        }
    
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PuckController puck)&& !puck.isGhost)
            {
                OnDamage(puck.damage);

                if (HealthBarController.CurrentHpPercentage <= 0.5f)
                {
                    Fire.Play();
                }
                if (HealthBarController.currentHp <= 0)
                {
                    OnDeath();
                }
            }
        }

        public void OnDamage(float dmg)
        {
            Debug.Log("DAMGE");
            CameraController.Instance.OnDamage();
            HealthBarController.OnDamage(dmg);
        }

        public void OnDeath()
        {
            PhysicsSceneManager.Instance.DestroyEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}
