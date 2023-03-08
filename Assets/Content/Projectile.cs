using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PooledObject
{
    [SerializeField] protected ProjectileData data;
    public ProjectileData Data => data;

    [SerializeField] protected PooledEffect impactEffectPrefab;

    private Transform aimTarget;

    private float lifeTimer = 0f;

    private float distanceChunk = 0f;

    private float travelledDistance = 0f;

    private RaycastHit collisionRayHit;

    private TrailRenderer projTrail;

    private float currentSpeed = 0f;

    private IHittable owner;

    public void SetOwner( IHittable newOwner )
    {
        owner = newOwner;
    }

    public void SetAimTarget( Transform target )
    {
        aimTarget = target;
    }

    public void InheritVelocity( Vector3 velocity )
    {
        Vector3 newHeading = velocity + transform.forward * Data.Speed;

        transform.forward = newHeading;
    }

    private void FixedUpdate()
    {
        if ( Data.Seeking && aimTarget != null )
        {
            transform.rotation = Quaternion.RotateTowards( transform.rotation, Quaternion.LookRotation( ( aimTarget.transform.position + Vector3.up * 0.5f ) - transform.position, Vector3.up ), Data.SeekRate * Time.fixedDeltaTime );
        }

        Vector3 rayStartPosition = transform.position - ( transform.forward * Mathf.Min( travelledDistance, distanceChunk ) );

        if ( Physics.Raycast( rayStartPosition, transform.forward, out collisionRayHit, 2 * distanceChunk, Constants.Projectile.CollisionLayerMask, QueryTriggerInteraction.Ignore ) )
        {
            if ( collisionRayHit.collider.TryGetComponent( out IHittable hittable ) )
            {
                if ( hittable != owner )
                    hittable.GetHit( Data.Damage, collisionRayHit.point );
            }
            else if ( collisionRayHit.collider.attachedRigidbody != null )
            {
                if ( collisionRayHit.collider.attachedRigidbody.TryGetComponent( out IHittable rbHittable ) )
                {
                    if ( rbHittable != owner )
                        rbHittable.GetHit( Data.Damage, collisionRayHit.point );
                }
            }

            if ( impactEffectPrefab != null )
            {
                PooledEffect impactEffectInstance = ObjectPoolManager.Instance.GetPooled( impactEffectPrefab.gameObject ).GetComponent<PooledEffect>();

                impactEffectInstance.transform.position = collisionRayHit.point;

                impactEffectInstance.transform.rotation = Quaternion.LookRotation( collisionRayHit.normal, Vector3.up );

                impactEffectInstance.Play();
            }

            if ( ObjectPoolManager.Exists )
                ObjectPoolManager.Instance.ReturnToPool( gameObject );
            else
                Destroy( gameObject );
        }
        else
        {
            transform.position += transform.forward * distanceChunk;

            if ( travelledDistance < distanceChunk )
                travelledDistance += distanceChunk;
        }

        lifeTimer += Time.fixedDeltaTime;

        if ( lifeTimer >= Data.LifeDuration )
        {
            if ( ObjectPoolManager.Exists )
                ObjectPoolManager.Instance.ReturnToPool( gameObject );
            else
                Destroy( gameObject );
        }
    }

    public override void Init()
    {
        currentSpeed = Data.Speed;

        distanceChunk = currentSpeed * Time.fixedDeltaTime;

        projTrail = GetComponent<TrailRenderer>();
    }

    public override void ResetObject()
    {
        lifeTimer = 0f;

        travelledDistance = 0f;

        owner = null;
    }

    public override void Returned()
    {
        if ( projTrail != null )
        {
            projTrail.Clear();
        }

        base.Returned();
    }

    public static Projectile Spawn( GameObject projectilePrefab, Vector3 spawnPosition, Vector3 direction, IHittable owner = null )
    {
        GameObject go = ObjectPoolManager.Instance.GetPooled( projectilePrefab );

        if ( go == null )
        {
            return null;
        }
        else
        {
            Projectile projectileInstance = go.GetComponent<Projectile>();

            if ( projectileInstance != null )
            {
                projectileInstance.transform.position = spawnPosition;

                projectileInstance.transform.forward = direction;

                projectileInstance.SetOwner( owner );
            }

            return projectileInstance;
        }
    }
}
