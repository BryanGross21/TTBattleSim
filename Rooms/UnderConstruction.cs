using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTBattleSim.Collisions;
using TTBattleSim.StateManagement;
using System.Threading;

namespace TTBattleSim.Rooms
{
	public class UnderConstruction : GameScreen
	{
		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		gameOptions gameoptions;

		CircleCamera camera;

		/// <summary>
		/// Menu song for the background
		/// </summary>
		Song Courtyard;

		Texture2D SellbotHQ;

		Texture2D buttons;


		BoundingRectangle mouse = new(0, 0, 32, 32);

		public BoundingRectangle Bounds => mouse;

		BoundingRectangle text;

		bool colliding;


		Crate[] crates;


		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		SpriteFont font;

		SoundEffect selected;

		ContentManager _content;

		public UnderConstruction(Game game, gameOptions gameOptions) 
		{
			this.game = game;
			gameoptions = gameOptions;
		}


		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			buttons = _content.Load<Texture2D>("Textures/Buttons");
			SellbotHQ = _content.Load<Texture2D>("Thumbnails/COG/SellbotHQ");
			Courtyard = _content.Load<Song>("TTOMusic/COG_HQ/SBHQCBHQ/SBHQ");
			font = _content.Load<SpriteFont>("menuFont");
			selected = _content.Load<SoundEffect>("SoundEffects/Generic/Select");
			crates = new Crate[]
				{
					new Crate(game, Matrix.CreateTranslation(-10,-4,0)),
					new Crate(game, Matrix.CreateTranslation(-12,-4,0)),
					new Crate(game, Matrix.CreateTranslation(-14,-2,0)),
					new Crate(game, Matrix.CreateTranslation(-14,0,0)),
					new Crate(game, Matrix.CreateTranslation(-14,2,0)),
					new Crate(game, Matrix.CreateTranslation(-12,4,0)),
					new Crate(game, Matrix.CreateTranslation(-10,4,0)),
					new Crate(game, Matrix.CreateTranslation(-6,2,0)),
					new Crate(game, Matrix.CreateTranslation(-6,0,0)),
					new Crate(game, Matrix.CreateTranslation(-6,-2,0)),
					new Crate(game, Matrix.CreateTranslation(-4,4,0)),
					new Crate(game, Matrix.CreateTranslation(-4,-4,0)),
					new Crate(game, Matrix.CreateTranslation(-2,4,0)),
					new Crate(game, Matrix.CreateTranslation(-2,-4,0)),
					new Crate(game, Matrix.CreateTranslation(0,-2,0)),
					new Crate(game, Matrix.CreateTranslation(0,2,0)),
					new Crate(game, Matrix.CreateTranslation(0,0,0)),
					new Crate(game, Matrix.CreateTranslation(4, 0, 0)),
					new Crate(game, Matrix.CreateTranslation(4, 2, 0)),
					new Crate(game, Matrix.CreateTranslation(6, -4, 0)),
					new Crate(game, Matrix.CreateTranslation(4, -2, 0)),
					new Crate(game, Matrix.CreateTranslation(6, -4, 0)),
					new Crate(game, Matrix.CreateTranslation(8, -4, 0)),
					new Crate(game, Matrix.CreateTranslation(6, 4, 0)),
					new Crate(game, Matrix.CreateTranslation(8, 4, 0)),
					new Crate(game, Matrix.CreateTranslation(10, -2, 0)),
					new Crate(game, Matrix.CreateTranslation(10, 0, 0)),
					new Crate(game, Matrix.CreateTranslation(8, 0, 0)),
					new Crate(game, Matrix.CreateTranslation(12, 0, 0)),
					new Crate(game, Matrix.CreateTranslation(10, -4, -4)),
					new Crate(game, Matrix.CreateTranslation(8, -4, -4)),
					new Crate(game, Matrix.CreateTranslation(6, -4, -4)),
					new Crate(game, Matrix.CreateTranslation(8, -2, -4)),
					new Crate(game, Matrix.CreateTranslation(8, 0, -4)),
					new Crate(game, Matrix.CreateTranslation(8, 2, -4)),
					new Crate(game, Matrix.CreateTranslation(8, 4, -4)),
					new Crate(game, Matrix.CreateTranslation(6, 4, -4)),
					new Crate(game, Matrix.CreateTranslation(10, 4, -4)),
					new Crate(game, Matrix.CreateTranslation(2, 4, -4)),
					new Crate(game, Matrix.CreateTranslation(2, 2, -4)),
					new Crate(game, Matrix.CreateTranslation(2, 0, -4)),
					new Crate(game, Matrix.CreateTranslation(2, -2, -4)),
					new Crate(game, Matrix.CreateTranslation(2, -4, -4)),
					new Crate(game, Matrix.CreateTranslation(0, 2, -4)),
					new Crate(game, Matrix.CreateTranslation(-2, 0, -4)),
					new Crate(game, Matrix.CreateTranslation(-4, -2, -4)),
					new Crate(game, Matrix.CreateTranslation(-4, 0, -4)),
					new Crate(game, Matrix.CreateTranslation(-4, 2, -4)),
					new Crate(game, Matrix.CreateTranslation(-4, -4, -4)),
					new Crate(game, Matrix.CreateTranslation(-4, 4, -4)),
					new Crate(game, Matrix.CreateTranslation(-8, -2, -4)),
					new Crate(game, Matrix.CreateTranslation(-8, 0, -4)),
					new Crate(game, Matrix.CreateTranslation(-8, 2, -4)),
					new Crate(game, Matrix.CreateTranslation(-10, -4, -4)),
					new Crate(game, Matrix.CreateTranslation(-10, 4, -4)),
					new Crate(game, Matrix.CreateTranslation(-12, 4, -4)),
					new Crate(game, Matrix.CreateTranslation(-12, -4, -4)),
				};
			camera = new CircleCamera(game, new Vector3(0, 0, 25), 0.5f);
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(Courtyard);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			camera.Update(gameTime);

