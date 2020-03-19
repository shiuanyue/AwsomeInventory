﻿// <copyright file="ToggleGearTab.cs" company="Zizhen Li">
// Copyright (c) Zizhen Li. All rights reserved.
// Licensed under the GPL-3.0-only license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Linq;
using RimWorld;
using Verse;
using Verse.Sound;

namespace AwesomeInventory.UI
{
    /// <summary>
    /// Toggle Gear Tab.
    /// </summary>
    /// <remarks> Gear_Helmet.png Designed By nickfz from https://pngtree.com/Pngtree.com .</remarks>
    public class ToggleGearTab : Command_Action
    {
        private Type _tabType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleGearTab"/> class.
        /// </summary>
        /// <param name="tabType"> Tab to open when toggle. </param>
        public ToggleGearTab(Type tabType)
        {
            hotKey = KeyBindingDefOf.Misc12;
            action = ToggleTab;
            icon = TexResource.GearHelmet;
            _tabType = tabType;
        }

        /// <summary>
        /// Gets tooltip when hover on the Gizmo.
        /// </summary>
        public override string Desc => string.Concat("Corgi_ToggleGearTab".Translate()
                                                    + "\n"
                                                    + "Hotkey binded to Misc 12");

        private void ToggleTab()
        {
            Type inspectTabType = _tabType;
            MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;

            if (inspectTabType == mainTabWindow_Inspect.OpenTabType)
            {
                mainTabWindow_Inspect.OpenTabType = null;
                SoundDefOf.TabClose.PlayOneShotOnCamera();
            }
            else
            {
                InspectTabBase inspectTabBase = mainTabWindow_Inspect.CurTabs.Where((InspectTabBase t) => inspectTabType.IsAssignableFrom(t.GetType())).FirstOrDefault();
                inspectTabBase.OnOpen();
                mainTabWindow_Inspect.OpenTabType = inspectTabType;
                SoundDefOf.TabOpen.PlayOneShotOnCamera();
            }
        }
    }
}