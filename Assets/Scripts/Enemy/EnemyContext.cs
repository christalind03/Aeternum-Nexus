using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    private NavMeshAgent _navMeshAgent;

    public NavMeshAgent NavmeshAgent => _navMeshAgent;

    public EnemyContext(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
    }
}
