using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class WagonController : MonoBehaviour, IEnemy
    {
        public HealthBarController HealthBarController
        {
            get => healthBarController;
        }
        public ParticleSystem Fire
        {
            get => fire;
        }
        public float MaxHp
        {
            get => maxHp;
        }
        public HealthBarController healthBarController;
        public ParticleSystem fire;
        public float maxHp;
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
            if (other.gameObject.TryGetComponent(out PuckController puck)&& !puck.isGhost)
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
            PhysicsSceneManager.Instance.DestroyEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}