using UnityEngine;

public class TestEnemyStats : EnemyStats
{
    protected override void DamageProcess()
    {
        
    }

    protected override void DeathProcess()
    {
        enemyStateMachine.ChangeState(EnemySimpleStateMachine.EnemyState.Death);
    }
}
