using ArcaneArena.Entity.Character.Player;
using UnityEngine;

namespace ArcaneArena.Entity.Character.Ability
{
    public class EarthWallAbility : CharacterAbilityBase
    {
        [SerializeField] private GameObject earthWallPrefab;

        private GameObject earthWallInstance;

        private Vector3 targetLocation;

        public override void BeginCast()
        {
            base.BeginCast();

            targetLocation = character.AimTarget.transform.position;
        }

        public override void Cast()
        {
            base.Cast();

            earthWallInstance = ObjectPoolManager.Instance.GetPooled( earthWallPrefab );

            Vector3 earthWallPos = targetLocation;

            earthWallPos.y = 0;

            earthWallInstance.transform.position = earthWallPos;

            Vector3 aimDirection = character.AimTarget.position - character.transform.position;

            aimDirection.y = 0;

            earthWallInstance.transform.rotation = Quaternion.LookRotation( aimDirection, Vector3.up );
        }
    }
}
