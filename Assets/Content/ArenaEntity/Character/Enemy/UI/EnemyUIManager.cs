using ArcaneArena.Entity.Character.Enemy;
using ArcaneArena.UI;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent( typeof( Canvas ) )]
public class EnemyUIManager : Singleton<EnemyUIManager>
{
    [SerializeField] private GameObject enemyAttributeUI;

    private Dictionary<ArenaEnemy, CharacterAttributeUI> activeEnemyInterfaces = new Dictionary<ArenaEnemy, CharacterAttributeUI>();

    private Canvas enemyUICanvas;

    protected override void Awake()
    {
        base.Awake();

        enemyUICanvas = GetComponent<Canvas>();
    }

    public CharacterAttributeUI Subscribe( ArenaEnemy requestingEnemy )
    {
        CharacterAttributeUI enemyUIInstance = null;

        if ( !activeEnemyInterfaces.ContainsKey( requestingEnemy ) )
        {
            enemyUIInstance = ObjectPoolManager.Instance.GetPooled( enemyAttributeUI, enemyUICanvas.transform ).GetComponent<CharacterAttributeUI>();

            if ( enemyUIInstance != null )
                activeEnemyInterfaces.Add( requestingEnemy, enemyUIInstance );
        }

        return enemyUIInstance;
    }

    public void UnSubscribe( ArenaEnemy requestingEnemy )
    {
        if ( activeEnemyInterfaces.ContainsKey( requestingEnemy ) )
        {
            CharacterAttributeUI enemyUIInstance = activeEnemyInterfaces[requestingEnemy];

            if ( enemyUIInstance != null )
            {
                ObjectPoolManager.Instance.ReturnToPool( enemyUIInstance.gameObject );
            }

            activeEnemyInterfaces.Remove( requestingEnemy );
        }
    }
}
