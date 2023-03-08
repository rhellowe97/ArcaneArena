using ArcaneArena.Entity.Character.Enemy;
using UnityEngine;

namespace ArcaneArena.Entity.Character.Ability
{
    public class SwordSlashAbility : CharacterAbilityBase
    {
        [SerializeField] private WeaponHitbox swordCollider;

        [SerializeField] private int damage = 10;

        private void Awake()
        {
            if ( swordCollider != null )
                swordCollider.OnHit += WeaponHitbox_OnHit;
        }

        public override void Activate()
        {
            base.Activate();

            if ( Active )
            {
                character.Animator.SetInteger( CharacterAnimationParams.AttackVariation, Random.Range( 0, 2 ) );
            }
        }

        public override void Cast()
        {
            base.Cast();

            swordCollider.Toggle( true );
        }

        public override void CastEnd()
        {
            base.CastEnd();

            swordCollider.Toggle( false );
        }

        private void WeaponHitbox_OnHit( IHittable hittable, Vector3 point )
        {
            hittable.GetHit( damage, point );
        }
    }
}
