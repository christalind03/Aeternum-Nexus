using UnityEngine;

public abstract class EnemyState : BaseState<Enemy.EEnemyState>
{
    protected EnemyContext Context;

    public void Initialize(EnemyContext context, Enemy.EEnemyState stateKey)
    {
        Context = context;
        base.Initialize(stateKey);
    }
}
