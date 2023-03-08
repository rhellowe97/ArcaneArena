using Sirenix.OdinInspector;
using UnityEngine;

namespace ArcaneArena.Entity.Character
{
    [System.Serializable]
    public struct CharacterMovement
    {
        [SerializeField] public float acceleration;

        [SerializeField] public float maxSpeed;

        [SerializeField] public float turnSpeed;
    }
}
