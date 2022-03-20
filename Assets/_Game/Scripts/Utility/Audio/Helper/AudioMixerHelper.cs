using UnityEngine;

namespace Utility.Audio.Helper
{
    public enum AudioMixerEnum
    {
        Master,
        Music,
        Sfx,
        Ambience,
        Dialogue
    }

    public static class AudioMixerHelper
    {
        // Strings to access the Audio Mixer's Exposed Parameters
        public static string Master = "MasterVolume";
        public static string Music = "MusicVolume";
        public static string Sfx = "SfxVolume";
        public static string Ambience = "AmbienceVolume";
        public static string Dialogue = "DialogueVolume";

        public static string GetAudioMixerParameter(AudioMixerEnum mixerGroup) {
            switch (mixerGroup) {
                case AudioMixerEnum.Master:
                    return Master;
                case AudioMixerEnum.Music:
                    return Music;
                case AudioMixerEnum.Sfx:
                    return Sfx;
                case AudioMixerEnum.Ambience:
                    return Ambience;
                case AudioMixerEnum.Dialogue:
                    return Dialogue;
                default:
                    Debug.LogError("Invalid Audio Mixer Enum");
                    return null;
            }
        }
    }
}