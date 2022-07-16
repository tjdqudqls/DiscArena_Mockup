using UnityEngine;


public interface IEnemy 
{
    HealthBarController HealthBarController { get; }
    ParticleSystem Fire { get; }
    float MaxHp { get; }
    void OnDamage(float dmg);
    void OnDeath();
}
