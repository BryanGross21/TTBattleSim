using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TTBattleSim.Rooms;
using TTBattleSim.StateManagement;

namespace TTBattleSim
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private readonly ScreenManager _screens;
		private float _gameScale;
		private Vector2 _gameOffset;

		const int TARGETWIDTH = 1920;
		const int TARGETHEIGHT = 1080;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			DisplayMode screen = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
			_graphics.IsFullScreen = false;
			_graphics.PreferredBackBufferWidth = screen.Width;
			_graphics.PreferredBackBufferHeight = screen.Height;
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			var screenFactory = new ScreenFactory();
			Services.AddService(typeof(IScreenFactory), screenFactory);

			_screens = new ScreenManager(this);
			Components.Add(_screens);
			_screens.AddScreen(new GameSelect(this, gameOptions.TTO), null);


		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();


			float screenAspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
			float gameAspectRatio = (float)1920 / 1080;

			if (screenAspectRatio < gameAspectRatio)
			{
				_gameScale = (float)GraphicsDevice.Viewport.Height / 1920;
				_gameOffset.Y = (GraphicsDevice.Viewport.Width - 1080 * _gameScale) / 2f;
				_gameOffset.X = 0;
			}
			else
			{
				// Letterbox vertically
				_gameScale = (float)GraphicsDevice.Viewport.Width / 1920;
				_gameOffset.X = (GraphicsDevice.Viewport.Height - 1080 * _gameScale) / 2f;
				_gameOffset.Y = 0;
			}


			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			_spriteBatch.Begin(transformMatrix: Matrix.CreateScale(_gameScale) * Matrix.CreateTranslation(_gameOffset.X, _gameOffset.Y, 0));
				_screens.Draw(gameTime);
			_spriteBatch.End();


			base.Draw(gameTime);
		}
	}
}
