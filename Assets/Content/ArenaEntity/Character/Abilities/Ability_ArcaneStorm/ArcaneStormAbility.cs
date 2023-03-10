using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity.Character.Ability
{
    public class ArcaneStormAbility : CharacterAbilityBase
    {
        public override void Cast()
        {
            base.Cast();

            Debug.Log( "STORM ACTIVE" );
        }
    }
}
