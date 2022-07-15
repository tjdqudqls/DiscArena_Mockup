using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private float MAX_HP = 100f;

    public float Current_Hp;
    [SerializeField]
    private Image healthBar;
    // Start is called before the first frame update

    public void SetMaxHealth(float val)
    {
        MAX_HP = val;
        Current_Hp = val;
    }

    public void OnDamage(float val)
    {
        Current_Hp -= val;
        Current_Hp = Current_Hp <= 0 ? 0 : Current_Hp;
        healthBar.fillAmount = Current_Hp / MAX_HP;
    }

    public void OnHeal(float val)
    {
        Current_Hp += val;
        Current_Hp = Current_Hp > MAX_HP ? MAX_HP : Current_Hp;
        healthBar.fillAmount = Current_Hp / MAX_HP;
    }
    public void ChangeHealthValue(float val)
    {
        
        
    }
}
