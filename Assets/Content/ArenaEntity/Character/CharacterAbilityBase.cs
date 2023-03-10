using ArcaneArena.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity.Character.Ability
{
    public abstract class CharacterAbilityBase : CharacterComponent
    {
        [SerializeField] protected CharacterAbilityData data;
        public CharacterAbilityData Data => data;

        public bool Ready { get; private set; } = false; //Input is active, ability will try to fire. Single Cast will try to fire once then un-ready.

        public bool Active { get; private set; } = false; //Is this ability currently active

        public bool Casting { get; private set; } = false; //Is the casting part of this ability active (Playing continuous effects during ability)

        private string abilityAnim;

        public bool CanCast =>
            castTimer >= Data.Cooldown &&
            character.Attributes.Mana >= Data.ManaCost &&
            IsValidCastState();

        public virtual bool IsValidCastState()
        {
            return character.State == CharacterState.Free;
        }

        private float castTimer = 0f;

        public override void Init( ArenaCharacter ownerCharacter )
        {
            base.Init( ownerCharacter );

            switch ( Data.AnimType )
            {
                case CharacterAbilityData.AbilityAnimType.BasicCast:

                    abilityAnim = CharacterAnimationParams.BasicCast;

                    break;
                case CharacterAbilityData.AbilityAnimType.HeavyCast:

                    abilityAnim = CharacterAnimationParams.HeavyCast;

                    break;
                case CharacterAbilityData.AbilityAnimType.Summon:

                    abilityAnim = CharacterAnimationParams.Summon;

                    break;
                case CharacterAbilityData.AbilityAnimType.Block:

                    abilityAnim = CharacterAnimationParams.Block;

                    break;
            }
        }

        protected virtual void Start()
        {
            if ( Data != null )
            {
                castTimer = Data.Cooldown;
            }
            else
            {
                Debug.LogError( $"Data {this.name} is missing Ability Data." );

                Destroy( this );
            }
        }

        protected virtual void Update()
        {
            if ( castTimer < Data.Cooldown )
            {
                castTimer += Time.deltaTime;
            }

            if ( Ready )
            {
                if ( !Active )
                {
                    Activate();
                }

                switch ( Data.CastType )
                {
                    case CharacterAbilityData.AbilityCastType.Continuous:
                        if ( Casting )
                        {
                            Cast();
                        }
                        break;
                }
            }
        }

        public virtual void Prepare() //Put this ability in a state where it wants to cast
        {
            Ready = true;

            Activate();

            if ( !Active && !( Data.CastType == CharacterAbilityData.AbilityCastType.Repeat ) )
            {
                Ready = false;
            }
        }

        public virtual void Activate()
        {
            if ( CanCast )
            {
                Active = true;

                character.Animator.SetBool( abilityAnim, true );
            }
        }

        public virtual void BeginCast() //Cast animation has begun
        {
            if ( Data.CastType == CharacterAbilityData.AbilityCastType.Single || Data.CastType == CharacterAbilityData.AbilityCastType.Repeat )
                Casting = true;

            character.Animator.SetBool( CharacterAnimationParams.Channel, Data.CastType == CharacterAbilityData.AbilityCastType.Continuous );
        }

        public virtual void Cast() //Fire ability
        {
            if ( !Casting && Data.CastType == CharacterAbilityData.AbilityCastType.Continuous )
                Casting = true;

            character.UpdateMana( -Data.ManaCost );

            castTimer = 0f;
        }

        public virtual void CastEnd() //Casting is over
        {
            if ( character.State == Data.CharacterState )
                character.SetState( CharacterState.Free );

            character.Animator.SetBool( abilityAnim, false );

            Casting = false;

            Active = false;

            if ( Data.CastType == CharacterAbilityData.AbilityCastType.Single )
            {
                Ready = false;
            }
        }

        public virtual void EndAbility( bool interrupt = false )
        {
            Ready = false;

            if ( Active )
            {
                character.Animator.SetBool( CharacterAnimationParams.Channel, false );
            }

            if ( interrupt )
            {
                if ( Active )
                {
                    character.Animator.SetTrigger( CharacterAnimationParams.Interrupt );

                    if ( character.State == Data.CharacterState )
                    {
                        character.SetState( CharacterState.Free );
                    }
                }

                Active = false;

                Casting = false;
            }

            character.Animator.SetBool( abilityAnim, false );
        }
    }

    public abstract class CharacterAbilityBase<TCharacter> : CharacterAbilityBase where TCharacter : ArenaCharacter
    {
        protected new TCharacter character;
    }
}
