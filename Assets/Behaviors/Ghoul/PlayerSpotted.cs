using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/PlayerSpotted")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "PlayerSpotted", message: "[Agent] had spotted [Target]", category: "Events", id: "ebe436c4dabf9375eb34253f2024dece")]
public sealed partial class PlayerSpotted : EventChannel<GameObject, GameObject> { }

