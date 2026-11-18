using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BepInEx;
using UnityEngine;
using UnityEngine.Networking;

namespace ExoDub
{
    public static class DubAudioEngine
    {
        public static string GetPathToVoicePackLibrary => Path.Combine(Paths.PluginPath, DubState.Constants.ExoDubDirectory, DubState.Constants.VoicePackLibraryDirectory);
        public static string GetPathToSelectedVoicePackAudioAssets => Path.Combine(Paths.PluginPath, DubState.Constants.ExoDubDirectory, DubState.Constants.VoicePackLibraryDirectory, ExoDubMod.Instance.selectedVoicePack.Value);

        private static readonly Dictionary<string, int> AudioFileExtensionPriority = new Dictionary<string, int>
        {
            //{".flac", 0},
            {".ogg", 1},
            {".wav", 2},
            {".mp3", 3},
            {".aif", 4},
            {".aiff", 5},
            {".aifc", 6},
            
        };
        
        public static void LoadVoicePackDirectoryList()
        {
            try
            {
                DubState.VoicePackDirectoryList = Directory.EnumerateDirectories(GetPathToVoicePackLibrary).Select(Path.GetFileName).ToList();
                if (DubState.VoicePackDirectoryList.Count == 0)
                {
                    ExoDubMod.ExoDubLogger.LogWarning($"No Voice Packs found at '{GetPathToVoicePackLibrary}'");
                }
            }
            catch (Exception e)
            {
                ExoDubMod.ExoDubLogger.LogError($"Error searching for voice packs at '{GetPathToVoicePackLibrary}': {e}");
            }
            
        }
        
        public static void LoadFileList()
        {
            try
            {
                var fileList = Directory.EnumerateFiles(GetPathToSelectedVoicePackAudioAssets).ToList();
                if (fileList.Count == 0)
                {
                    ExoDubMod.ExoDubLogger.LogWarning($"No files found at {GetPathToSelectedVoicePackAudioAssets}");
                }
            
                DubState.CurrentFileList = fileList.Where(a =>
                        Path.HasExtension(a) && AudioFileExtensionPriority.ContainsKey(Path.GetExtension(a).ToLower())
                    )
                    .GroupBy(Path.GetFileNameWithoutExtension)
                    .ToDictionary(a =>
                            a.Key,
                        a => a.OrderBy(b =>
                                AudioFileExtensionPriority.TryGetValue(Path.GetExtension(b),
                                    out int order)
                                    ? order
                                    : int.MaxValue)
                            .ToArray());
            
                if (DubState.CurrentFileList.Count == 0)
                {
                    ExoDubMod.ExoDubLogger.LogWarning($"No valid audio files found at '{GetPathToSelectedVoicePackAudioAssets}'. Only '{String.Join("-", AudioFileExtensionPriority.Keys)}' formats supported.");
                }
            }
            catch (Exception e)
            {
                ExoDubMod.ExoDubLogger.LogError($"Error finding voice pack audio files at '{GetPathToSelectedVoicePackAudioAssets}': {e}");
            }
           
        }
        
        public static AudioClip LoadAudioFile(string assetId)
        {
            try
            {
                if (!DubState.CurrentFileList.TryGetValue(assetId, out string[] fileList)) return null;
                if (fileList.Length <= 0) return null;
                string filePath = fileList[0];
                
                var fileUri = "file:///" + filePath.Replace("\\", "/");
                using (var req = UnityWebRequestMultimedia.GetAudioClip(fileUri, GetAudioTypeFromFilepath(filePath)))
                {
                    var requestResult = req.SendWebRequest();
                    while (!requestResult.isDone)
                    {
                        Thread.Sleep(1);
                    }
                    if (!string.IsNullOrWhiteSpace(requestResult.webRequest.error))
                    {
                        ExoDubMod.ExoDubLogger.LogError(requestResult.webRequest.error);
                    }
                
                    var audioClip = DownloadHandlerAudioClip.GetContent(requestResult.webRequest);
                    audioClip.name = assetId;
                    //DubState.AudioClips.AddSafe(assetId,audioClip);
                    return audioClip;
                }
            }
            catch (Exception e)
            {
                ExoDubMod.ExoDubLogger.LogError($"Error loading audio file '{assetId}': {e}");
            }
            return null;
        }

        private static AudioType GetAudioTypeFromFilepath(string filepath)
        {
            var fileExtension = Path.GetExtension(filepath);
            switch (fileExtension)
            {
                case ".wav":
                    return AudioType.WAV;
                case ".mp3":
                    return AudioType.MPEG;
                case ".ogg":
                    return AudioType.OGGVORBIS;
                case ".aif":
                case ".aiff":
                case ".aifc":
                    return AudioType.AIFF;
                default:
                    throw new NotImplementedException();
            }        
        }
    }
}