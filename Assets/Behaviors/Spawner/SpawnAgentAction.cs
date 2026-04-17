using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Spawn Agent", story: "[Spawner] spawns [Agent]", category: "Action", id: "18d06d7108667b87fa8fac084d1bb449")]
public partial class SpawnAgentAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Spawner;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    public static Action<GameObject, GameObject> OnAgentSpawned;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Spawner.Value == null || Agent.Value == null)
            return Status.Failure;

        // Spawn position (you can adjust this)
        Vector3 spawnPos = Spawner.Value.transform.position;
        Quaternion spawnRot = Quaternion.identity;

        // Instantiate a fresh copy of the prefab
        GameObject newAgent = UnityEngine.Object.Instantiate(Agent.Value, spawnPos, spawnRot);

        GameEvents.OnAgentSpawned?.Invoke(newAgent, Spawner.Value);

        return Status.Success;
    }

    protected override void OnEnd()
    {
        
    }
}

public static class GameEvents
{
    public static Action<GameObject, GameObject> OnAgentSpawned;
}

