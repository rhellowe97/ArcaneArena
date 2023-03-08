using ArcaneArena.Entity.Character.Ability;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity.Character
{
    public class CharacterCombat<TCharacter> : CharacterComponent<TCharacter> where TCharacter : ArenaCharacter
    {
        [SerializeField] protected List<CharacterAbilityBase> abilities;
        public List<CharacterAbilityBase> Abilities => abilities;

        [ShowInInspector, ReadOnly]
        protected CharacterAbilityBase currentAbility = null;

        public override void Init( ArenaCharacter ownerCharacter )
        {
            base.Init( ownerCharacter );

            foreach ( CharacterAbilityBase ability in abilities )
            {
                if ( ability != null )
                    ability.Init( character );
            }

            character.AnimationEventHandler.OnCastBegin += AnimationEventHandler_OnCastBegin;

            character.AnimationEventHandler.OnCast += AnimationEventHandler_OnCast;

            character.AnimationEventHandler.OnCastEnd += AnimationEventHandler_OnEndCast;

            character.OnStateChanged += Character_OnStateChanged;
        }

        private void Character_OnStateChanged( CharacterState oldState, CharacterState newState )
        {
            if ( newState == CharacterState.Evading )
            {
                if ( currentAbility != null )
                {
                    currentAbility.EndAbility( true );
                }
            }
        }

        private void AnimationEventHandler_OnCastBegin()
        {
            if ( currentAbility.Active )
            {
                character.SetState( currentAbility.Data.CharacterState );

                currentAbility.BeginCast();
            }
        }

        private void AnimationEventHandler_OnCast()
        {
            if ( currentAbility != null && currentAbility.Casting )
                currentAbility.Cast();
        }

        private void AnimationEventHandler_OnEndCast()
        {
            if ( currentAbility != null && currentAbility.Casting )
                currentAbility.CastEnd();
        }

        public bool HasAbility( int index )
        {
            return abilities.Count > index && abilities[index] != null;
        }

        public void UseAbility( int index )
        {
            if ( HasAbility( index ) && ( currentAbility == null || ( currentAbility == Abilities[index] || ( !currentAbility.Active && !currentAbility.Ready ) ) ) )
            {
                Abilities[index].Prepare();

                currentAbility = Abilities[index];
            }
        }

        public void EndAbility( int index )
        {
            if ( HasAbility( index ) )
            {
                if ( Abilities[index].Active )
                {
                    Abilities[index].EndAbility();
                }

                if ( Abilities[index] == currentAbility )
                {
                    foreach ( CharacterAbilityBase<TCharacter> ability in abilities )
                    {
                        if ( ability.Ready )
                        {
                            currentAbility = ability;
                        }
                    }
                }
            }
        }
    }
}
