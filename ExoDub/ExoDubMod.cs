
using System.Reflection;
using BepInEx;
using HarmonyLib;


namespace ExoDub
{
    [BepInPlugin(ModId, "ExoDub", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private const string ModId = "exodub.exocolonist.exodub";
        private void Awake()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Harmony val = new Harmony(ModId);
            val.PatchAll(executingAssembly);
            var manualLogSource = Logger;
            if (manualLogSource != null)
                manualLogSource.LogInfo((object)"Plugin ExoDub is loaded!");
        }
    }

}