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
    public class ArenaConstants
    {
        [SerializeField] protected LayerMask environmentLayerMask;
        public LayerMask EnvironmentLayerMask => environmentLayerMask;
    }

    [Title( "Projectile" )]
    [SerializeField] protected ArenaConstants arena;
    public static ArenaConstants Arena => Data.arena;
}