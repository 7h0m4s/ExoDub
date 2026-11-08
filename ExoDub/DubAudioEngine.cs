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
        
        
        public static string SafeURIForUnityWWWFromPath(string rawPath)
        {
            string text = rawPath;
            for (int i = 0; i < rawPath.Length; i++)
            {
                text = text.Replace(FilePathRaw[i], FilePathSafe[i]);
            }
            return "file://" + text;
        }
        private static readonly string[] FilePathRaw = new string[73]
        {
            "Ž", "ž", "Ÿ", "¡", "¢", "£", "¤", "¥", "¦", "§",
            "\u00a8", "©", "ª", "À", "Á", "Â", "Ã", "Ä", "Å", "Æ",
            "Ç", "È", "É", "Ê", "Ë", "Ì", "Í", "Î", "Ï", "Ð",
            "Ñ", "Ò", "Ó", "Ô", "Õ", "Ö", "Ù", "Ú", "Û", "Ü",
            "Ý", "Þ", "ß", "à", "á", "â", "ã", "ä", "å", "æ",
            "ç", "è", "é", "ê", "ë", "ì", "í", "î", "ï", "ð",
            "ñ", "ò", "ó", "ô", "õ", "ö", "ù", "ú", "û", "ü",
            "ý", "þ", "ÿ"
        };

        private static readonly string[] FilePathSafe = new string[73]
        {
            "%8E", "%9E", "%9F", "%A1", "%A2", "%A3", "%A4", "%A5", "%A6", "%A7",
            "%A8", "%A9", "%AA", "%C0", "%C1", "%C2", "%C3", "%C4", "%C5", "%C6",
            "%C7", "%C8", "%C9", "%CA", "%CB", "%CC", "%CD", "%CE", "%CF", "%D0",
            "%D1", "%D2", "%D3", "%D4", "%D5", "%D6", "%D9", "%DA", "%DB", "%DC",
            "%DD", "%DE", "%DF", "%E0", "%E1", "%E2", "%E3", "%E4", "%E5", "%E6",
            "%E7", "%E8", "%E9", "%EA", "%EB", "%EC", "%ED", "%EE", "%EF", "%F0",
            "%F1", "%F2", "%F3", "%F4", "%F5", "%F6", "%F9", "%FA", "%FB", "%FC",
            "%FD", "%FE", "%FF"
        };
        
    }
}