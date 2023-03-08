using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity.Character
{
    public abstract class CharacterComponent : MonoBehaviour
    {
        protected ArenaCharacter character;

        protected bool setupSuccessful = false;

        public virtual void Init( ArenaCharacter ownerCharacter )
        {
            character = ownerCharacter;

            setupSuccessful = true;
        }
    }

    public abstract class CharacterComponent<TCharacter> : CharacterComponent where TCharacter : ArenaCharacter
    {
        protected TCharacter Character => ( TCharacter ) character;

        public override void Init( ArenaCharacter ownerCharacter )
        {
            if ( !( ownerCharacter is TCharacter ) )
            {
                Debug.LogError( $"Component {this} is wrongly assigned. Need {typeof( TCharacter )} type." );

                enabled = false;

                return;
            }

            base.Init( ownerCharacter );
        }
    }
}
