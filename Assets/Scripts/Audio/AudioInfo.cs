using System.Collections.Generic;

public class AudioInfo : Singleton<AudioInfo>
{
    public enum AudioType
    {
        Null,

        Bgm,

        Star1,
        Star2,
        Star3,

        Fail,

        UiTap,

        Count,
    }

    private static Dictionary<AudioType, string> keyValuePairs = new Dictionary<AudioType, string> ()
    {
        { AudioType.Bgm, "bgm" },

        { AudioType.Star1, "star1" },
        { AudioType.Star2, "star2" },
        { AudioType.Star3, "star3" },

        { AudioType.Fail, "die" },

        { AudioType.UiTap, "tap" },
    };

    public static string GetAudioName (AudioType audioType)
    {
        if (keyValuePairs.ContainsKey (audioType))
        {
            return keyValuePairs[audioType];
        }

        return string.Empty;
    }
}