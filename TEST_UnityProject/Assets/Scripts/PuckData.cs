using UnityEngine;

namespace DefaultNamespace
{
    public class PuckData
    {
        public float Damage;
        public float Speed;
        public Material Look;

        public PuckData(float dmg, float speed, Material look)
        {
            this.Damage = dmg;
            this.Speed = speed;
            this.Look = look;
        }
    }
}