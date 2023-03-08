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
            }
        }

        public Action<int> OnHealthChanged;

        public Action<int> OnManaChanged;

        public void SetHealth( int delta )
        {
            Attributes.Health = Attributes.Health + delta;

            OnHealthChanged?.Invoke( Attributes.Health );
        }

        public void SetMana( int delta )
        {
            Attributes.Mana = Attributes.Mana + delta;

            OnManaChanged?.Invoke( Attributes.Mana );
        }

        public void GetHit( int damage, Vector3 hitLocation )
        {
            SetHealth( -damage );

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