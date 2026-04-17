using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SmoothLookAt", story: "[Agent] slowly looks at [Target]", category: "Action", id: "929badf26e4bdf3293b5471709975b48")]
public partial class SmoothLookAtAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    public float rotationSpeed = 15f;

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target.Value == null)
            return Status.Failure;

        Transform selfTransform = Agent.Value.transform;

        Vector3 direction = Target.Value.transform.position - selfTransform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return Status.Success;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        selfTransform.rotation = Quaternion.Slerp(
            selfTransform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        // Check if we're close enough to consider rotation "done"
        float angle = Quaternion.Angle(selfTransform.rotation, targetRotation);

        if (angle < 15f) // threshold in degrees
        {
            return Status.Success; // <-- allows tree to move on
        }

        return Status.Running;
    }
}


