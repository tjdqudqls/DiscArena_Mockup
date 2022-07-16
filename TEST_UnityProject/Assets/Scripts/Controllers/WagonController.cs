using Interfaces;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class WagonController : MonoBehaviour, IEnemy
    {
        public HealthBarController HealthBarController => healthBarController;

        public ParticleSystem Fire => fire;

        public float MaxHp => maxHp;
        public HealthBarController healthBarController;
        public ParticleSystem fire;
        public float maxHp;
        private Vector3 _patrolDirection = Vector3.forward;
        private void Update()
        {
            transform.Translate(_patrolDirection * (4 * Time.deltaTime));
        }

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
            else
            {
                _patrolDirection *= -1;
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
            Destroy(gameObject);
        }
    }
}