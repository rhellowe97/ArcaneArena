using ArcaneArena.Data;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ArcaneArena.Entity
{
    public class ArenaEntity : MonoBehaviour, IHittable
    {
        [SerializeField] protected EntityAttributeData baseAttributeData;
        public EntityAttributeData BaseAttributeData => baseAttributeData;

        [ShowInInspector, ReadOnly]
        public EntityAttributes Attributes { get; protected set; }

        protected virtual void Start()
        {
            if ( BaseAttributeData != null )
            {
                Attributes = new EntityAttributes( BaseAttributeData.Attributes );

                MaxHealth = Attributes.Health;

                MaxMana = Attributes.Mana;
            }
        }

        public float MaxHealth { get; private set; } = 0;

        public float MaxMana { get; private set; } = 0;

        public Action<float> OnHealthChanged;

        public Action<float> OnManaChanged;

        public void UpdateHealth( float delta )
        {
            Attributes.Health = Mathf.Clamp( Attributes.Health + delta, 0, MaxHealth );

            OnHealthChanged?.Invoke( Attributes.Health );
        }

        public void UpdateMana( float delta )
        {
            Attributes.Mana = Mathf.Clamp( Attributes.Mana + delta, 0, MaxMana );

            OnManaChanged?.Invoke( Attributes.Mana );
        }

        public void GetHit( int damage, Vector3 hitLocation )
        {
            UpdateHealth( -damage );

            if ( Attributes.Health <= 0 )
            {
                OnDeath();
            }
        }

        protected virtual void OnDeath()
        {
            Destroy( gameObject ); //Temporary
        }
    }
}