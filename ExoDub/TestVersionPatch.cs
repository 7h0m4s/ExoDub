using HarmonyLib;

namespace ExoDub
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.GetVersionText))]
    public class TestVersionPatch
    {
        [HarmonyPostfix]
        public static void GetVersionText(MainMenu __instance,  ref string __result)
        {
            __result = "ExoDub Forever! : " + __result;
        }
    }
}