using StrongSystems.Audio.Actions;
using UniRx;
using UnityEngine;

namespace StrongSystems.Audio
{
    public static class AudioSystemExtensions
    {
        public static void Play(this string soundName, string tag = null)
        {
            MessageBroker.Default.Publish(new AudioActSFXPlay { Name = soundName, Tag = tag });
        }
        
        public static void PlayRandom(this string[] soundArray, string tag = null)
        {
            MessageBroker.Default.Publish(new AudioActSFXPlay { Name = soundArray[Random.Range(0, soundArray.Length - 1)], Tag = tag });
        }
    }
}