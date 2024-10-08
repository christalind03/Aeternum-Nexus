using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyContext
{
    private NavMeshAgent _navMeshAgent;

    public NavMeshAgent NavmeshAgent => _navMeshAgent;

    public MeleeEnemyContext(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
    }
}
