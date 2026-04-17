using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/Agent Died")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Agent Died", message: "[Agent] Died", category: "Events", id: "0f52eee8c90d688571c27288e43d25d2")]
public sealed partial class AgentDied : EventChannel<GameObject> { }

