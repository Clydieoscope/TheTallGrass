using UnityEngine;
using Unity.Behavior;

public class GhoulStateHandler : MonoBehaviour
{
    [SerializeField] private BehaviorGraphAgent m_Agent;

    private BlackboardVariable<AgentDied> m_agentDiedBBV;
    private BlackboardVariable<bool> m_isChasingBBV;
    private bool _wasChasing = false;

    private void OnEnable()
    {
        if (m_Agent.GetVariable("AgentDied", out m_agentDiedBBV))
            m_agentDiedBBV.Value.Event += OnAgentDied;
        else
            Debug.LogWarning($"GhoulStateHandler: Could not find 'AgentDied' event channel on {gameObject.name}'s blackboard.");

        if (!m_Agent.GetVariable("IsChasing", out m_isChasingBBV))
            Debug.LogWarning($"GhoulStateHandler: Could not find 'IsChasing' on {gameObject.name}'s blackboard.");
    }

    private void OnDisable()
    {
        if (m_agentDiedBBV != null)
            m_agentDiedBBV.Value.Event -= OnAgentDied;
    }

    private void Update()
    {
        if (m_isChasingBBV == null) return;

        bool isChasing = m_isChasingBBV.Value;

        if (isChasing != _wasChasing)
            Debug.Log($"{gameObject.name} IsChasing changed: {_wasChasing} → {isChasing}");

        if (isChasing && !_wasChasing)
        {
            Debug.Log($"{gameObject.name} entered chase. Registering.");
            GhoulChaseTracker.Instance.RegisterChase();
        }
        else if (!isChasing && _wasChasing)
        {
            Debug.Log($"{gameObject.name} exited chase. Unregistering.");
            GhoulChaseTracker.Instance.UnregisterChase();
        }

        _wasChasing = isChasing;
    }
    public void OnDeath()
    {
        m_agentDiedBBV?.Value.SendEventMessage(gameObject);
    }

    private void OnAgentDied(GameObject agent)
    {
        // Ensure chase is unregistered if the ghoul dies mid-chase
        if (_wasChasing)
        {
            GhoulChaseTracker.Instance.UnregisterChase();
            _wasChasing = false;
        }

        Debug.Log($"{agent.name} has died.");
    }
}