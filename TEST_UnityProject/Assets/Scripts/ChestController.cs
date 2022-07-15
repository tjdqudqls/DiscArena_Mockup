using System;
using DefaultNamespace;
using UnityEngine;

public class ChestController : MonoBehaviour, IEnemy
{
    public HealthBarController HealthBarController;
    public float MaxHp = 2;
    public void Awake()
    {
        HealthBarController.SetMaxHealth(MaxHp);
    }

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out PuckController puck))
        {
            OnDamage(puck.Damage);
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
        LevelManager.Instance.ClearedLevel();
    }
}
