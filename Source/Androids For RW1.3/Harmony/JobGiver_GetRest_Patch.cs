﻿using System;
using Verse;
using Verse.AI;
using HarmonyLib;
using RimWorld;

namespace ATReforged
{
    internal class JobGiver_GetRest_Patch
    {
        // Override job for getting rest based on whether the pawn can charge instead or not. Pawns that can rest and charge will seek to do both simultaneously.
        [HarmonyPatch(typeof(JobGiver_GetRest), "TryGiveJob")]
        public class TryGiveJob_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn pawn, ref Job __result)
            {
                try
                {
                    // If the pawn can't use charging or isn't on a map, then there's nothing to override. 
                    if (pawn != null && pawn.Map != null && Utils.CanUseBattery(pawn))
                    {
                        // Don't override non-spawned or drafted pawns.
                        if (!pawn.Spawned || pawn.Drafted)
                            return;
                        
                        // Attempt to locate a viable charging bed for the pawn. This can suit comfort, rest, and room needs whereas the charging station can not.
                        Building_Bed bed = Utils.GetAvailableChargingBed(pawn);
                        if (bed != null)
                        {
                            __result = new Job(JobDefOf.RechargeBattery, new LocalTargetInfo(bed));
                            return;
                        }
                    }
                    // If there is no viable charging bed or charging station, then the pawn is free to grab whatever food it was originally planning to consume.
                }
                catch (Exception ex)
                {
                    Log.Warning("[ATR] ATReforged.JobGiver_GetRest_Patch Encountered an error while attempting to check pawn" + pawn + " for charging. Default vanilla behavior will proceed." + ex.Message + " " + ex.StackTrace);
                }
            }
        }
    }
}