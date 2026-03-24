using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsTargetDead", story: "[Target] is dead", category: "Conditions", id: "6eb00b781f3ed529fcf3701dec972a2d")]
public partial class IsTargetDeadCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public override bool IsTrue()
    {
        if (Target.Value == null) return false;

        HealthSystem health = Target.Value.GetComponent<HealthSystem>();
        if (health == null) return false;

        return health.isDead();
    }
}
