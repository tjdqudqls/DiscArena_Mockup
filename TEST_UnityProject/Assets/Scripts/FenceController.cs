using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceController : MonoBehaviour, IEnemy
{
    public HealthBarController HealthBarController;
    public ParticleSystem Fire;
    public float MaxHp;
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
    }

    public void OnDamage(float dmg)
    {
        CameraController.Instance.OnDamage();
        HealthBarController.OnDamage(dmg);
    }

    public void OnDeath()
    {
        Debug.Log("DEATH");
        Destroy(gameObject);
    }
}
