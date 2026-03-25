using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using StarterAssets;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RangeDetector", story: "Update Range [Detector] and assign [Target]", category: "Action", id: "64e5c0a83f1160570faa7bf904f68552")]
public partial class RangeDetectorAction : Action
{
    [SerializeReference] public BlackboardVariable<RangeDetector> Detector;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnUpdate()
    {
        Target.Value = Detector.Value.UpdateDetector();
        return Target.Value == null ? Status.Failure : Status.Success;
    }

}

