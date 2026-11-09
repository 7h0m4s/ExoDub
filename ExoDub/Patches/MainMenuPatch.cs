using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using ExoDub.Utility;
using HarmonyLib;

namespace ExoDub.Patches
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.GetVersionText))]
    public class MainMenuPatch
    {
        [HarmonyPostfix]
        public static void GetVersionText(MainMenu __instance,  ref string __result)
        {
            __result = "ExoDub Is Active! : " + __result;

            // var allRawResultTexts = Story.allStories.SelectMany(a => a.allChoices)
            //     .Where(a => !String.IsNullOrWhiteSpace(a.resultText))
            //     .Select(a => (Choice: a, RawResultText: a.resultText));
            // var totalIfStatementsInRawResultTexts =
            //     allRawResultTexts.Where(a => a.RawResultText.ToLower().Contains("[if"));
            // var allRawResultTextsFormatted = allRawResultTexts.Select(a=>new
            // {
            //     raw_result_text = a.RawResultText,
            //     story_id = a.Choice.story.storyID,
            //     choice_id = a.Choice.choiceID,
            //     hashcode = a.RawResultText.GetHashCode(),
            //     filename = $"{a.RawResultText.GetHashCode()}.wav",
            //     character = a.Choice.story?.chara?.nickname ?? "",
            //     characters = a.Choice.story?.charas.Join(),
            //     button_text = a.Choice.buttonText,
            //     sets = a.Choice.sets.Join(),
            //     requirements = a.Choice.requirements.Join(),
            //     path = a.Choice.path,
            //     level = a.Choice.level,
            //     is_dream = a.Choice.isDream,
            //     is_flirt = a.Choice.isFlirt,
            //     is_groundhog = a.Choice.isGroundhog,
            //     has_battle = a.Choice.hasBattle,
            //     previous_line = a.Choice.parent != null ? a.Choice.parent.resultText : String.Empty,
            // });
            //
            // using (var writer = new StreamWriter("AllRawResultTextDump.csv"))
            // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            // {
            //     csv.WriteRecords(allRawResultTextsFormatted);
            // }

            if (ExoDubMod.Instance.generateCsvExport.Value)
            {
                var allLines = Story.allStories.SelectMany(a => a.allChoices)
                    .Where(a => !String.IsNullOrWhiteSpace(a.resultText))
                    .Select(a => a.GetAllChoiceResultLines())
                    .SelectMany(a => a);

                var allLinesFormatted = allLines.Select(a => new
                {
                    story_id = a.Choice.story.storyID,
                    choice_id = a.Choice.choiceID,
                    hashcode = a.Hash,
                    filename = $"{a.Line.GetHashCode()}.wav",
                    character = a.Choice.story?.chara?.nickname ?? "",
                    characters = a.Choice.story?.charas.Join(),
                    button_text = a.Choice.buttonText,
                    sets = a.Choice.sets.Join(),

                    line_to_dub = a.Line,
                    requirements = a.Choice.requirements.Join(),
                    path = a.Choice.path,
                    level = a.Choice.level,
                    is_dream = a.Choice.isDream,
                    is_flirt = a.Choice.isFlirt,
                    is_groundhog = a.Choice.isGroundhog,
                    has_battle = a.Choice.hasBattle,
                    previous_line = a.Choice.parent != null ? a.Choice.parent.resultText : String.Empty,
                });

                using (var writer = new StreamWriter("AllLinesExport.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(allLinesFormatted);
                }
            }
        }
    }
}