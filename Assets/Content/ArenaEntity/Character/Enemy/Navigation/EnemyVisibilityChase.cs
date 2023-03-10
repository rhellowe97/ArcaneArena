using UnityEngine;
using UnityEngine.AI;

namespace ArcaneArena.Entity.Character.Enemy
{
    public class EnemyVisibilityChase : EnemyChase
    {
        [SerializeField] private float visibilityCheckRate = 1f;

        [SerializeField] private LayerMask environmentLayerMask;

        private float visibilityCheckTimer = 0f;

        private NavMeshPath visibilityPath;

        private Vector3[] visibilityPathCorners = new Vector3[2];

        private bool hasVisibility = true;

        public override void Init( ArenaCharacter ownerCharacter )
        {
            base.Init( ownerCharacter );

            visibilityPath = new NavMeshPath();

            GenerateVisibilityPath();
        }

        protected override void ProcessNavigation()
        {
            visibilityCheckTimer += navigationTickRate;

            if ( visibilityCheckTimer >= visibilityCheckRate )
            {
                visibilityCheckTimer = 0f;

                GenerateVisibilityPath();
            }

            base.ProcessNavigation();
        }

        protected override void SetGoalDestination()
        {
            if ( !hasVisibility )
            {
                goalDestination = visibilityPathCorners[1];

                stoppingDistance = 0.2f;
            }
            else
            {
                stoppingDistance = defaultStoppingDistance;

                base.SetGoalDestination();
            }
        }

        private void GenerateVisibilityPath()
        {
            hasVisibility = true;

            if ( Character.PlayerTarget != null )
            {
                if ( !Character.HasLineOfSight )
                {
                    bool validPath = NavMesh.CalculatePath( Character.PlayerTarget.transform.position, Character.transform.position, NavMesh.AllAreas, visibilityPath );

                    if ( validPath )
                    {
                        int cornerCount = visibilityPath.GetCornersNonAlloc( visibilityPathCorners );

                        if ( cornerCount < 2 || visibilityPath.status == NavMeshPathStatus.PathPartial )
                        {
                            hasVisibility = true; //Ignore visibility pathing if there isn't a full path to use
                        }
                        else
                        {
                            hasVisibility = false;
                        }
                    }
                }
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if ( Character != null && !Character.HasLineOfSight )
            {
                Gizmos.color = Color.magenta;

                if ( visibilityPathCorners != null )
                {
                    if ( visibilityPathCorners[0] != null && visibilityPathCorners[1] != null )
                    {
                        Gizmos.DrawLine( visibilityPathCorners[0], visibilityPathCorners[1] );
                    }
                }
            }

            Gizmos.color = Color.red;

            Gizmos.DrawSphere( goalDestination, 0.5f );
        }
#endif
    }
}
