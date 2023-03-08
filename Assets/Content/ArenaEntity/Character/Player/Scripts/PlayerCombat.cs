using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ArcaneArena.Entity.Character.Player
{
    public class PlayerCombat : CharacterCombat<ArenaPlayer>
    {
        [BoxGroup( "Input" )]
        [SerializeField] private InputActionReference firstAbilityActionRef;
        private InputAction firstAbilityAction;

        [BoxGroup( "Input" )]
        [SerializeField] private InputActionReference secondAbilityActionRef;
        private InputAction secondAbilityAction;

        [BoxGroup( "Input" )]
        [SerializeField] private InputActionReference thirdAbilityActionRef;
        private InputAction thirdAbilityAction;

        public override void Init( ArenaCharacter ownerCharacter )
        {
            base.Init( ownerCharacter );

            if ( firstAbilityActionRef != null )
            {
                firstAbilityAction = PlayerInputManager.Instance.Controls.FindAction( firstAbilityActionRef.action.id );

                firstAbilityAction.performed += FirstAbilityAction_Performed;
                firstAbilityAction.canceled += FirstAbilityAction_Canceled;
            }
            else
            {
                Debug.LogError( "PlayerCombat is missing a FirstAbility Action Ref." );

                setupSuccessful = false;
            }

            if ( secondAbilityActionRef != null )
            {
                secondAbilityAction = PlayerInputManager.Instance.Controls.FindAction( secondAbilityActionRef.action.id );

                secondAbilityAction.performed += SecondAbilityAction_Performed;
                secondAbilityAction.canceled += SecondAbilityAction_Canceled;
            }
            else
            {
                Debug.LogError( "PlayerCombat is missing a SecondAbility Action Ref." );

                setupSuccessful = false;
            }

            if ( thirdAbilityActionRef != null )
            {
                thirdAbilityAction = PlayerInputManager.Instance.Controls.FindAction( thirdAbilityActionRef.action.id );

                thirdAbilityAction.performed += ThirdAbilityAction_Performed;
                thirdAbilityAction.canceled += ThirdAbilityAction_Canceled;
            }
            else
            {
                Debug.LogError( "PlayerCombat is missing a ThirdAbilityRef Action Ref." );

                setupSuccessful = false;
            }
        }

        private void FirstAbilityAction_Performed( InputAction.CallbackContext context )
        {
            if ( HasAbility( 0 ) )
            {
                UseAbility( 0 );
            }
        }

        private void FirstAbilityAction_Canceled( InputAction.CallbackContext context )
        {
            if ( HasAbility( 0 ) )
            {
                Abilities[0].EndAbility();
            }
        }

        private void SecondAbilityAction_Performed( InputAction.CallbackContext context )
        {
            if ( HasAbility( 1 ) )
            {
                UseAbility( 1 );
            }
        }

        private void SecondAbilityAction_Canceled( InputAction.CallbackContext context )
        {
            if ( HasAbility( 1 ) )
            {
                Abilities[1].EndAbility();
            }
        }

        private void ThirdAbilityAction_Performed( InputAction.CallbackContext context )
        {
            if ( HasAbility( 2 ) )
            {
                UseAbility( 2 );
            }
        }

        private void ThirdAbilityAction_Canceled( InputAction.CallbackContext context )
        {

            if ( HasAbility( 2 ) )
            {
                Abilities[2].EndAbility();
            }
        }

        private void OnDestroy()
        {
            if ( firstAbilityAction != null )
            {
                firstAbilityAction.performed -= FirstAbilityAction_Performed;
                firstAbilityAction.canceled -= FirstAbilityAction_Canceled;
            }

            if ( secondAbilityAction != null )
            {
                secondAbilityAction.performed -= SecondAbilityAction_Performed;
                secondAbilityAction.canceled -= SecondAbilityAction_Canceled;
            }

            if ( thirdAbilityAction != null )
            {
                thirdAbilityAction.performed -= ThirdAbilityAction_Performed;
                thirdAbilityAction.canceled -= ThirdAbilityAction_Canceled;
            }
        }
    }
}