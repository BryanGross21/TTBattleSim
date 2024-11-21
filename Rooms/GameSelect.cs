using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TTBattleSim.StateManagement;
using System.Threading;
using TTBattleSim.Collisions;


namespace TTBattleSim.Rooms
{
	public enum gameOptions 
	{
		TTO = 0,
		TTR,
		Clash
	}

	public class GameSelect : GameScreen
	{

		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		string[] descriptions = { "The original game, this mode \nfocuses on preserving the\nauthentic experience of \nToontown.\n(currently only \nunder contruction)", "DUMMY", "A reimagined Toontown, with\ndifferent mechanics and\nrepresents a \ndifferent experience.\n(NOT IMPLEMENTED, \nJUST LEADS TO UNDER \nCONSTRUCTION SCREEN)" };

		/// <summary>
		/// Menu song for TTR
		/// </summary>
		Song TTO;
		/// <summary>
		/// Menu song for TTO
		/// </summary>
		Song TTR;
		/// <summary>
		/// Menu song for Corporate Clash
		/// </summary>
		Song Clash;

		Texture2D TTOBack;

		Texture2D TTCCBack;

		Texture2D TTRBack;

		Texture2D overlay;

		Texture2D buttons;

		Texture2D arrowButton;

		BoundingRectangle mouse = new(0, 0, 32, 32);

		public BoundingRectangle Bounds => mouse;

		BoundingRectangle text;

		BoundingRectangle arrow;

		bool colliding;

		bool collidingArrowButton;


		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		ContentManager _content;

		gameOptions current = gameOptions.TTO;

		private SoundEffect option;
		private SoundEffect selected;

		SpriteFont font;

		public GameSelect(Game game, gameOptions gameOp) 
		{
			this.game = game;
			current = gameOp;
		}


		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			TTOBack = _content.Load<Texture2D>("MenuBackgrounds/tto");
			TTCCBack = _content.Load<Texture2D>("MenuBackgrounds/corporateclash");
			TTRBack = _content.Load<Texture2D>("MenuBackgrounds/ttr");
			overlay = _content.Load<Texture2D>("MenuBackgrounds/overlay");
			arrowButton = _content.Load<Texture2D>("Textures/makeatoon_palette_4alla_2");
			buttons = _content.Load<Texture2D>("Textures/Buttons");
			TTO = _content.Load<Song>("MenuMusic/ttotheme");
			TTR = _content.Load<Song>("MenuMusic/ttrtheme");
			Clash = _content.Load<Song>("MenuMusic/clashtheme");
			option = _content.Load<SoundEffect>("SoundEffects/Generic/Next");
			selected = _content.Load<SoundEffect>("SoundEffects/Generic/Select");
			font = _content.Load<SpriteFont>("menuFont");
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(TTO);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

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

			text = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width - 500), (ScreenManager.GraphicsDevice.Viewport.Height - 100), 118*2, 52*2);
			Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);

			if (current == 0)
			{
				arrow = new BoundingRectangle((ScreenManager.Game.GraphicsDevice.Viewport.Width - 200), (ScreenManager.Game.GraphicsDevice.Viewport.Height - 200), 127 * 1.5f, 127 * 1.5f);
			}
			else
			{
				arrow = new BoundingRectangle(50, (ScreenManager.Game.GraphicsDevice.Viewport.Height - 200), 127 * 1.5f, 127 * 1.5f);
			}

			if (mouse.collidesWith(text))
			{
				colliding = true;
				if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
				{
					MediaPlayer.Stop();
					selected.Play();
					if (current == 0)
					{
						foreach (var screen in ScreenManager.GetScreens())
							screen.ExitScreen();

						ScreenManager.AddScreen(new TTO_LevelSelect(game, PlayGround.TTC), null);
					}
					else 
					{
						foreach (var screen in ScreenManager.GetScreens())
							screen.ExitScreen();

						ScreenManager.AddScreen(new UnderConstruction(game, current), null);
					}
				}
			}
			else
			{
				colliding = false;
			}

			if (mouse.collidesWith(arrow))
			{
				collidingArrowButton = true;
				if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
				{
					if (current == gameOptions.TTO)
					{
						current = gameOptions.Clash;
					}
					else
					{
						current = gameOptions.TTO;
					}
					option.Play();

					Song selectedSong = current == gameOptions.TTO ? TTO : Clash;
					if (MediaPlayer.Queue.ActiveSong != selectedSong)
					{
						MediaPlayer.Play(selectedSong);
					}
				}
			}
			else
			{
				collidingArrowButton = false;
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

			Rectangle buttonSource;
			Rectangle arrowSource = new Rectangle(0, 0, 127, 127);
			Color arrowColor = Color.White;
			spriteBatch.Begin();
			Vector2 destination = new Vector2((graphics.Viewport.Width - 500), (graphics.Viewport.Height - 100));
			var destinationRectangle = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
			if (colliding == false)
			{
				buttonSource = new Rectangle(389, 265, 118, 52);
			}
			else 
			{
				buttonSource = new(263, 265, 118, 52);
			}

			if (collidingArrowButton) 
			{
				arrowColor = Color.SkyBlue;
			}

			if (current == 0)
			{
				spriteBatch.Draw(TTOBack, Vector2.Zero, destinationRectangle, Color.White);
			}
			else if ((int)current == 1)
			{
				spriteBatch.Draw(TTRBack, Vector2.Zero, destinationRectangle, Color.White);
			}
			else
			{
				spriteBatch.Draw(TTCCBack, Vector2.Zero, destinationRectangle, Color.White);
			}


			spriteBatch.Draw(overlay, Vector2.Zero, destinationRectangle, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);

			spriteBatch.Draw(buttons, destination, buttonSource, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

			spriteBatch.DrawString(font, "PLAY", destination + new Vector2(50, 0), Color.Black);


			if (current == 0)
			{
				destination = new Vector2((graphics.Viewport.Width - 200), (graphics.Viewport.Height - 200));
				spriteBatch.Draw(arrowButton, destination, arrowSource, arrowColor, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
			}
			else 
			{
				destination = new Vector2((50), (graphics.Viewport.Height - 200));
				spriteBatch.Draw(arrowButton, destination, arrowSource, arrowColor, 0f, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0);
			}



			destination = new Vector2((graphics.Viewport.Width - 1800) / 2, graphics.Viewport.Height - 750);
			spriteBatch.DrawString(font, descriptions[(int)current], destination, Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);

			spriteBatch.End();

		}



	}
}
