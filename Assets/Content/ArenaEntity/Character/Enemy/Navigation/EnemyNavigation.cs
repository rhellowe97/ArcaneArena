using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ArcaneArena.Entity.Character.Enemy
{
    public abstract class EnemyNavigation : CharacterComponent<ArenaEnemy>
    {
        [SerializeField] protected float navigationTickRate = 0.2f;

        [SerializeField] protected CharacterMovement movement;

        private float navigationTimer = 0f;

        protected Vector3 goalDestination;

        protected Vector3 goalLookTarget;

        protected Vector3 movementVector = Vector3.zero;

        protected NavMeshPath navPath;

        protected Vector3[] navMeshCorners = new Vector3[50];

        protected virtual void Update()
        {
            navigationTimer += Time.deltaTime;

            if ( navigationTimer >= navigationTickRate )
            {
                ProcessNavigation();

                navigationTimer = 0f;
            }
        }

        protected virtual void FixedUpdate()
        {
            if ( character != null )
            {
                if ( character.CanMove )
                {
                    if ( character.Rigidbody.isKinematic )
                    {
                        character.Rigidbody.isKinematic = false;

                        character.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    }

                    character.Rigidbody.AddForce( movementVector * movement.acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange );

                    if ( character.Rigidbody.velocity.sqrMagnitude >= movement.maxSpeed * movement.maxSpeed )
                        character.Rigidbody.velocity = Vector3.ClampMagnitude( character.Rigidbody.velocity, movement.maxSpeed );

                    Vector3 toLookTarget = goalLookTarget - character.transform.position;

                    toLookTarget.y = 0;

                    character.transform.rotation = Quaternion.RotateTowards( character.transform.rotation, Quaternion.LookRotation( toLookTarget, Vector3.up ), movement.turnSpeed * Time.fixedDeltaTime );
                }
                else
                {
                    if ( character.State == CharacterState.HeavyCasting && !character.Rigidbody.isKinematic )
                    {
                        character.Rigidbody.isKinematic = true;
                    }
                }

                Vector3 localVel = character.transform.InverseTransformVector( character.Rigidbody.velocity );

                character.Animator.SetFloat( CharacterAnimationParams.MoveX, localVel.x / movement.maxSpeed );

                character.Animator.SetFloat( CharacterAnimationParams.MoveY, localVel.z / movement.maxSpeed );
            }
        }

        protected abstract void ProcessNavigation();
    }
}
