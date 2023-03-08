using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : PooledObject
{
    [SerializeField] private Transform wallHelper;

    [SerializeField] private float appearSpeed = 0.5f;

    private Tween appearTween = null;

    private float timer = 0f;

    private bool despawn = false;

    public override void Init()
    {

    }

    public override void ResetObject()
    {
        wallHelper.localPosition = Vector3.zero;

        timer = 0f;

        if ( appearTween != null )
        {
            appearTween.Kill();

            appearTween = null;
        }

        despawn = false;
    }

    private void OnEnable()
    {
        appearTween = wallHelper.DOLocalMoveY( 2, appearSpeed ).SetEase( Ease.OutSine );
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if ( timer >= 8f && !despawn )
        {
            despawn = true;

            appearTween = wallHelper.DOLocalMoveY( 0, appearSpeed ).SetEase( Ease.InSine ).OnComplete( () => ObjectPoolManager.Instance.ReturnToPool( gameObject ) );
        }
    }
}
