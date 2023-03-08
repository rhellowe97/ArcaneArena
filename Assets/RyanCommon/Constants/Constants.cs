using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Create Constants" )]
public class Constants : ScriptableObject
{
    private static Constants Data => ConstantsSingleton.Data;

    [Serializable]
    public class ProjectileConstants
    {
        [SerializeField] protected LayerMask collisionLayerMask;
        public LayerMask CollisionLayerMask => collisionLayerMask;
    }

    [Title( "Projectile" )]
    [SerializeField] protected ProjectileConstants projectile;
    public static ProjectileConstants Projectile => Data.projectile;
}
