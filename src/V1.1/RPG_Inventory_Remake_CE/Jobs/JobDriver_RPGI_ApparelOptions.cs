﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using RimWorld;
using RPG_Inventory_Remake_Common;

namespace RPG_Inventory_Remake_CE
{
    public class JobDriver_RPGI_ApparelOptions : JobDriver
    {

        private int duration;
        private Apparel apparel;
        // -1 for removing, 0 for equiping from inventory, 1 for forced equip
        private int mode;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (TargetThingA.PositionHeld != pawn.Position)
            {
                return pawn.Reserve(TargetThingA, job, 1, -1, null, errorOnFailed);
            }
            return true;
        }

        public override string GetReport()
        {
            string action = mode < 0 ? "Unequip " : "Equip ";
            return job.def.reportString.Insert(0, action);
        }

        public override void Notify_Starting()
        {
            base.Notify_Starting();
            apparel = TargetThingA as Apparel;

            mode = job.count;
            duration = (int)(apparel.GetStatValue(StatDefOf.EquipDelay) * 60f);
            if (mode > 0)
            {
                List<Apparel> wornApparel = pawn.apparel.WornApparel;
                int count = wornApparel.Count;
                Log.Message("wornApparel.Count: " + wornApparel.Count);
                Log.Message("Count: " + count);
                for (int num = 0; num < count; ++num)
                {
                    Log.Message("wornApparel.Count2: " + wornApparel.Count);
                    Log.Message("num: " + num);
                    if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[num].def, pawn.RaceProps.body))
                    {
                        duration += (int)(wornApparel[num].GetStatValue(StatDefOf.EquipDelay) * 60f);
                    }
                }
            }
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.A);
            if (TargetThingA.PositionHeld != pawn.Position)
            {
                this.FailOnBurningImmobile(TargetIndex.A);
                yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
            }
            yield return Toils_General.Wait(10);
            yield return Toils_General.Wait(duration).WithProgressBarToilDelay(TargetIndex.A);
            if (mode == -1)
            {
                yield return new Toil()
                {
                    initAction = delegate
                    {
                        pawn.apparel.Remove(apparel);
                        pawn.inventory.innerContainer.TryAdd(apparel);
                    },
                    atomicWithPrevious = true
                };
            }
            else if (mode >= 0)
            {
                yield return new Toil()
                {
                    initAction = delegate
                    {
                        pawn.inventory.innerContainer.Remove(apparel);
                        Utility.Wear(pawn, apparel, false);
                        if (mode == 1)
                        {
                            pawn.outfits.forcedHandler.SetForced(apparel, true);
                        }
                    },
                    atomicWithPrevious = true
                };
            }
        }
    }
}
