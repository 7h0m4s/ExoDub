using System;
using System.IO;
using System.Linq;
using System.Threading;
using BepInEx;
using DarkTonic.MasterAudio;
using Lib;
using UnityEngine;
using UnityEngine.Networking;

namespace ExoDub
{
    public class DubAudioEngine
    {

        // TODO make asynchronous
        // TODO by turning into a coroutine
        public static void LoadAudioFiles()
        {
            
            // string currentDirectory = DubState.PluginInfo.Location;
            string currentDirectory = Paths.PluginPath;
            string audioAssetFilepath = Path.Combine(Paths.PluginPath, DubState.Constants.ExoDubDirectory, DubState.Constants.AudioAssetDirectory);
            var fileList = Directory.EnumerateFiles(audioAssetFilepath);

            foreach (var filePath in fileList.Where(a=> a.EndsWith(".ogg")))
            {
                try
                {
                    var fileUri = "file:///" + filePath.Replace("\\", "/");
                    UnityWebRequestAsyncOperation requestResult;
                    using (var req = UnityWebRequestMultimedia.GetAudioClip(fileUri, AudioType.OGGVORBIS))
                    {
                        requestResult = req.SendWebRequest();
                        while (!requestResult.isDone)
                        {
                            Thread.Sleep(1);
                        }
                        if (!string.IsNullOrWhiteSpace(requestResult.webRequest.error))
                        {
                            ExoDubMod.ExoDubLogger.LogError(requestResult.webRequest.error);
                            continue;
                        }
                    
                        var audioClip = DownloadHandlerAudioClip.GetContent(requestResult.webRequest);
                        audioClip.name = Path.GetFileNameWithoutExtension(filePath);
                        DubState.AudioClips.AddSafe(Path.GetFileNameWithoutExtension(filePath),audioClip);
                        // var forceInstance = MasterAudio.Instance;
                        // var newGroup = MasterAudio.GrabGroup(audioClip.name);
                        // var firstGroupVariation = newGroup.groupVariations.First();
                        // firstGroupVariation.audLocation = MasterAudio.AudioLocation.Clip;
                        // firstGroupVariation.VarAudio.clip = audioClip;
                    }

                    
                }
                catch (Exception e)
                {
                    ExoDubMod.ExoDubLogger.LogError(e);
                    Console.WriteLine(e);
                    throw;
                }

            }
            return;
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