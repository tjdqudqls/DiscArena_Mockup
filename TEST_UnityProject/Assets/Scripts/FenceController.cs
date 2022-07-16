using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class FenceController : MonoBehaviour, IEnemy
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
