using System;
using System.Media;
using DarkTonic.MasterAudio;
using HarmonyLib;
using UnityEngine;

namespace ExoDub.Patches
{

    
    [HarmonyPatch(typeof(ResultsTextPanel), nameof(ResultsTextPanel.SetContent))]
    public static class ResultsTextPanelPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ResultsTextPanel __instance)
        {
            //SoundManager.PlaySound(SoundType.ui_challenge_card_draw);

            return true;
        }
        
        [HarmonyPostfix]
        public static void Postfix(ResultsTextPanel __instance)
        {
            //SoundManager.PlaySound(SoundType.ui_card_forget);

            //var playSoundResult = MasterAudio.PlaySound("123");
            try
            {
                var result = DubState.AudioClips.TryGetValue("123", out var selectedClip);
                // if (DubState.AudioSource == null) DubState.AudioSource = new AudioSource();
                // DubState.AudioSource.PlayOneShot(selectedClip); 
                DubState.AudioSource.PlayOneShot(selectedClip);
            }
            catch (Exception e)
            {
                ExoDubMod.ExoDubLogger.LogError(e);
                Console.WriteLine(e);
                throw;
            }
            
            
            
            return;
        }
    }
}