﻿using Verse;
using HarmonyLib;
using RimWorld;

namespace ATReforged
{
    internal class ThinkNode_ConditionalNeedPercentageAbove_Patch
    {
        // Automatically satisfy all non-existant needs. TODO: Ensure satisfying non-existant needs in this way is proper and necessary.
        [HarmonyPatch(typeof(ThinkNode_ConditionalNeedPercentageAbove), "Satisfied")]
        public class Satisfied_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn pawn, ref bool __result, NeedDef  ___need)
            {
                if (pawn.needs.TryGetNeed(___need) == null)
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }
    }
}