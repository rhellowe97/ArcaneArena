using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity.Character
{
    public class ArenaCharacter : ArenaEntity, IHittable,
#if UNITY_EDITOR
        ISerializationCallbackReceiver
#endif
    {
        public bool CanMove => State != CharacterState.Evading && State != CharacterState.HeavyCasting;

        [SerializeField] protected new Rigidbody rigidbody;
        public Rigidbody Rigidbody => rigidbody;

        [SerializeField] protected new Collider collider;
        public Collider Collider => collider;

        [SerializeField] protected Animator animator;
        public Animator Animator => animator;

        [SerializeField] protected Transform aimTarget;
        public Transform AimTarget => aimTarget;

        [SerializeField] protected CharacterAnimationEventHandler animationEventHandler;
        public CharacterAnimationEventHandler AnimationEventHandler => animationEventHandler;

        [ShowInInspector, ReadOnly]
        protected List<CharacterComponent> subComponents = new List<CharacterComponent>();

        public CharacterState State = CharacterState.Free;

        public Action<CharacterState, CharacterState> OnStateChanged;

        public void SetState( CharacterState newState )
        {
            CharacterState oldState = State;

            State = newState;

            OnStateChanged?.Invoke( oldState, newState );
        }

        protected override void Start()
        {
            base.Start();

            if ( Rigidbody == null )
            {
                if ( !TryGetComponent( out rigidbody ) )
                    Debug.LogError( "Character missing a Rigidbody reference." );
            }

            if ( Collider == null )
            {
                if ( !TryGetComponent( out collider ) )
                    Debug.LogError( "Character missing a Collider reference." );
            }

            if ( Animator == null )
            {
                if ( !TryGetComponent( out animator ) )
                    Debug.LogError( "Character missing a Collider reference." );
            }


            GetComponentsInChildren( subComponents );

            foreach ( CharacterComponent component in subComponents )
            {
                component.Init( this );
            }
        }

        protected virtual void Update()
        {

        }

#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            GetComponentsInChildren( subComponents ); //Fetch components in editor to verify they will setup correctly in-game
        }

        public void OnAfterDeserialize()
        {

        }
#endif
    }

    public enum CharacterState
    {
        Free,
        BasicCasting,
        HeavyCasting,
        Evading,
        Blocking
    }

    public class CharacterAnimationParams
    {
        public const string MoveX = "MoveX";

        public const string MoveY = "MoveY";

        public const string Evade = "Evade";

        public const string BasicCast = "BasicCast";

        public const string HeavyCast = "HeavyCast";

        public const string Summon = "Summon";

        public const string Block = "Block";

        public const string AttackVariation = "AttackVariation";

        public const string Channel = "Channel";
    }
}