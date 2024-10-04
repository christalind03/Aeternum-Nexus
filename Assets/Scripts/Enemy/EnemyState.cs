using UnityEngine;

public abstract class EnemyState : BaseState<EnemyStateMachine.EEnemyState>
{
    protected EnemyContext Context;

    public EnemyState(EnemyContext context, EnemyStateMachine.EEnemyState stateKey) : base(stateKey)
    {
        Context = context;
    }
}
