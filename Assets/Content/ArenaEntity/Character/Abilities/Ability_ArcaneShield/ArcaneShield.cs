using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneShield : MonoBehaviour, IHittable
{
    [SerializeField] private PooledEffect blockEffectPrefab;

    public void GetHit( int damage, Vector3 hitLocation )
    {
        if ( blockEffectPrefab != null )
        {
            PooledEffect blockEffectInstance = ObjectPoolManager.Instance.GetPooled( blockEffectPrefab.gameObject ).GetComponent<PooledEffect>();

            Vector3 localPosition = transform.InverseTransformPoint( hitLocation );

            localPosition.z = 0;

            blockEffectInstance.transform.position = transform.TransformPoint( localPosition );

            blockEffectInstance.transform.forward = transform.forward;

            blockEffectInstance.Play();
        }
    }
}
