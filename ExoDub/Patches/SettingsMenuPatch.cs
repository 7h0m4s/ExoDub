using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using ExoDub.Utility;
using HarmonyLib;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ExoDub.Patches
{
    [HarmonyPatch(typeof(SettingsMenu), nameof(SettingsMenu.UpdateButtons))]
    public static class SettingsMenuPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref SettingsMenu __instance)
        {
            // Skip if not the Audio Settings screen
            var currentGroup = Traverse.Create(__instance).Field("currentGroup").GetValue<SettingsGroup>();
            if (currentGroup != SettingsGroup.audio) return;
            
            Selectable aboveButton = Traverse.Create(__instance).Field("aboveButton").GetValue<Selectable>();
            aboveButton = SettingsUtility.CreateButton(ref __instance, ExoDubMod.Instance.enabled, aboveButton,
                "ExoDub [ON]", "ExoDub [OFF]", currentGroup);
            if (ExoDubMod.Instance.enabled.Value)
            {
                aboveButton = SettingsUtility.CreateVoicePackSelectDropdown(ref __instance, "Selected Voice Pack", ExoDubMod.Instance.selectedVoicePack, aboveButton);
                aboveButton = SettingsUtility.CreateSlider(ref __instance, "Dubbing Volume", ExoDubMod.Instance.volume, aboveButton);
                aboveButton = SettingsUtility.CreateSlider(ref __instance, "Delay Between Voice Lines", ExoDubMod.Instance.delayBetweenLines, aboveButton);
                aboveButton = SettingsUtility.CreateButton(ref __instance,  ExoDubMod.Instance.generateCsvExport, aboveButton,"Csv Export On Start [ON]", "Csv Export On Start [OFF]", currentGroup);
            }
        }
    }
}