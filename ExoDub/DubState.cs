using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using UnityEngine;

namespace ExoDub
{
    public static class DubState
    {
        public static class Constants
        {
            public const string ExoDubDirectory = "ExoDub";
            public const string VoicePackLibraryDirectory = "VoicePackLibrary";
            public const string DefaultVoicePack = "DefaultVoicePack";
            public const int ChoiceAudioAssetLoadingDepth = 3;
        }

        public static float DelayBetweenPlayingLines = 0.3f;
        
        public static PluginInfo PluginInfo = new PluginInfo();
        
        public static Dictionary<string,AudioClip> AudioClips = new Dictionary<string, AudioClip>();
        
        public static GameObject UnityObject = new GameObject();
        
        public static AudioSource AudioSource;

        public static Dictionary<string,String[]> CurrentFileList;
        
        public static List<string> VoicePackDirectoryList;
        
        public static Coroutine CurrentAudioCoroutine;
    }
}