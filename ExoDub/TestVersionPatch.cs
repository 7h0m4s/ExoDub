using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using DarkTonic.MasterAudio;
using HarmonyLib;
using UnityEngine;

namespace ExoDub
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.GetVersionText))]
    public class TestVersionPatch
    {
        [HarmonyPostfix]
        public static void GetVersionText(MainMenu __instance,  ref string __result)
        {
            __result = "ExoDub Forever! : " + __result;

           
            
            
            
            
            
            // var allResultText = Story.allStories.SelectMany(a => a.allChoices)
            //     .Where(a => !String.IsNullOrWhiteSpace(a.resultText))
            //     .Select(a =>
            //         Regex.Split(a.resultText, @"(?!\[[^\]]*Nick[^\]]*\])(?!\[[^\]]*Name[^\]]*\])\[[^\]]*\]", RegexOptions.IgnoreCase)
            //             .Where(b=> !string.IsNullOrWhiteSpace(b))
            //             .Select(c=> (Choice: a, Line:c)))
            //     .SelectMany(a=> a);
            //
            // var allResultsFormatted = allResultText.Select(a=>new
            // {
            //     StoryID = a.Choice.story.storyID,
            //     ChoiceID = a.Choice.choiceID,
            //     HashCode = a.Line.GetHashCode(),
            //     Filename = $"{a.Line.GetHashCode()}.wav",
            //     Character = a.Choice.story?.chara?.nickname ?? "",
            //     Characters = a.Choice.story?.charas.Join(),
            //     ButtonText = a.Choice.buttonText,
            //     Sets = a.Choice.sets.Join(),
            //     LineToDub = a.Line,
            //     Requirements = a.Choice.requirements.Join(),
            //     Path = a.Choice.path,
            //     Level = a.Choice.level,
            //     IsDream = a.Choice.isDream,
            //     IsFlirt = a.Choice.isFlirt,
            //     IsGroundhog = a.Choice.isGroundhog,
            //     HasBattle = a.Choice.hasBattle,
            // });
            //
            // using (var writer = new StreamWriter("AllLinesDump.csv"))
            // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            // {
            //     csv.WriteRecords(allResultsFormatted);
            // }
            //
            // // ResultsMenu;
            // // ResultsTextPanel
            //
            // var allstories = Story.allStories;
            
        }
    }
}