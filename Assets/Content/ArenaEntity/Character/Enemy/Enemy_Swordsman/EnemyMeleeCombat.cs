using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity.Character.Enemy
{
    [RequireComponent( typeof( ArenaEnemy ) )]
    public class EnemyMeleeCombat : CharacterCombat<ArenaEnemy>
    {
        [SerializeField] private float attackRange = 0.5f;

        private void Update()
        {
            if ( ( Character.PlayerTarget.transform.position - Character.transform.position ).sqrMagnitude < attackRange * attackRange && Vector3.Dot( ( Character.PlayerTarget.transform.position - Character.transform.position ).normalized, Character.transform.forward ) > 0.8f )
            {
                if ( currentAbility == null || !currentAbility.Ready )
                    UseAbility( 0 ); //Internal ability logic controls cooldowns etc.
            }
        }
    }
}
