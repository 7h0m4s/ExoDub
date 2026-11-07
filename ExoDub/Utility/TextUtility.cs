using System;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using ExoDub.DataStructures;

namespace ExoDub.Utility
{
    public static class TextUtility
    {
        private static readonly Regex ResultTextSplitRegex =
            new Regex(@"(?!\[[^\]]*Nick[^\]]*\])(?!\[[^\]]*Name[^\]]*\])\[[^\]]*\]", RegexOptions.IgnoreCase);
        
        
        public static ChoiceLine[] GetAllChoiceResultLines(this Choice choice)
        {
           return Utility.TextUtility.ResultTextSplitRegex.Split(choice.resultText)
                .Where(b=> !string.IsNullOrWhiteSpace(b))
                .Select(c=> new ChoiceLine
                {
                    Choice = choice,
                    Line = c,
                    Hash = c.GetHashCode().ToString(),
                    AudioAssetId = $"{choice.story.storyID}_{c.GetHashCode()}"
                })
                .ToArray();
        }


        public static ChoiceLine[] GetOrderedChoiceLinesForDisplayedText(this Choice choice, string displayedText)
        {
            return choice.GetAllChoiceResultLines()
                .Select(a => (Index: displayedText.Trim().IndexOf(TextFilter.FilterResultText(a.Line.Trim(), Princess.result, Princess.result.story) , StringComparison.InvariantCultureIgnoreCase), ChoiceLine: a))
                .Where(a => a.Index >= 0)
                .OrderBy(a => a.Index)
                .Select(a=> a.ChoiceLine)
                .ToArray();
        }
    }
}