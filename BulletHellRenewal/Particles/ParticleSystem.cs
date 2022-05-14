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
    public class ParticleSystem : IDisposable
    {
        private Game _game;
        private ParticleEffect _particleEffect;
        private Texture2D _particleTexture;

        public ParticleSystem(Game game)
        {
            _game = game;
        }

        public virtual void LoadContent()
        {
            _particleTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _particleTexture.SetData(new[] { Color.White });

            TextureRegion2D textureRegion = new TextureRegion2D(_particleTexture);
            _particleEffect = new ParticleEffect(autoTrigger: false)
            {
                Position = new Vector2(400, 240),
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(textureRegion, 500, TimeSpan.FromSeconds(2.5), Profile.BoxUniform(250, 100))
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
                                        StartValue = Color.Yellow.ToHsl(),
                                        EndValue = Color.Red.ToHsl(),
                                    }
                                }
                            },
                            new RotationModifier {RotationRate = -2.1f},
                            new RectangleContainerModifier {Width = 800, Height = 480},
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

        public void Update(GameTime time)
        {
            _particleEffect.Update(time.GetElapsedSeconds());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            spriteBatch.Draw(_particleEffect);
            spriteBatch.End();
        }

        public void Dispose()
        {
            UnloadContent();
        }
    }
}
