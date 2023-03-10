using ArcaneArena.Entity.Character;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneArena.UI
{
    [RequireComponent( typeof( RectTransform ) )]
    public class CharacterAttributeUI : PooledObject
    {
        [SerializeField] private Slider healthBar;

        [SerializeField] private Slider manaBar;

        private ArenaCharacter assignedCharacter;

        public RectTransform Rect { get; private set; }

        private void Awake()
        {
            Rect = GetComponent<RectTransform>();
        }

        public void AssignCharacter( ArenaCharacter character )
        {
            assignedCharacter = character;

            if ( assignedCharacter == null )
                return;

            if ( healthBar != null )
            {
                assignedCharacter.OnHealthChanged += Character_OnHealthChanged;
            }

            if ( manaBar != null )
            {
                assignedCharacter.OnManaChanged += Character_OnManaChanged;
            }
        }

        private void Character_OnHealthChanged( float newHealth )
        {
            healthBar.value = ( float ) newHealth / assignedCharacter.BaseAttributeData.Attributes.Health;
        }

        private void Character_OnManaChanged( float newMana )
        {
            manaBar.value = ( float ) newMana / assignedCharacter.BaseAttributeData.Attributes.Mana;
        }

        protected virtual void OnDestroy()
        {
            if ( assignedCharacter != null )
            {
                assignedCharacter.OnHealthChanged -= Character_OnHealthChanged;

                assignedCharacter.OnManaChanged -= Character_OnManaChanged;
            }
        }

        public override void ResetObject()
        {
            base.ResetObject();

            if ( healthBar != null )
                healthBar.value = 1;

            if ( manaBar != null )
                manaBar.value = 1;
        }
    }
}
