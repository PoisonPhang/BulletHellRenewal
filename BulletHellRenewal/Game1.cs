using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Collections.Generic;

using BulletHellRenewal.Camera;
using BulletHellRenewal.Controlls;
using BulletHellRenewal.Entities;
using BulletHellRenewal.Map;
using BulletHellRenewal.Particles;
using BulletHellRenewal.Physics;
using System;

namespace BulletHellRenewal
{
    public enum GameState
    {
        Title,
        Game,
        Win,
        Lose,
    }

    public class Game1 : Game
    {
        private const float GRAVITY_Y = 9.8f;

        private BackgroundRenderer _background;
        private FollowerCamera _camera;
        private PlayerCharacter _character;
        private List<IEnemy> _enemies;
        private int _currentEnemy;
        private SpriteFont _font;
        private GraphicsDeviceManager _graphics;
        private GameState _gameState;
        private LevelMap _levelMap;
        private SpriteBatch _spriteBatch;
        private World _world;
        private Title _title;
        private ParticleSystem _particles;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _background = new BackgroundRenderer(new BackgroundLayer[]
            {
                new BackgroundLayer("parallax-mountain-bg", Vector2.One * 3),
                new BackgroundLayer("parallax-mountain-montain-far", Vector2.One * 3),
                new BackgroundLayer("parallax-mountain-mountains", Vector2.One * 3),
                new BackgroundLayer("parallax-mountain-trees", Vector2.One * 3),
                new BackgroundLayer("parallax-mountain-foreground-trees", Vector2.One * 3),
            });
            _camera = new FollowerCamera(this, new Vector2(GraphicsDevice.Viewport.Width / 60, 0), 0, 35 * 32);
            _levelMap = new LevelMap(this, "map-01");
            _world = new World(35 * 32, 17 * 32, new Vector2(0, GRAVITY_Y));
            _character = new PlayerCharacter(this, _world, new Vector2(32, 32), 4, 16, new KeyboardController(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            _enemies = new List<IEnemy>()
            {
                new CircleEnemy(this, _world, 16, 32),
                new RectangleEnemy(this, _world, new Vector2(32, 16), 16),
            };

            _title = new Title(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _particles = new ParticleSystem(this);

            _camera.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _background.LoadContent(Content);
            _levelMap.LoadContent();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Mechanical");

            _character.LoadContent(_levelMap.SpawnPoints[0]);
            (_enemies[_currentEnemy] as CircleEnemy).LoadContent(_levelMap.SpawnPoints[1] - new Vector2(0, 32));

            _world.InsertEntity(_character);
            _world.InsertEntity(_enemies[_currentEnemy]);
            _levelMap.LoadCollision(_world);
            _title.LoadContent(Content);
            _particles.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (_gameState == GameState.Title && Keyboard.GetState().IsKeyDown(Keys.Enter))
                _gameState = GameState.Game;

            _levelMap.Update(gameTime);
            _world.Update(gameTime);
            _character.Update(gameTime);
            if (_gameState == GameState.Game)
                UpdateEnemies(gameTime);
            _camera.Update(_character.Bounds.Position);
            _background.Update(_camera.Position);
            _particles.Update(gameTime);

            if (_character.HitPoints <= 0)
                _gameState = GameState.Lose;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _background.Draw(_spriteBatch);

            if (_gameState == GameState.Title)
            {
                _spriteBatch.Begin();
                DrawTitleScreen(gameTime);
                _spriteBatch.End();
            }
            else if (_gameState == GameState.Game)
            {
                _levelMap.Draw(_camera.GetViewMatrix());

                _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
                _character.Draw(_spriteBatch);
                _enemies[_currentEnemy].Draw(_spriteBatch);
                _world.Draw(_spriteBatch);
                _spriteBatch.End();

                if (!_character.HasShot)
                {
                    _spriteBatch.Begin();
                    string message = "Move with [A] & [D], Shoot with right click";
                    Vector2 position = new Vector2(GraphicsDevice.Viewport.Width / 2 - GetStringOffset(message), GraphicsDevice.Viewport.Height / 8 * 6);
                    _spriteBatch.DrawString(_font, message, position, Color.White);
                    _spriteBatch.End();
                }

                _enemies[_currentEnemy].Particles.Draw(_spriteBatch, _camera.GetViewMatrix());

            }
            else if (_gameState == GameState.Win)
            {
                _particles.Draw(_spriteBatch);
                DrawExitScreen("You Win!");
            }
            else if (_gameState == GameState.Lose)
                DrawExitScreen("The Enemies Won :(");
            base.Draw(gameTime);
        }

        private void DrawTitleScreen(GameTime time)
        {
            _title.Draw(_spriteBatch, time);
            string message = "Press [Enter] to Play";
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width / 2 - GetStringOffset(message), GraphicsDevice.Viewport.Height / 8 * 6);
            _spriteBatch.DrawString(_font, message, position, Color.White);
            message = "Press [ESC] to Quit";
            position = new Vector2(GraphicsDevice.Viewport.Width / 2 - GetStringOffset(message), GraphicsDevice.Viewport.Height / 8 * 7);
            _spriteBatch.DrawString(_font, message, position, Color.White);
        }

        private void DrawExitScreen(string message)
        {
            _spriteBatch.Begin();
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width / 2 - GetStringOffset(message), GraphicsDevice.Viewport.Height / 8 * 6);
            _spriteBatch.DrawString(_font, message, position, Color.White);
            message = "Press [ESC] to Quit";
            position = new Vector2(GraphicsDevice.Viewport.Width / 2 - GetStringOffset(message), GraphicsDevice.Viewport.Height / 8 * 7);
            _spriteBatch.DrawString(_font, message, position, Color.White);
            _spriteBatch.End();
        }

        private float GetStringOffset(string message)
        {
            return _font.MeasureString(message).X / 2;
        }

        private void UpdateEnemies(GameTime time)
        {
            if (_enemies[_currentEnemy].HitPoints <= 0)
            {
                _currentEnemy++;
                if (_currentEnemy >= _enemies.Count)
                {
                    _gameState = GameState.Win;
                    return;
                }

                (_enemies[_currentEnemy] as RectangleEnemy).LoadContent(_levelMap.SpawnPoints[1] - new Vector2(0, 32));
                _enemies[_currentEnemy - 1].Dispose();
                _world.InsertEntity(_enemies[_currentEnemy]);
            }


            _enemies[_currentEnemy].Update(time);
            _enemies[_currentEnemy].Target = _character.Bounds.Position;
        }
    }
}
