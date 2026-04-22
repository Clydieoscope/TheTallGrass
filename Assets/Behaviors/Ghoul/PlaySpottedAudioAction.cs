using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Play Spotted Audio", story: "Play spotted audio", category: "Action", id: "9fdc83b3b8c38d37d0626eb7248fce31")]
public partial class PlaySpottedAudioAction : Action
{
    protected override Status OnStart()
    {
        AmbientSFXHandler.Instance?.PlaySpotted();
        return Status.Success;
    }
}

