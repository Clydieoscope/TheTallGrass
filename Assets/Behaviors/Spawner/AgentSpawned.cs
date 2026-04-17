using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/AgentSpawned")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "AgentSpawned", message: "[Agent] was spawned", category: "Events", id: "d97e0b26d57d35492255ef2f7a8fea9f")]
public sealed partial class AgentSpawned : EventChannel<GameObject> { }

