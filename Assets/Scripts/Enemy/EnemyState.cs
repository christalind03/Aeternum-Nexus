using System;
using UnityEngine;

public abstract class EnemyState<EState> : BaseState<EState> where EState : Enum
{
    protected EnemyContext Context;

    public virtual void Set(EnemyState<EState> otherInstance) { }
    public void Initialize(EnemyContext context, EState stateKey)
    {
        Context = context;
        base.Initialize(stateKey);
    }
}
