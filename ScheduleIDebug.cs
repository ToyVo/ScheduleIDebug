using MelonLoader;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#if NETSTANDARD2_1
using ScheduleOne;
using ScheduleOne.Properties;
using ScheduleOne.Product;
using ScheduleOne.PlayerScripts;
using FishNet.Component.Spawning;
using FishNet.Object;
using VLB;
#elif NET6_0
using Il2CppScheduleOne;
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
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("cuke").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("banana").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("paracetamol").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("donut").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("viagra").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("mouthwash").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("flumedicine").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("gasoline").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("energydrink").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("motoroil").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("megabean").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("chili").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("battery").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("iodine").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("addy").Properties[0]);
                Debug.Property(Registry.GetItem<PropertyItemDefinition>("horsesemen").Properties[0]);
            }
        }
    }

    public static class Debug
    {
        public static void Property(Property property)
        {
            var message =$"{property}";
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
    
    [HarmonyPatch(typeof(PropertyMixCalculator), nameof(PropertyMixCalculator.MixProperties))]
    public static class MixPropertiesPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(
            ref List<Property> existingProperties,
            ref Property newProperty,
            ref EDrugType drugType,
            ref List<Property> __result
        )
        {
            MelonLogger.Msg($"Adding {newProperty.Name} to {drugType}");
            foreach (var property in existingProperties)
            {
                Debug.Property(property);
            }
            Debug.Property(newProperty);
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix(ref List<Property> __result)
        {
            foreach (var property in __result)
            {
                Debug.Property(property);
            }
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
