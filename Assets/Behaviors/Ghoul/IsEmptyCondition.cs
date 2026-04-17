using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsEmpty", story: "[List] is Empty", category: "Conditions", id: "b358dcb92556da279ce25f3a31ece199")]
public partial class IsEmptyCondition : Condition
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> List;

    public override bool IsTrue()
    {
        return List?.Value == null || List.Value.Count == 0;
    }
}
