using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class WagonController : MonoBehaviour, IEnemy
    {
        public HealthBarController HealthBarController;
        public ParticleSystem Fire;
        public float MaxHp;
        private Vector3 patrolDirection = Vector3.forward;
        private void Update()
        {
            transform.Translate(patrolDirection* 4 * Time.deltaTime);
        }

        public void Awake()
        {
            HealthBarController.SetMaxHealth(MaxHp);
            Fire.Stop();
        }
    
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PuckController puck))
            {
                OnDamage(puck.Damage);
                if (HealthBarController.CurrentHpPercentage <= 0.5f)
                {
                    Fire.Play();
                }
                if (HealthBarController.Current_Hp <= 0)
                {
                    OnDeath();
                }
            }
            else
            {
                patrolDirection *= -1;
            }
        }

        public void OnDamage(float dmg)
        {
            CameraController.Instance.OnDamage();
            HealthBarController.OnDamage(dmg);
        }

        public void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}