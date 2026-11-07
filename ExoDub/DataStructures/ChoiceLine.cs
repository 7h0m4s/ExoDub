using UnityEngine;

namespace ExoDub.DataStructures
{
    public class ChoiceLine
    {
        public Choice Choice { get; set; }
        public string Line { get; set; }
        public string Hash { get; set; }
        public string AudioAssetId { get; set; }


        public AudioClip GetAudioClip()
        {
            if (DubState.AudioClips.TryGetValue(this.AudioAssetId, out var selectedAudioClip))
            {
                return selectedAudioClip;
            }
            else
            {
                return null;
            }
        }
    }
}