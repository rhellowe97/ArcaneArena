using ArcaneArena.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity.Character.Player
{
    public class ArenaPlayer : ArenaCharacter
    {
        [SerializeField] private CharacterAttributeUI attributeUI;

        protected override void Start()
        {
            base.Start();

            if ( attributeUI != null )
            {
                attributeUI.AssignCharacter( this );
            }
        }
    }
}
