using ArcaneArena.Entity.Character.Player;
using ArcaneArena.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneArena.Entity.Character.Enemy
{
    public class ArenaEnemy : ArenaCharacter
    {
        private const float visibilityCheckRate = 0.5f;

        private float visibilityCheckTimer = 0f;

        [SerializeField] private Transform uiReference;

        private CharacterAttributeUI enemyAttributeUI;

        private bool uiInitialized = false;

        private Camera mainCamera;

        public bool HasLineOfSight { get; private set; }

        public ArenaPlayer PlayerTarget { get; protected set; }

        public void AssignPlayerTarget( ArenaPlayer target )
        {
            PlayerTarget = target;
        }

        protected override void Start()
        {
            ArenaPlayer player = FindObjectOfType<ArenaPlayer>();

            AssignPlayerTarget( player );

            base.Start();

            mainCamera = Camera.main;

            if ( EnemyUIManager.Exists )
            {
                enemyAttributeUI = EnemyUIManager.Instance.Subscribe( this );

                if ( enemyAttributeUI != null )
                {
                    enemyAttributeUI.AssignCharacter( this );

                    uiInitialized = true;

                    SetAttributeUIPosition();
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            HasLineOfSight = !Physics.Linecast( transform.position + Vector3.up, PlayerTarget.transform.position + Vector3.up, Constants.Arena.EnvironmentLayerMask, QueryTriggerInteraction.Ignore );
        }

        private void FixedUpdate()
        {
            SetAttributeUIPosition();
        }

        private void SetAttributeUIPosition()
        {
            if ( uiInitialized )
                enemyAttributeUI.Rect.position = mainCamera.WorldToScreenPoint( uiReference.position );
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            if ( EnemyUIManager.Exists )
            {
                EnemyUIManager.Instance.UnSubscribe( this );
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if ( PlayerTarget != null )
            {
                Gizmos.color = Color.yellow;

                Gizmos.DrawLine( transform.position + Vector3.up, PlayerTarget.transform.position + Vector3.up );
            }
        }
#endif
    }
}
