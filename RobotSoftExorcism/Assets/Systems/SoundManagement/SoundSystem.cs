using SystemBase;
using Systems.SceneManagement.Events;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems.SoundManagement
{
    [GameSystem]
    public class SoundSystem : GameSystem<SoundComponent>
    {
        public override void Register(SoundComponent component)
        {
            MessageBroker.Default.Receive<StartEvent>()
                .Subscribe(e => PlayMusic(component, 0))
                .AddTo(component);
            
            MessageBroker.Default.Receive<BarEnterEvent>()
                .Subscribe(e => CheckBarMusicAlreadyPlaying(component))
                .AddTo(component);
            
            MessageBroker.Default.Receive<CityEnterEvent>()
                .Subscribe(e => PlayMusic(component, 1))
                .AddTo(component);
            
            MessageBroker.Default.Receive<HomeEnterEvent>()
                .Subscribe(e => PlayMusic(component, 2))
                .AddTo(component);
            
            MessageBroker.Default.Receive<PrisonEnterEvent>()
                .Subscribe(e => PlayMusic(component, 3))
                .AddTo(component);
        }

        private void CheckBarMusicAlreadyPlaying(SoundComponent component)
        {
            var audioSource = IoC.Game.GetComponent<AudioSource>();
            if (audioSource.clip.name == "shorterbarnight")
            {
                return;
            }
            PlayMusic(component, 0);
        }

        private void PlayMusic(SoundComponent component, int index)
        {
            Debug.Log("Play music");
            var audioSource = IoC.Game.GetComponent<AudioSource>();
            audioSource.clip = component.MusicClips[index];
            audioSource.Play();
        }
    }
}