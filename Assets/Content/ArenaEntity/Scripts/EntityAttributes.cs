using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity
{
    [System.Serializable]
    public class EntityAttributes
    {
        [SerializeField] protected int health = 100;
        public int Health
        {
            get => health;
            set
            {
                health = Mathf.Max( value, 0 );
            }
        }

        [SerializeField] protected int mana = 100;
        public int Mana
        {
            get => mana;
            set
            {
                mana = Mathf.Max( value, 0 );
            }
        }

        [SerializeField] protected int manaRegen = 5;
        public int ManaRegen => manaRegen;

        [SerializeField] protected int cooldownReduction = 0;
        public int CooldownReduction => cooldownReduction;

        [Range( 0, 1 )]
        [SerializeField] protected float airResistance = 0;
        public float AirResistance => airResistance;

        [Range( 0, 1 )]
        [SerializeField] protected float fireResistance = 0;
        public float FireResistance => fireResistance;

        [Range( 0, 1 )]
        [SerializeField] protected float earthResistance = 0;
        public float EarthResistance => earthResistance;

        public EntityAttributes( EntityAttributes source )
        {
            this.health = source.health;
            this.mana = source.mana;
            this.airResistance = source.airResistance;
            this.fireResistance = source.fireResistance;
            this.earthResistance = source.earthResistance;
        }
    }
}
