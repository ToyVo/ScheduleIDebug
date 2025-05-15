using MelonLoader;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
#if NETSTANDARD2_1
using ScheduleOne.Properties;
using ScheduleOne.Product;
using ScheduleOne.PlayerScripts;
using FishNet.Component.Spawning;
using FishNet.Object;
using VLB;
#elif NET6_0
using Il2CppScheduleOne.Properties;
using Il2CppScheduleOne.Product;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppFishNet.Component.Spawning;
using Il2CppFishNet.Object;
using Il2CppVLB;
#endif

[assembly: MelonInfo(typeof(ScheduleIDebug.Main), "ScheduleIDebug", "1.0.0", "ToyVo", "https://github.com/ToyVo/ScheduleIDebug/releases")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace ScheduleIDebug
{
    public class Main: MelonMod 
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Initializing ScheduleIDebug");
            new HarmonyLib.Harmony("dev.toyvo.scheduleidebug").PatchAll();
            MelonLogger.Msg("Initialized ScheduleIDebug");
        }
    }

    public class ModObject : MonoBehaviour
    {
        private int _counter = 0;
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                var properties = PropertyUtility.Instance.AllProperties;
                PrintProperty(properties[_counter++ % properties.Count]);
                PrintProperty(properties[_counter++ % properties.Count]);
                PrintProperty(properties[_counter++ % properties.Count]);
            }
        }
        
        public void PrintProperty(Property property)
        {
            var message = $"{property}";
            message += $"\n * AddBaseValueMultiple {property.AddBaseValueMultiple}";
            message += $"\n * Addictiveness {property.Addictiveness}";
            message += $"\n * Description {property.Description}";
            message += $"\n * ID {property.ID}";
            message += $"\n * ImplementedPriorMixingRework {property.ImplementedPriorMixingRework}";
            message += $"\n * LabelColor {property.LabelColor}";
            message += $"\n * MixDirection {property.MixDirection}";
            message += $"\n * MixMagnitude {property.MixMagnitude}";
            message += $"\n * Name {property.Name}";
            message += $"\n * ProductColor {property.ProductColor}";
            message += $"\n * Tier {property.Tier}";
            message += $"\n * ValueChange {property.ValueChange}";
            message += $"\n * ValueMultiplier {property.ValueMultiplier}";
            message += $"\n * hideFlags {property.hideFlags}";
            message += $"\n * name {property.name}";
            MelonLogger.Msg(message);
        }
    }
    
    [HarmonyPatch(typeof(PlayerSpawner), "InitializeOnce")]
    public static class PlayerSpawnerPatch
    {
        [HarmonyPostfix]
        public static void Postfix(PlayerSpawner __instance)
        {
            // Get the player
            var playerPrefabField = typeof(PlayerSpawner).GetField("_playerPrefab", BindingFlags.NonPublic | BindingFlags.Instance);
            if (playerPrefabField == null)
            {
                MelonLogger.Error("PlayerSpawner field not found");
                return;
            }
            var playerPrefab = playerPrefabField.GetValue(__instance);
            if (playerPrefab == null)
            {
                MelonLogger.Error("PlayerSpawner field is null");
                return;           
            }
            var player = ((NetworkObject)playerPrefab).GetComponent<Player>();
            if (player == null)
            {
                MelonLogger.Error("PlayerSpawner player is null");
                return;           
            }
            
            // Attach our mod object to the player
            player.GetOrAddComponent<ModObject>();
        }   
    }
}