			pastMousePosition = currentMousePosition;
			currentMousePosition = Mouse.GetState();


			if (!ScreenManager.Game.IsActive)
			{
				// Pause the music or stop sound effects when the game loses focus
				if (MediaPlayer.State == MediaState.Playing)
				{
					MediaPlayer.Pause();
				}
				return;
			}
			else
			{
				// Resume music if the game becomes active again
				if (MediaPlayer.State == MediaState.Paused)
				{
					MediaPlayer.Resume();
				}
			}
			text = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width - 1100), (ScreenManager.GraphicsDevice.Viewport.Height - 400), 118 * 2, 52 * 2);
			Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);


			if (mouse.collidesWith(text))
			{
				colliding = true;
				if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
				{
					selected.Play();

					foreach (var screen in ScreenManager.GetScreens()) {
						screen.ExitScreen();
					}
					ScreenManager.AddScreen(new GameSelect(game, gameoptions), null);
				}
			}
			else
			{
				colliding = false;
			}

			mouse.X = mousePosition.X;
			mouse.Y = mousePosition.Y;
		}

		/// <summary>
		/// Draws the sprite using the supplied SpriteBatch
		/// </summary>
		/// <param name="gameTime">The game time</param>
		public override void Draw(GameTime gameTime)
		{
			var graphics = ScreenManager.GraphicsDevice;
			var spriteBatch = ScreenManager.SpriteBatch;
			var font = ScreenManager.Font;

			graphics.Clear(Color.Black);

			var destinationRectangle = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
			spriteBatch.Begin();
			spriteBatch.Draw(SellbotHQ, Vector2.Zero, destinationRectangle, Color.White);
			spriteBatch.End();


			foreach (Crate crate in crates)
			{
				crate.Draw(camera);
			}

			Rectangle buttonSource;
			spriteBatch.Begin();
			Vector2 destination = new Vector2((graphics.Viewport.Width - 1100), (graphics.Viewport.Height - 400));
			if (colliding == false)
			{
				buttonSource = new Rectangle(389, 265, 118, 52);
			}
			else
			{
				buttonSource = new(263, 265, 118, 52);
			}




			spriteBatch.Draw(buttons, destination, buttonSource, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

			spriteBatch.DrawString(font, "UNDER \nCONSTRUCTION!!!", new Vector2((graphics.Viewport.Width - 1500)/2, (graphics.Viewport.Height - 700)/2), Color.Red, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(font, " Return to \n Mode Select", destination, Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);

			spriteBatch.End();

		}


	}
}
