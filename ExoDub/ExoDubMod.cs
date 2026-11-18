using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
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
        
        public ConfigEntry<bool> enabled;
        public ConfigEntry<float> volume;
        public ConfigEntry<string> selectedVoicePack;
        public ConfigEntry<float> delayBetweenLines;
        public ConfigEntry<bool> generateCsvExport;
        
        private void Awake()
        {
            enabled = Config.Bind(
                "General",              // Section
                "Enable",        // Key
                true,                   // Default value
                "Turns the Dubbing playback on or off. Default is true"); // Description
            volume = Config.Bind(
                "General",
                "DubbingVolume",
                0.75f,
                "Controls volume of dubbing playback. Default is 0.75. Min is 0. Max is 1."); 
            selectedVoicePack = Config.Bind(
                "General",
                "SelectedVoicePack",
                DubState.Constants.DefaultVoicePack,
                "Select which directory inside 'VoicePackLibrary' contains the voice pack audio files you want to use.  Default is 'Default'"); 
            delayBetweenLines = Config.Bind(
                "General",
                "DelayBetweenLines",
                0.3f,
                "Seconds between each voiceline in the same textbox is played. Default is 0.3. Minimum of 0");
            generateCsvExport = Config.Bind(
                "General",              
                "GenerateCsvExport",        
                false,                   
                "Create csv export on opening Exocolonist of all in-game text for creating new voice packs. Default is false"); 

            volume.BoxedValue = volume.Value.Clamp(0f,1f);
            
            Config.Save();
            
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
            
            // Load all file system data once at the start
            DubAudioEngine.LoadFileList();
            DubAudioEngine.LoadVoicePackDirectoryList();
            if (DubState.VoicePackDirectoryList.All(directoryName => directoryName != selectedVoicePack.Value))
            {
                // Reset to Default Voice Pack if selected pack doesn't exist
                selectedVoicePack.BoxedValue = DubState.Constants.DefaultVoicePack;
            }
            
            
            
            
        }
    }

}