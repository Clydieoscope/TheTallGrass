using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsAgentAlive", story: "[Agent] is alive", category: "Conditions", id: "3211ce14a3e174cfefd7f67dba6da739")]
public partial class IsAgentAliveCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    public override bool IsTrue()
    {
        if (Agent.Value == null) return false;

        HealthSystem health = Agent.Value.GetComponent<HealthSystem>();
        if (health == null) return false;

        // Debug.Log(health.IsDead()!);

        return health.IsDead()!;
    }
}
