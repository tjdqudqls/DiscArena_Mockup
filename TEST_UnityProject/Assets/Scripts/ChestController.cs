using System;
using UnityEngine;

public class ChestController : MonoBehaviour, IEnemy
{

    public HealthBarController HealthBarController;
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
        HealthBarController.OnDamage(dmg);
    }

    public void OnDeath()
    {
        Debug.Log("DEATH"); 
    }
}
