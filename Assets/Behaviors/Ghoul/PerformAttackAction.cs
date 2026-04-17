using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PerformAttack", story: "[Agent] uses attack from [AttackList]", category: "Action", id: "fb870746e87bf3aee5f9a113b580c0f9")]
public partial class PerformAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<List<string>> AttackList;
    private Animator _animator;
    private string _chosenAttack;

    protected override Status OnStart()
    {
        if (Agent?.Value == null)
        {
            Debug.LogWarning("PerformAttackAction: Agent is not assigned.");
            return Status.Failure;
        }

        if (AttackList?.Value == null || AttackList.Value.Count == 0)
        {
            Debug.LogWarning("PerformAttackAction: AttackList is empty or not assigned.");
            return Status.Failure;
        }

        _animator = Agent.Value.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogWarning("PerformAttackAction: No Animator found on Agent.");
            return Status.Failure;
        }

        // Pick a random trigger from the list and fire it
        _chosenAttack = AttackList.Value[UnityEngine.Random.Range(0, AttackList.Value.Count)];
        _animator.SetTrigger(_chosenAttack);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Wait until the animator is no longer in the attack state
        // before signalling success so the behavior graph doesn't
        // move on while the animation is still playing
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(_chosenAttack) && stateInfo.normalizedTime >= 1f)
            return Status.Success;

        // Also succeed if the animator has already transitioned away
        // (e.g. interrupted by a hit reaction)
        if (!stateInfo.IsName(_chosenAttack) && !_animator.IsInTransition(0))
            return Status.Success;

        return Status.Running;
    }

    protected override void OnEnd()
    {
        _animator = null;
        _chosenAttack = null;
    }
}
