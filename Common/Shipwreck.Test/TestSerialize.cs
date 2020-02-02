using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shipwreck.Math;
using Shipwreck.WorldData;

namespace Shipwreck.Test
{
    [TestClass]
    public class TestSerialize
    {
        [TestMethod]
        public void SerializePlayer()
        {
            var player = new Player
            {
                Guid = Guid.NewGuid(),
                Name = "Player 1",
                Type = PlayerType.Astronaut
            };

            // Serialize to/from JSON
            var json = player.ToJson();
            var player2 = Player.FromJson(json);

            // Assert JSON representations are equal
            Assert.AreEqual(json, player2.ToJson());
        }

        [TestMethod]
        public void SerializeAstronaut()
        {
            var astronaut = new Astronaut
            {
                Guid = Guid.NewGuid(),
                Position = new Vec2(1, 2),
                Velocity = new Vec2(3, 4)
            };

            // Serialize to/from JSON
            var json = astronaut.ToJson();
            var astronaut2 = Astronaut.FromJson(json);

            // Assert JSON representations are equal
            Assert.AreEqual(json, astronaut2.ToJson());
        }

        [TestMethod]
        public void SerializeAsteroid()
        {
            var asteroid = new Asteroid
            {
                Guid = Guid.NewGuid(),
                Owner = Guid.NewGuid(),
                Position = new Vec3(1, 2, 3),
                Velocity = new Vec3(4, 5, 6)
            };

            // Serialize to/from JSON
            var json = asteroid.ToJson();
            var asteroid2 = Asteroid.FromJson(json);

            // Assert JSON representations are equal
            Assert.AreEqual(json, asteroid2.ToJson());
        }

        [TestMethod]
        public void SerializeGamePlayers()
        {
            var players = new GamePlayers
            {
                Players = new List<Player>
                {
                    new Player
                    {
                        Guid = Guid.NewGuid(),
                        Name = "Player 1",
                        Type = PlayerType.Astronaut
                    },
                    new Player
                    {
                        Guid = Guid.NewGuid(),
                        Name = "Player 2",
                        Type = PlayerType.Astronaut
                    },
                    new Player
                    {
                        Guid = Guid.NewGuid(),
                        Name = "Player 3",
                        Type = PlayerType.Alien
                    }
                }
            };

            // Serialize to/from JSON
            var json = players.ToJson();
            var players2 = GamePlayers.FromJson(json);

            // Assert JSON representations are equal
            Assert.AreEqual(json, players2.ToJson());

            // Assert players count is equal
            Assert.AreEqual(players.Players.Count, players2.Players.Count);
        }

        [TestMethod]
        public void SerializeGameState()
        {
            var gameState = new GameState
            {
                Ship = new Ship
                {
                    Wing1Health = 54,
                    Wing2Health = 32,
                    Wing3Health = 10,
                    Shielded = true
                },
                Astronauts = new List<Astronaut>
                {
                    new Astronaut
                    {
                        Guid = Guid.NewGuid(),
                        Position = new Vec2(1, 2),
                        Velocity = new Vec2(3, 4)
                    }
                },
                Asteroids = new List<Asteroid>
                {
                    new Asteroid
                    {
                        Guid = Guid.NewGuid(),
                        Owner = Guid.NewGuid(),
                        Position = new Vec3(1, 2, 3),
                        Velocity = new Vec3(4, 5, 6)
                    }
                }
            };


            // Serialize to/from JSON
            var json = gameState.ToJson();
            var gameState2 = GameState.FromJson(json);

            // Assert JSON representations are equal
            Assert.AreEqual(json, gameState2.ToJson());
        }
    }
}
