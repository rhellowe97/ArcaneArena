using ArcaneArena.Entity.Character.Ability;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ArcaneArena.Entity.Character.Player
{
    public class PlayerBlock : CharacterComponent<ArenaPlayer>
    {
        [SerializeField] protected CharacterAbilityBase blockAbility;

        [BoxGroup( "Input" )]
        [SerializeField] private InputActionReference blockActionRef;
        private InputAction blockAction;

        private bool blockActive = false;

        public override void Init( ArenaCharacter ownerCharacter )
        {
            base.Init( ownerCharacter );

            blockAbility.Init( character );

            if ( blockActionRef != null )
            {
                blockAction = PlayerInputManager.Instance.Controls.FindAction( blockActionRef.action.id );

                blockAction.performed += BlockAction_Performed;
                blockAction.canceled += BlockAction_Canceled;
            }
            else
            {
                Debug.LogError( "PlayerCombat is missing a FirstAbility Action Ref." );

                setupSuccessful = false;
            }

            character.OnStateChanged += Character_OnStateChanged;
        }

        private void Character_OnStateChanged( CharacterState oldState, CharacterState newState )
        {
            if ( newState == CharacterState.Evading )
            {
                ReleaseBlock();
            }

            if ( newState == CharacterState.Free && blockActive )
            {
                BeginBlock();
            }
        }

        private void BlockAction_Performed( InputAction.CallbackContext obj )
        {
            blockActive = true;

            BeginBlock();
        }

        private void BlockAction_Canceled( InputAction.CallbackContext obj )
        {
            blockActive = false;

            ReleaseBlock();
        }

        public void BeginBlock()
        {
            if ( blockAbility != null )
            {
                if ( character.State == CharacterState.Free || character.State == CharacterState.BasicCasting )
                {
                    blockAbility.Prepare();

                    character.SetState( CharacterState.Blocking );
                }
            }
        }

        public void ReleaseBlock()
        {
            if ( blockAbility != null )
            {
                blockAbility.EndAbility( true );

                if ( character.State == CharacterState.Blocking )
                    character.SetState( CharacterState.Free );
            }
        }
    }
}
