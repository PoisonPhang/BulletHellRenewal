using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Particles
{
    public class EnemyGas
    {
        private Game _game;
        private ParticleEffect _particleEffect;
        private Texture2D _particleTexture;
        private Vector2 _position;
        private Vector2 _direction;
        private Color _color1;
        private Color _color2;

        public EnemyGas(Game game, Vector2 position, Color color1, Color color2)
        {
            _game = game;
            _position = position;
            _direction = Vector2.One;
            _color1 = color1;
            _color2 = color2;
        }

        public virtual void LoadContent()
        {
            _particleTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _particleTexture.SetData(new[] { Color.White });

            TextureRegion2D textureRegion = new TextureRegion2D(_particleTexture);
            _particleEffect = new ParticleEffect(autoTrigger: false)
            {
                Position = _position,
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(textureRegion, 500, TimeSpan.FromSeconds(2.5), Profile.Spray(_direction, 2))
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(0f, 50f),
                            Quantity = 3,
                            Rotation = new Range<float>(-1f, 1f),
                            Scale = new Range<float>(3.0f, 4.0f)
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ColorInterpolator
                                    {
                                        StartValue = _color1.ToHsl(),
                                        EndValue = _color2.ToHsl(),
                                    }
                                }
                            },
                            new RotationModifier {RotationRate = -2.1f},
                            new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f},
                        }
                    },
                }
            };
        }

        public void UnloadContent()
        {
            _particleTexture.Dispose();
            _particleEffect.Dispose();
        }

        public void Update(GameTime time, Vector2 position, Vector2 direction)
        {
            _position = position;
            _direction = direction;
            _particleEffect.Position = position;
            _particleEffect.Update(time.GetElapsedSeconds());
        }

        public void Draw(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: transform);
            spriteBatch.Draw(_particleEffect);
            spriteBatch.End();
        }

        public void Dispose()
        {
            UnloadContent();
        }
    }
}
