using System;
using System.Linq;
using StrongSystems.Audio;
using SystemBase;
using Systems.Player;
using Systems.Player.Events;
using Systems.SceneManagement.Events;
using UniRx;
using UnityEngine;
using Utils;
using Utils.DotNet;
using Random = UnityEngine.Random;

namespace Systems.SoundManagement
{
    [GameSystem]
    public class SoundSystem : GameSystem<SoundComponent>
    {
        public static readonly string[] RodPuke = new[] { "puke_1", "puke_2", "puke_3", "puke_4", "puke_5" };
        public static readonly string[] RodKick = new[] { /*"kicked_1",*/ "kicked_2", /*"kicked_3",*/ "kicked_4", "kicked_5", /*"kicked_6"*/ };
        public static readonly string[] RodPolice = new[] { "police_1", "police_2" };
        public static readonly string[] RodDrink = new[] { "drink_1", "drink_2" };
        public static readonly string[] RodFalling = new[] { "fall_1", "fall_2", "fall_3" };
        public static readonly string[] RodPoebel = new[] { "poeble_1","poeble_2","poeble_3","poeble_4","poeble_5","poeble_6" };
        public static readonly string[] Police = new[] { "police_3", "police_4" };
        
        private string[] PukeReaction = new []{ "did_he_female", "gross_female", "shame_female"};
        private string[] KickReaction = new[] { "looks_unstable_male", "police_female", "shame_female", "stop_him_female", "stop_male"};
        private string[] PoebelReaction = new[] { "get_lost_male", "calm_down_male", "not_rioting_male", "police_female", "stop_him_female", "stop_male" };
        private string[] GeneralReaction = new[] {"dog_1", "dog_2", "dog_3", "hammered_male", "is_he_drunk_male", "looks_drunk_female", "same_male", "well_oiled_male", "why_drunk_male"};
        public override void Register(SoundComponent component)
        {
            //Music
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
            
            //Sounds
            // MessageBroker.Default.Receive<PlayerStateChangeEvent>()
            //     .Subscribe(e => PlayRodriguesSound(e.PlayerState))
            //     .AddTo(component);
            
            MessageBroker.Default.Receive<PlayerCaughtByPoliceEvent>()
            .Subscribe(e => RodPolice.PlayRandom())
            .AddTo(component);
            
            MessageBroker.Default.Receive<PlayerDrinkEvent>()
                .Subscribe(e => RodDrink.PlayRandom())
                .AddTo(component);
            
            MessageBroker.Default.Receive<PlayerKickEvent>()
                .Subscribe(e =>
                {
                    PlayKickReaction();
                    RodKick.PlayRandom();
                })
                .AddTo(component);
            
            MessageBroker.Default.Receive<PlayerPoebelEvent>()
                .Subscribe(e =>
                {
                    PoebelReaction.PlayRandom();
                })
                .AddTo(component);
            
            MessageBroker.Default.Receive<PlayerPukeEvent>()
                .Subscribe(e => PukeReaction.PlayRandom())
                .AddTo(component);

            Observable.Interval(TimeSpan.FromMilliseconds(8000))
                .Subscribe(_ => GeneralReaction.ToList().Randomize().First().Play())
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
            var audioSource = IoC.Game.GetComponent<AudioSource>();
            audioSource.clip = component.MusicClips[index];
            if (index == 3)
            {
                audioSource.volume = 0.5f;
            }
            audioSource.Play();
        }

        // private void PlayRodriguesSound(PlayerState playerState)
        // {
        //     if (playerState == PlayerState.Puking)
        //     {
        //         RodPuke.PlayRandom();
        //     } else if (playerState == PlayerState.Falling)
        //     {
        //         RodFalling[Random.Range(0, RodFalling.Length-1)].Play();
        //         GeneralReaction[Random.Range(0, GeneralReaction.Length - 1)].Play();
        //     } else if (playerState == PlayerState.Poebeling)
        //     {
        //         RodPoebel[Random.Range(0, RodPoebel.Length - 1)].Play();
        //     }
        // }

        private void RodWasCaught()
        {
            Police[Random.Range(0, Police.Length - 1)].Play();
            RodPolice[Random.Range(0, RodPolice.Length - 1)].Play();
        }

        private void PlayKickReaction()
        {
            RodKick[Random.Range(0, RodKick.Length - 1)].Play();
            KickReaction[Random.Range(0, KickReaction.Length - 1)].Play();
        }
    }
}