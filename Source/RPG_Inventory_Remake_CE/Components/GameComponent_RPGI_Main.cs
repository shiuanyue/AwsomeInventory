﻿using RPG_Inventory_Remake_Common;
using Verse;


namespace RPG_Inventory_Remake_CE
{
    public class GameComponent_RPGI_Main : GameComponent
    {
        public GameComponent_RPGI_Main(Game game)
        {

        }
        public override void FinalizeInit()
        {
            JobGiver_RPGIUnload.JobInProgress = false;
            //if (LoadedModManager.RunningModsListForReading.Any(m => m.Name == "Combat Extended"))
            //{
            //    RPG_GearTab_CE.IsCE = true;
            //}
        }
    }
}
