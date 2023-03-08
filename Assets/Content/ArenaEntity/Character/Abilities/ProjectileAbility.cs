using ArcaneArena.Entity.Character.Enemy;
using UnityEngine;

namespace ArcaneArena.Entity.Character.Ability
{
    public class ProjectileAbility : CharacterAbilityBase
    {
        [SerializeField] private GameObject projectilePrefab;

        private Projectile projectileInstance;

        [SerializeField] private Transform source;

        [SerializeField] private bool targetLockedCast = false;

        private Vector3 targetLocation;

        public override void BeginCast()
        {
            base.BeginCast();

            targetLocation = character.AimTarget.transform.position;
        }

        public override void Cast()
        {
            base.Cast();

            Projectile.Spawn( projectilePrefab, source.position, GetAimDirection(), character );
        }

        private Vector3 GetAimDirection()
        {
            Vector3 aimDirection = ( targetLockedCast ? targetLocation : character.AimTarget.transform.position ) - source.position;

            aimDirection.y = 0;

            return aimDirection;
        }
    }
}