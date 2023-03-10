using UnityEngine;
using UnityEngine.AI;

namespace ArcaneArena.Entity.Character.Enemy
{
    public class EnemyChase : EnemyNavigation
    {
        [SerializeField] protected float stoppingDistance = 0.5f;

        protected float defaultStoppingDistance = 0.5f;

        private bool inRange = false;

        private void Start()
        {
            navPath = new NavMeshPath();

            defaultStoppingDistance = stoppingDistance;
        }
        protected virtual void SetGoalDestination()
        {
            goalDestination = goalLookTarget = Character.PlayerTarget.transform.position;
        }

        protected override void ProcessNavigation()
        {
            SetGoalDestination();

            movementVector = Vector3.zero;

            if ( !inRange )
            {
                if ( NavMesh.CalculatePath( character.transform.position, goalDestination, NavMesh.AllAreas, navPath ) )
                {
                    navPath.GetCornersNonAlloc( navMeshCorners );

                    if ( navMeshCorners.Length > 1 )
                    {
                        movementVector = ( navMeshCorners[1] - navMeshCorners[0] ).normalized;
                    }
                }

                if ( ( goalDestination - character.transform.position ).sqrMagnitude < stoppingDistance * stoppingDistance )
                {
                    inRange = true;
                }
            }
            else
            {
                if ( ( goalDestination - character.transform.position ).sqrMagnitude > ( stoppingDistance + 1 ) * ( stoppingDistance + 1 ) )
                {
                    inRange = false;
                }
            }

            character.AimTarget.transform.position = Character.PlayerTarget.transform.position;
        }
    }
}
