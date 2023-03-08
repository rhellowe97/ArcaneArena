using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

namespace ArcaneArena.Entity.Character.Player
{
    public class PlayerNavigation : CharacterComponent<ArenaPlayer>
    {
        [BoxGroup( "Input" )]
        [SerializeField] private InputActionReference moveActionRef;
        private InputAction moveAction;

        [BoxGroup( "Input" )]
        [SerializeField] private InputActionReference lookActionRef;
        private InputAction lookAction;

        [BoxGroup( "Input" )]
        [SerializeField] private InputActionReference evadeActionRef;
        private InputAction evadeAction;

        [BoxGroup( "Movement" )]
        [SerializeField] private CharacterMovement movement;

        [BoxGroup( "Movement" )]
        [SerializeField] private LayerMask mousecastLayerMask;

        [BoxGroup( "Movement" )]
        [SerializeField] private float evadeImpulse = 15f;

        private Vector3 lookTargetLocation;

        private Vector3 toLookTargetNormalized = Vector3.zero;

        private RaycastHit[] raycastHitBuffer = new RaycastHit[1];

        private Camera mainCamera;

        private Vector3 moveInput = Vector3.zero;

        private Vector2 lookInput = Vector2.zero;

        public override void Init( ArenaCharacter ownerCharacter )
        {
            base.Init( ownerCharacter );

            if ( !PlayerInputManager.Exists )
            {
                Debug.LogError( "Missing PlayerInputManager!" );

                setupSuccessful = false;

                return;
            }

            if ( moveActionRef != null )
            {
                moveAction = PlayerInputManager.Instance.Controls.FindAction( moveActionRef.action.id );

                Debug.Log( moveAction );
            }
            else
            {
                Debug.LogError( "PlayerMovement is missing a Move Action Ref." );

                setupSuccessful = false;
            }

            if ( lookActionRef != null )
            {
                lookAction = PlayerInputManager.Instance.Controls.FindAction( lookActionRef.action.id );
            }
            else
            {
                Debug.LogError( "PlayerMovement is missing a Look Action Ref." );

                setupSuccessful = false;
            }


            if ( evadeActionRef != null )
            {
                evadeAction = PlayerInputManager.Instance.Controls.FindAction( evadeActionRef.action.id );
            }
            else
            {
                Debug.LogError( "PlayerMovement is missing an Evade Action Ref." );

                setupSuccessful = false;
            }

            mainCamera = Camera.main;

            lookTargetLocation = transform.position + transform.forward;

            character.AnimationEventHandler.OnEvadeEnd += CharacterAnimationEventHandler_OnEvadeEnd;

            evadeAction.performed += EvadeAction_Performed;
        }

        private void CharacterAnimationEventHandler_OnEvadeEnd()
        {
            if ( character.State == CharacterState.Evading )
                character.SetState( CharacterState.Free );

            character.Animator.SetBool( CharacterAnimationParams.Evade, false );
        }

        private void Update()
        {
            if ( setupSuccessful )
            {
                moveInput = moveAction.ReadValue<Vector2>();

                moveInput.z = moveInput.y;

                moveInput.y = 0;

                lookInput = lookAction.ReadValue<Vector2>();
            }
        }

        private void FixedUpdate()
        {
            if ( character != null && setupSuccessful && character.CanMove )
            {
                if ( character.Rigidbody.isKinematic )
                {
                    character.Rigidbody.isKinematic = false;

                    character.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                }

                if ( Physics.RaycastNonAlloc( mainCamera.ScreenPointToRay( lookInput ), raycastHitBuffer, 100f, mousecastLayerMask ) > 0 )
                {
                    lookTargetLocation = raycastHitBuffer[0].point;
                }

                lookTargetLocation.y = character.transform.position.y;

                character.AimTarget.position = lookTargetLocation;

                toLookTargetNormalized = ( lookTargetLocation - character.transform.position ).normalized;

                character.Rigidbody.AddForce( moveInput * movement.acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange );

                if ( character.Rigidbody.velocity.sqrMagnitude >= movement.maxSpeed * movement.maxSpeed )
                    character.Rigidbody.velocity = Vector3.ClampMagnitude( character.Rigidbody.velocity, movement.maxSpeed );

                character.transform.rotation = Quaternion.RotateTowards( character.transform.rotation, Quaternion.LookRotation( toLookTargetNormalized, Vector3.up ), movement.turnSpeed * Time.fixedDeltaTime );

                Vector3 localVel = character.transform.InverseTransformVector( character.Rigidbody.velocity );

                character.Animator.SetFloat( CharacterAnimationParams.MoveX, localVel.x / movement.maxSpeed );

                character.Animator.SetFloat( CharacterAnimationParams.MoveY, localVel.z / movement.maxSpeed );
            }
            else
            {
                if ( character.State == CharacterState.HeavyCasting && !character.Rigidbody.isKinematic )
                {
                    character.Rigidbody.isKinematic = true;
                }
            }
        }

        private void EvadeAction_Performed( InputAction.CallbackContext context )
        {
            if ( character.CanMove )
            {
                character.transform.rotation = Quaternion.LookRotation( moveInput, Vector3.up );

                character.SetState( CharacterState.Evading );

                character.Animator.SetBool( CharacterAnimationParams.Evade, true );

                character.Rigidbody.velocity = Vector3.zero;

                character.Rigidbody.AddForce( character.transform.forward * evadeImpulse, ForceMode.Impulse );
            }
        }
    }
}
