using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ArcaneArena.Entity.Character.Enemy
{
    [RequireComponent( typeof( ArenaEnemy ) )]
    public class EnemyRangedCombat : CharacterCombat<ArenaEnemy>
    {
        [SerializeField] private float attackRange = 8f;

        private void Update()
        {
            if ( Character.HasLineOfSight && ( Character.PlayerTarget.transform.position - Character.transform.position ).sqrMagnitude < attackRange * attackRange && Vector3.Dot( ( Character.PlayerTarget.transform.position - Character.transform.position ).normalized, Character.transform.forward ) > 0.8f )
            {
                if ( currentAbility == null || !currentAbility.Ready )
                    UseAbility( 0 ); //Internal ability logic controls cooldowns etc.
            }
            else if ( !Character.HasLineOfSight && currentAbility != null && !currentAbility.Active )
            {
                currentAbility.EndAbility( true );

                currentAbility = null;
            }
        }
    }
}
