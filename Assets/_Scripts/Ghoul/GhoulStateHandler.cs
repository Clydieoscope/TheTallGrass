using UnityEngine;
using Unity.Behavior;

public class GhoulStateHandler : MonoBehaviour
{
    [SerializeField] private BehaviorGraphAgent m_Agent;

    private BlackboardVariable<AgentDied> m_agentDiedBBV;

    private void OnEnable()
    {
        if (m_Agent.GetVariable("AgentDied", out m_agentDiedBBV))
            m_agentDiedBBV.Value.Event += OnAgentDied;
        else
            Debug.LogWarning($"GhoulStateHandler: Could not find 'AgentDied' event channel on {gameObject.name}'s blackboard.");
    }

    private void OnDisable()
    {
        if (m_agentDiedBBV != null)
            m_agentDiedBBV.Value.Event -= OnAgentDied;
    }

    // Wire to HealthSystem.OnDeath in the Inspector
    public void OnDeath()
    {
        m_agentDiedBBV?.Value.SendEventMessage(gameObject);
    }

    private void OnAgentDied(GameObject agent)
    {
        Debug.Log($"{agent.name} has died.");
    }
}