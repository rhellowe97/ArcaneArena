using ArcaneArena.Entity;
using UnityEngine;

namespace ArcaneArena.Data
{
    [CreateAssetMenu( menuName = "Database/Entity Attribute Data" )]
    public class EntityAttributeData : ScriptableData<EntityAttributeData>
    {
        [SerializeField] protected EntityAttributes attributes;
        public EntityAttributes Attributes => attributes;
    }
}
