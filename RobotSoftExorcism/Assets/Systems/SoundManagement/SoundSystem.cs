using StrongSystems.Audio;
using SystemBase;
using Systems.Player;
using Systems.Player.Events;
using Systems.SceneManagement.Events;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems.SoundManagement
{
    [GameSystem]
    public class SoundSystem : GameSystem<SoundComponent>
    {
        private string[] PukeReaction = new []{ "did_he_female", "gross_female", "shame_female"};
        private string[] KickReaction = new[] { "looks_unstable_male", "police_female", "shame_female", "stop_him_female", "stop_male"};
        private string[] PoebelReaction = new[] { "get_lost_male", "calm_down_male", "not_rioting_male", "police_female", "stop_him_female", "stop_male" };
        private string[] GeneralReaction = new[] {"hammered_male", "is_he_drunk_male", "looks_drunk_female", "same_male", "well_oiled_male", "why_drunk_male"};
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
            //Todo Helen
            // MessageBroker.Default.Receive<PlayerStateChangeEvent>()
            //     .Subscribe(e => PlayRodriguesSound(e.PlayerState))
            //     .AddTo(component);
            // MessageBroker.Default.Receive<PlayerCaughtByPoliceEvent>()
            // .Subscribe(e => "caught".Play())
            // .AddTo(component);
            //
            // MessageBroker.Default.Receive<PlayerDrinkEvent>()
            //     .Subscribe(e => "drink".Play())
            //     .AddTo(component);
            //
            MessageBroker.Default.Receive<PlayerKickEvent>()
                .Subscribe(e => PlayKickReaction())
                .AddTo(component);
            
            MessageBroker.Default.Receive<PlayerTalksToBarkeeper>()
                .Subscribe(e => BarkeeperTalks())
                .AddTo(component);
            
            MessageBroker.Default.Receive<PlayerPoebelEvent>()
                .Subscribe(e => PoebelReaction[Random.Range(0, PoebelReaction.Length-1)].Play())
                .AddTo(component);
            
            MessageBroker.Default.Receive<PlayerPukeEvent>()
                .Subscribe(e => PukeReaction[Random.Range(0, PukeReaction.Length-1)].Play())
                .AddTo(component);
            
            //TODO play on enter bar scene "go_home_male".Play();
        }

        private void BarkeeperTalks()
        {
            Debug.Log("Barkeeper");
            "go_home_male".Play();
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

        private void PlayRodriguesSound(PlayerState playerState)
        {
            if (playerState == PlayerState.Puking)
            {
                "puke".Play();
            } else if (playerState == PlayerState.Falling)
            {
                "falling".Play();
                GeneralReaction[Random.Range(0, GeneralReaction.Length - 1)].Play();
            } else if (playerState == PlayerState.Poebeling)
            {
                "poebel".Play();
            }
        }

        private void PlayKickReaction()
        {
            "kick".Play();
            KickReaction[Random.Range(0, KickReaction.Length - 1)].Play();
        }
    }
}