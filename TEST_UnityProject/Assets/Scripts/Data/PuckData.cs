using UnityEngine;

namespace Data
{
    public class PuckData
    {
        public readonly float Damage;
        public readonly float Speed;
        public readonly Material Look;

        public PuckData(float dmg, float speed, Material look)
        {
            Damage = dmg;
            Speed = speed;
            Look = look;
        }
    }
}