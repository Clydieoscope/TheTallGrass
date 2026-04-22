using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpawnNewAgent", story: "[Spawner] spawns new [Agent] with [PatrolPoints] and attach to [CurrentAgent]", category: "Action", id: "b3faf1ba9628f25e44a76c98f09c3f9a")]
public partial class SpawnNewAgentAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Spawner;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> CurrentAgent;
    [SerializeReference] public BlackboardVariable<List<GameObject>> PatrolPoints;
    public static Action<GameObject, GameObject> OnAgentSpawned;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Spawner.Value == null || Agent.Value == null)
            return Status.Failure;

        if (CurrentAgent.Value != null && !CurrentAgent.Value.GetComponent<HealthSystem>().IsDead())
            return Status.Failure;

        // Spawn position (you can adjust this)
        Vector3 spawnPos = Spawner.Value.transform.position;
        Quaternion spawnRot = Quaternion.identity;

        // Instantiate a fresh copy of the prefab
        GameObject newAgent = UnityEngine.Object.Instantiate(Agent.Value, spawnPos, spawnRot);

        // Set agent patrol points
        var behaviorAgent = newAgent.GetComponent<BehaviorGraphAgent>();
        behaviorAgent.BlackboardReference.SetVariableValue("PatrolPoints", PatrolPoints.Value);

        CurrentAgent.Value = newAgent;

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

