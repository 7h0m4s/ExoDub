using System;
using System.Collections;
using System.Media;
using DarkTonic.MasterAudio;
using HarmonyLib;
using ExoDub.Utility;
using UnityEngine;

namespace ExoDub.Patches
{
    [HarmonyPatch(typeof(ResultsTextPanel), nameof(ResultsTextPanel.SetContent))]
    public static class ResultsTextPanelPlayAudioPatch
    {
        // [HarmonyPrefix]
        // public static bool Prefix(ResultsTextPanel __instance)
        // {
        //     return true;
        // }

        [HarmonyPostfix]
        public static void Postfix(ref ResultsTextPanel __instance)
        {
            
            if(DubState.CurrentAudioCoroutine is null == false) ExoDubMod.Instance.StopCoroutine(DubState.CurrentAudioCoroutine);
            DubState.AudioSource.Stop();
            ExoDubMod.Instance.StartCoroutine(Coroutine_PlayChoiceLines(__instance));
            
            return;
        }

        static IEnumerator Coroutine_PlayChoiceLines(ResultsTextPanel __instance)
        {
                yield return null;
                var displayedText = ((NWText)__instance.text).text;
                var currentResult = Princess.result;
                var currentChoice = currentResult.choice;
                ExoDubMod.ExoDubLogger.LogInfo((object)$"Running Coroutine for {currentResult.choice.choiceID}!");
                var currentResultText = currentResult.choice.resultText;
                var orderedChoiceLines = currentResult.choice.GetOrderedChoiceLinesForDisplayedText(displayedText);
                yield return null;
                foreach (var choiceLine in orderedChoiceLines)
                {
                    DubAudioEngine.LoadAudioFile(choiceLine.AudioAssetId);
                    yield return null;
                }
                
                for (int i = 0; i < orderedChoiceLines.Length; i++)
                {
                    var choiceLine =  orderedChoiceLines[i];
                    var audioClip = choiceLine.GetAudioClip();
                    if (audioClip is null) continue;
                    if (!audioClip) continue;
                    
                    DubState.AudioSource.PlayOneShot(audioClip);
                    if (i != orderedChoiceLines.Length - 1)
                    {
                        yield return new WaitForSecondsRealtime(1f);
                    }
                    else
                    {
                        yield return null;
                    }
            }
        }
    }
}