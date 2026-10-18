using System.Collections.Generic;
using HarmonyLib;

namespace ExoDub
{

    [HarmonyPatch(typeof(ParserStory), nameof(ParserStory.LoadStoriesFile))]
    public class ParserStoryPatch
    {
        [HarmonyPrefix]
        public static bool LoadAllDataPrefix()
        {
            
                
            return true;
        }
        
        [HarmonyPostfix]
        public static void LoadAllDataPostfix(List<StoryTemplate>__result)
        {
            var temp = 1;
            return;
        }
    }
}