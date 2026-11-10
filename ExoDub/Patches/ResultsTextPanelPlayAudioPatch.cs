using System;
using System.Collections;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using DarkTonic.MasterAudio;
using HarmonyLib;
using ExoDub.Utility;
using UnityEngine;

namespace ExoDub.Patches
{
    [HarmonyPatch]
    public static class ResultsTextPanelPlayAudioPatch
    {
        // [HarmonyPrefix]
        // public static bool Prefix(ResultsTextPanel __instance)
        // {
        //     return true;
        // }

        [HarmonyPatch(typeof(ResultsTextPanel), nameof(ResultsTextPanel.SetContent))]
        [HarmonyPostfix]
        public static void Postfix(ref ResultsTextPanel __instance)
        {
            // Skip if mod is disabled
            if (ExoDubMod.Instance.enabled.Value == false) return;
            DubState.AudioSource.volume = Settings.instance.volumeMaster * ExoDubMod.Instance.volume.Value;
            if (DubState.CurrentAudioCoroutine is null == false)
                ExoDubMod.Instance.StopCoroutine(DubState.CurrentAudioCoroutine);
            DubState.AudioSource.Stop();
            DubState.CurrentAudioCoroutine = ExoDubMod.Instance.StartCoroutine(Coroutines.Coroutine_PlayChoiceLines(__instance));
        }

        
        [HarmonyPatch(typeof(ResultsTextPanel), nameof(ResultsTextPanel.OnRecycle))]
        [HarmonyPrefix]
        public static bool Prefix(ref ResultsTextPanel __instance)
        {
            // Skip if mod is disabled
            if (ExoDubMod.Instance.enabled.Value == false) return true;
            
            // Stop current audio
            if (DubState.CurrentAudioCoroutine is null == false)
                ExoDubMod.Instance.StopCoroutine(DubState.CurrentAudioCoroutine);
            DubState.AudioSource.Stop();
            return true;
        }
    }
}