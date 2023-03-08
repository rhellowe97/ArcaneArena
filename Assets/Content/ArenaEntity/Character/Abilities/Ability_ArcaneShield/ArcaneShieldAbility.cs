using ArcaneArena.Entity.Character.Player;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity.Character.Ability
{
    public class ArcaneShieldAbility : CharacterAbilityBase
    {
        [SerializeField] private GameObject shield;

        [SerializeField] private Collider shieldCollision;

        [SerializeField] private float appearSpeed = 0.5f;

        private float shieldScale = 0;

        private Tween shieldTween;

        public override bool IsValidCastState()
        {
            return base.IsValidCastState() || character.State == CharacterState.BasicCasting;
        }

        public override void Prepare()
        {
            base.Prepare();

            if ( Active )
            {
                shield.gameObject.SetActive( true );

                shieldCollision.enabled = true;
            }
        }

        protected override void Update()
        {
            base.Update();

            if ( ( !Active && shieldScale > 0 ) || ( Active && shieldScale < 1 ) )
            {
                shieldScale = Mathf.MoveTowards( shieldScale, Active ? 1 : 0, ( 1 / appearSpeed ) * Time.deltaTime );

                shield.transform.localScale = Vector3.one * shieldScale;

                if ( !Active && shieldScale == 0 )
                {
                    shield.SetActive( false );
                }
            }
        }

        public override void EndAbility( bool interrupt = false )
        {
            base.EndAbility( interrupt );

            shieldCollision.enabled = false;
        }
    }
}
