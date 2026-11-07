using System;
using System.Collections;
using HarmonyLib;

namespace ExoDub.Patches
{
    
    
    // [HarmonyPatch(typeof(ResultsTextPanel), "Awake")]
    // public static class ResultsTextPanelLoadAudioPatch
    // {
    //     [HarmonyPostfix]
    //     public static void Postfix(ref ResultsTextPanel __instance)
    //     {
    //             var currentResultText = ((NWText)__instance.text).text;
    //
    //         return;
    //     }
    //
    //     static IEnumerator LoadAudio(ResultsTextPanel __instance)
    //     {
    //         var currentResultText = ((NWText)__instance.text).text;
    //         var result = Princess.result;
    //         result.choice.resultText.GetHashCode();
    //         //result.choice.choices
    //         //DubState.Constants.ChoiceAudioAssetLoadingDepth;
    //             
    //         // Skip first frame just in case
    //         yield return null;
    //         
    //         
    //         
    //
    //
    //     }
    // }
}