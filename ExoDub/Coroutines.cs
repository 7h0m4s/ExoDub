using System.Collections;
using System.Collections.Generic;
using ExoDub.Utility;
using UnityEngine;

namespace ExoDub
{
    public static class Coroutines
    {
        public static IEnumerator Coroutine_PlayChoiceLines(ResultsTextPanel __instance)
        {
            yield return null;
            var displayedText = ((NWText)__instance.text).text;
            var currentResult = Princess.result;
            var currentChoice = currentResult.choice;
            ExoDubMod.ExoDubLogger.LogInfo((object)$"============================================");
            ExoDubMod.ExoDubLogger.LogInfo((object)$"Running Coroutine for StoryId:{currentResult.choice.story.storyID} ChoiceId:{currentResult.choice.choiceID}");
            var currentResultText = currentResult.choice.resultText;
            var orderedChoiceLines = currentResult.choice.GetOrderedChoiceLinesForDisplayedText(displayedText);
            if (orderedChoiceLines.Length == 0)
            {
                ExoDubMod.ExoDubLogger.LogInfo((object)$"[Warning] No ChoiceLines found for StoryId:{currentResult.choice.story.storyID} ChoiceId:{currentResult.choice.choiceID}");
                ExoDubMod.ExoDubLogger.LogInfo((object)$"[Warning] Result Text Panel's DisplayText: {displayedText.Replace(@"\n",@"\\n")}");
                ExoDubMod.ExoDubLogger.LogInfo((object)$"[Warning] Current Choice's ResultText: {currentResultText.Replace(@"\n",@"\\n")}");
                ExoDubMod.ExoDubLogger.LogInfo((object)$"============================================");
            }
            if (orderedChoiceLines.Length >= 2)
            {
                ExoDubMod.ExoDubLogger.LogInfo((object)$"[Info] {orderedChoiceLines.Length } ChoiceLines found for StoryId:{currentResult.choice.story.storyID} ChoiceId:{currentResult.choice.choiceID}");
                ExoDubMod.ExoDubLogger.LogInfo((object)$"[Info] Result Text Panel's DisplayText: {displayedText.Replace(@"\n",@"\\n")}");
                ExoDubMod.ExoDubLogger.LogInfo((object)$"[Info] Current Choice's ResultText: {currentResultText.Replace(@"\n",@"\\n")}");
                ExoDubMod.ExoDubLogger.LogInfo((object)$"============================================");
            }
            yield return null;
            var audioClipsInOrder = new List<AudioClip>();
            foreach (var choiceLine in orderedChoiceLines)
            {
                var audioClip = DubAudioEngine.LoadAudioFile(choiceLine.AudioAssetId);
                if (audioClip is null == false)
                {
                    audioClipsInOrder.Add(audioClip);
                };
                yield return null;
            }
            
            for (int i = 0; i < audioClipsInOrder.Count; i++)
            {
                var audioClip = audioClipsInOrder[i];
                //DubState.AudioSource.PlayOneShot(audioClip);
                DubState.AudioSource.clip = audioClip;
                DubState.AudioSource.Play();
                yield return new WaitForSeconds(audioClip.length + DubState.DelayBetweenPlayingLines);
            }
        }
    }
}