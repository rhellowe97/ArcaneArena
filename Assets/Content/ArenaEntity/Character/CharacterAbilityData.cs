using ArcaneArena.Entity.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Data
{
    [CreateAssetMenu( menuName = "Database/Character Ability Data" )]
    public class CharacterAbilityData : ScriptableData<CharacterAbilityData>
    {
        public enum AbilityCastType
        {
            Single,
            Repeat,
            Continuous
        }

        public enum AbilityAnimType
        {
            BasicCast,
            HeavyCast,
            Summon,
            Block
        }

        [SerializeField] private float cooldown = 3f;
        public float Cooldown => cooldown;

        [SerializeField] private int manaCost = 0;
        public int ManaCost => manaCost;

        [SerializeField] private AbilityCastType castType = AbilityCastType.Single;
        public AbilityCastType CastType => castType;

        [SerializeField] private AbilityAnimType animType = AbilityAnimType.BasicCast;
        public AbilityAnimType AnimType => animType;

        [SerializeField] private CharacterState characterState = CharacterState.BasicCasting;
        public CharacterState CharacterState => characterState;
    }
}
