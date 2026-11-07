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
            public const string DubCollectionLibraryDirectory = "DubLibrary";
            public const int ChoiceAudioAssetLoadingDepth = 3;
        }

        public static float DelayBetweenPlayingLines = 1f;
        
        public static string CurrentDubCollectionDirectory = "Default";
        
        public static PluginInfo PluginInfo = new PluginInfo();
        
        public static Dictionary<string,AudioClip> AudioClips = new Dictionary<string, AudioClip>();
        
        public static GameObject UnityObject = new GameObject();
        
        public static AudioSource AudioSource;

        public static Dictionary<string,String[]> CurrentFileList;
        
        public static Coroutine CurrentAudioCoroutine;
    }
}