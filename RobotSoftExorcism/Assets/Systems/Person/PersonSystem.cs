﻿using System;
using Assets.Utils.Math;
using StrongSystems.Audio;
using SystemBase;
using Systems.Movement;
using Systems.Player;
using Systems.Player.Events;
using Systems.Score;
using UniRx;
using UnityEngine;
using Utils;
using Utils.Plugins;
using Random = UnityEngine.Random;

namespace Systems.Person
{
    [GameSystem]
    public class PersonSystem : GameSystem<PersonComponent, PlayerBrainComponent>
    {
        private readonly ReactiveProperty<PlayerBrainComponent> _player = new ReactiveProperty<PlayerBrainComponent>(); 
        
        public override void Register(PersonComponent component)
        {
            _player.WhereNotNull()
                .Subscribe(_ =>
                {
                    SystemUpdate(component)
                        .Subscribe(UpdatePerson)
                        .AddTo(component);
                })
                .AddTo(component);
        }

        private void UpdatePerson(PersonComponent person)
        {
            var playerMovement = _player.Value.GetComponent<MovementComponent>();
            var personMovement = person.GetComponent<MovementComponent>();
            if (playerMovement.Direction.Value.magnitude > 0.1)
            {
                switch (person.movePattern)
                {
                    case PersonMovePattern.Still:
                        break;
                    case PersonMovePattern.Random:
                        personMovement.Direction.Value = Random.insideUnitCircle;
                        break;
                    case PersonMovePattern.Follow:
                        if (_player.Value.transform.position.DistanceTo(person.transform.position) < 5)
                        {
                            personMovement.Direction.Value =
                                (_player.Value.transform.position - person.transform.position).normalized;
                        }

                        if (person.personType == PersonType.Police)
                        {
                            MovePolice(personMovement, person);
                        }
                        break;
                    case PersonMovePattern.Block:
                        if (_player.Value.transform.position.DistanceTo(person.transform.position) < 5)
                        {
                            var pointInFrontOfPlayer = _player.Value.transform.position.XY() + playerMovement.Velocity.normalized * 2; 
                            personMovement.Direction.Value =
                                (new Vector3(pointInFrontOfPlayer.x, pointInFrontOfPlayer.y) - person.transform.position).normalized;
                        }
                        break;
                    case PersonMovePattern.BehindBar:
                        personMovement.Direction.Value = (Random.value > 0.5f) ? Vector2.left : Vector2.right;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                if (person.personType != PersonType.Police)
                {
                    personMovement.Direction.Value = Vector2.zero;
                }
            }
        }

        private void MovePolice(MovementComponent personMovement, PersonComponent person)
        {
            personMovement.Direction.Value =
                (_player.Value.transform.position - person.transform.position).normalized;
            var finishedGame = IoC.Game.GetComponent<ScoreComponent>().finishedGame;
            if(finishedGame) return;
            
            if (_player.Value.transform.position.DistanceTo(person.transform.position) < 5)
            {
                MessageBroker.Default.Publish(new PlayerCaughtByPoliceEvent());
                IoC.Game.GetComponent<ScoreComponent>().finishedGame = true;
            }
        }

        public override void Register(PlayerBrainComponent component)
        {
            _player.Value = component;
            
            Observable.Timer(TimeSpan.FromMilliseconds(3000))
                .Subscribe(_ => "go_home_male".Play());
        }
    }
}