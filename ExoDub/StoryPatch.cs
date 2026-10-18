
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace ExoDub
{

    [HarmonyPatch(typeof(Story), nameof(Story.Execute))]
    public class StoryPatch
    {
        [HarmonyPostfix]
        public static void Execute(Story __instance)
        {
            var temp = 1;
            var manualLogSource = ExoDubMod.ExoDubLogger;
            if (manualLogSource != null)
                manualLogSource.LogInfo((object)"Stories Validated!");
        }
    }
}