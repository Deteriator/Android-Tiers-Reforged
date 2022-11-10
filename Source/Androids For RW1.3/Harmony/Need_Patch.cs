﻿using Verse;
using HarmonyLib;
using RimWorld;

namespace ATReforged
{
    internal class Need_Patch
    {
        // Store the energy translation locally to avoid having to constantly translate it.
        private static string energyTranslated = "ATR_EnergyNeed".Translate();
        private static string energyDescTranslated = "ATR_EnergyNeedDesc".Translate();

        // Mechanicals don't have a food meter, they have an energy meter. Since we're hijacking hunger, change the labelled name for mechanicals.
        [HarmonyPatch(typeof(Need), "get_LabelCap")]
        public class get_LabelCap
        {
            [HarmonyPostfix]
            public static void Listener( ref string __result, Pawn ___pawn, Need __instance)
            {
                if (__instance.def.defName == "Food")
                {
                    if (Utils.CanUseBattery(___pawn))
                    {
                        __result = energyTranslated;
                    }
                }
            }
        }

        // Ensure the tooltip for hunger displays a tooltip about energy for mechanicals.
        [HarmonyPatch(typeof(Need_Food), "GetTipString")]
        public class GetTipString_Patch
        {
            [HarmonyPostfix]
            public static void Listener(ref string __result, Pawn ___pawn, Need __instance)
            {
                if (__instance.def.defName == "Food")
                {
                    if (Utils.CanUseBattery(___pawn))
                    {
                        __result = string.Concat(new string[]
                            {
                                __instance.LabelCap,
                                ": ",
                                __instance.CurLevelPercentage.ToStringPercent(),
                                " (",
                                __instance.CurLevel.ToString("0.##"),
                                " / ",
                                __instance.MaxLevel.ToString("0.##"),
                                ")\n",
                                energyDescTranslated
                            });
                    }
                }
            }
        }
    }
}