using MelonLoader;
using HarmonyLib;
using System.Collections.Generic;
#if NETSTANDARD2_1
using ScheduleOne.Properties;
using ScheduleOne.Product;
#elif NET6_0
using Il2CppScheduleOne.Properties;
using Il2CppScheduleOne.Product;
#endif

[assembly: MelonInfo(typeof(ScheduleIDebug.Main), "ScheduleIDebug", "1.0.0", "ToyVo")]
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
}
