using System.Collections.Generic;
using BepInEx;
using UnityEngine;

namespace ExoDub
{
    public static class DubState
    {
        public static class Constants
        {
            public const string ExoDubDirectory = "ExoDub";
            public const string AudioAssetDirectory = "AudioAssets";
        }

        public static PluginInfo PluginInfo = new PluginInfo();
        
        public static Dictionary<string,AudioClip> AudioClips = new Dictionary<string, AudioClip>();
        
        public static GameObject UnityObject = new GameObject();
        
        public static AudioSource AudioSource;
    }
}