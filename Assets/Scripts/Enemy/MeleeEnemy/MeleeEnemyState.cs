using UnityEngine;

public abstract class MeleeEnemyState : BaseState<MeleeEnemy.EEnemyState>
{
    protected MeleeEnemyContext Context;

    public MeleeEnemyState(MeleeEnemyContext context, MeleeEnemy.EEnemyState stateKey) : base(stateKey)
    {
        Context = context;
    }
}
