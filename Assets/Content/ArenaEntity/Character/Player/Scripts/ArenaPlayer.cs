using ArcaneArena.UI;
using Cinemachine;
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

            CinemachineVirtualCamera playerVCam = GetComponentInChildren<CinemachineVirtualCamera>();

            if ( playerVCam != null )
            {
                playerVCam.transform.SetParent( null );
            }
        }
    }
}
