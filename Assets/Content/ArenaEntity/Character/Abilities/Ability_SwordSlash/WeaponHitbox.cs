using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity.Character.Ability
{
    [RequireComponent( typeof( BoxCollider ) )]
    public class WeaponHitbox : MonoBehaviour, IHitConfigurable
    {
        public Action<IHittable, Vector3> OnHit;

        private new BoxCollider collider;

        private HashSet<IHittable> hits = new HashSet<IHittable>();

        private IHittable owner;

        private LayerMask targetLayerMask;

        private void Awake()
        {
            collider = GetComponent<BoxCollider>();
        }

        public void Toggle( bool toggle )
        {
            if ( toggle )
                hits.Clear();

            collider.enabled = toggle;
        }

        private void OnTriggerEnter( Collider other )
        {
            if ( other.TryGetComponent( out IHittable hittable ) && hittable != owner && targetLayerMask == ( targetLayerMask | ( 1 << ( ( MonoBehaviour ) hittable ).gameObject.layer ) ) )
            {
                if ( !hits.Contains( hittable ) )
                {
                    hits.Add( hittable );

                    OnHit?.Invoke( hittable, transform.TransformPoint( collider.center ) );
                }
            }
        }

        public void Configure( IHittable owner, LayerMask targetLayerMask )
        {
            this.owner = owner;

            this.targetLayerMask = targetLayerMask;
        }
    }
}
