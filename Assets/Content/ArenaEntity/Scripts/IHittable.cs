
using UnityEngine;

public interface IHittable
{
    public void GetHit( int damage, Vector3 hitLocation );
}
