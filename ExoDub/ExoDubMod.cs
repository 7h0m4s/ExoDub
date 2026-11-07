
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;


namespace ExoDub
{
    [BepInPlugin(ModId, "ExoDub", "1.0.0")]
    public class ExoDubMod : BaseUnityPlugin
    {
        private const string ModId = "exodub.exocolonist.exodub";
        public static ManualLogSource ExoDubLogger;
        public static ExoDubMod Instance { get; private set; }
        
        
        private void Awake()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Harmony val = new Harmony(ModId);
            val.PatchAll(executingAssembly);
            ExoDubLogger = Logger;
            if (ExoDubLogger != null)
                ExoDubLogger.LogInfo((object)"Plugin ExoDub is loaded!");

            Instance = this;
            DubState.UnityObject = new GameObject();
            Object.DontDestroyOnLoad(DubState.UnityObject);
            DubState.AudioSource = DubState.UnityObject.AddComponent<AudioSource>();
            DubState.AudioSource.loop = false;
            
            // We don't want to load all 15000 audio files into memory
            //DubAudioEngine.LoadAllAudioFiles();
            DubAudioEngine.LoadFileList();
        }
    }

}