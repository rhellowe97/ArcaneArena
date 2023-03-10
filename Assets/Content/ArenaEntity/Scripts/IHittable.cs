
using UnityEngine;

public interface IHittable
{
    public void GetHit( int damage, Vector3 hitLocation );
}

public interface IHitConfigurable
{
    public void Configure( IHittable owner, LayerMask targetLayerMask );
}