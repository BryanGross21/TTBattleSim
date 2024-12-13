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
using System.Diagnostics.Eventing.Reader;
using TTBattleSim.Entities;
using SharpDX.XInput;

namespace TTBattleSim.Rooms
{
	public class CogTauntScreen : GameScreen
	{
		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		Song[] songs = new Song[9];

		Texture2D[] backgrounds = new Texture2D[21];

		SoundEffect[] cogSounds = new SoundEffect[4];

		Texture2D arrowButton;

		Texture2D textBox;

		/// <summary>
		/// Decides what background to use
		/// </summary>
		int i = 0;

		BoundingRectangle mouse = new(0, 0, 32, 32);

		public BoundingRectangle Bounds => mouse;

		BoundingRectangle text;

		bool colliding;


		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		private KeyboardState past;
		private KeyboardState currentK;

		ContentManager _content;

		Area currentA = Area.Loopy;

		PlayGround current;

		private SoundEffect option;

		SpriteFont font;

		COG[] cogs = new COG[4];

		public CogTauntScreen(Game game, PlayGround current, Area ground, COG[] cogs)
		{
			this.game = game;
			currentA = ground;
			this.cogs = cogs;
			this.current = current;
		}


		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			backgrounds[0] = _content.Load<Texture2D>("Thumbnails/TTC/LoopyBlurred");
			backgrounds[1] = _content.Load<Texture2D>("Thumbnails/TTC/PunchlineBlurred");
			backgrounds[2] = _content.Load<Texture2D>("Thumbnails/TTC/SillyBlurred");
			backgrounds[3] = _content.Load<Texture2D>("Thumbnails/DD/BarnacleBlurred");
			backgrounds[4] = _content.Load<Texture2D>("Thumbnails/DD/LighthouseBlurred");
			backgrounds[5] = _content.Load<Texture2D>("Thumbnails/DD/SeaweedBlurred");
			backgrounds[6] = _content.Load<Texture2D>("Thumbnails/DG/ElmBlurred");
			backgrounds[7] = _content.Load<Texture2D>("Thumbnails/DG/MapleBlurred");
			backgrounds[8] = _content.Load<Texture2D>("Thumbnails/DG/OakBlurred");
			backgrounds[9] = _content.Load<Texture2D>("Thumbnails/MML/AltoBlurred");
			backgrounds[10] = _content.Load<Texture2D>("Thumbnails/MML/BaritoneBlurred");
			backgrounds[11] = _content.Load<Texture2D>("Thumbnails/MML/TenorBlurred");
			backgrounds[12] = _content.Load<Texture2D>("Thumbnails/BRRRGH/WalrusBlurred");
			backgrounds[13] = _content.Load<Texture2D>("Thumbnails/BRRRGH/SleetBlurred");
			backgrounds[14] = _content.Load<Texture2D>("Thumbnails/BRRRGH/PolarBlurred");
			backgrounds[15] = _content.Load<Texture2D>("Thumbnails/DDL/LullabyBlurred");
			backgrounds[16] = _content.Load<Texture2D>("Thumbnails/DDL/PajamaBlurred");
			backgrounds[17] = _content.Load<Texture2D>("Thumbnails/COG/SBHQBlurred");
			backgrounds[18] = _content.Load<Texture2D>("Thumbnails/COG/FactBlurred");
			backgrounds[19] = _content.Load<Texture2D>("Thumbnails/COG/CBHQBlurred");
			backgrounds[20] = _content.Load<Texture2D>("Thumbnails/COG/LBHQBlurred");

			songs[0] = _content.Load<Song>("TTOMusic/TTC/TTC_Battle");
			songs[1] = _content.Load<Song>("TTOMusic/DD/DD_Battle");
			songs[2] = _content.Load<Song>("TTOMusic/DG/DG_Battle");
			songs[3] = _content.Load<Song>("TTOMusic/MML/MML_Battle");
			songs[4] = _content.Load<Song>("TTOMusic/BRRRGH/BRRRGH_Battle");
			songs[5] = _content.Load<Song>("TTOMusic/DDL/DDL_Battle");
			songs[6] = _content.Load<Song>("TTOMusic/COG_HQ/SBHQCBHQ/SBHQ_Battle");
			songs[7] = _content.Load<Song>("TTOMusic/COG_HQ/SBHQCBHQ/CBHQ_Battle");
			songs[8] = _content.Load<Song>("TTOMusic/COG_HQ/LBHQ/LBHQ_Battle");

			cogSounds[0] = _content.Load<SoundEffect>("SoundEffects/CogSounds/COG_VO_grunt");
			cogSounds[1] = _content.Load<SoundEffect>("SoundEffects/CogSounds/COG_VO_murmur");
			cogSounds[2] = _content.Load<SoundEffect>("SoundEffects/CogSounds/COG_VO_question_2");
			cogSounds[3] = _content.Load<SoundEffect>("SoundEffects/CogSounds/COG_VO_question_3");

			arrowButton = _content.Load<Texture2D>("Textures/phase_3_palette_4alla_1");
			textBox = _content.Load<Texture2D>("Textures/textbox");
			option = _content.Load<SoundEffect>("SoundEffects/Generic/Select");
			font = _content.Load<SpriteFont>("menuFont");
			i = GetBackground();
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(songs[(int)current]);
			if (cogs[0].Taunt.Contains('?'))
			{
				cogSounds[3].Play();
			}
			else if (cogs[0].Taunt.Contains('!'))
			{
				cogSounds[0].Play();
			}
			else 
			{
				Random ran = new();
				int random = ran.Next(0, 2);
				if (random == 0)
				{
					cogSounds[1].Play();
				}
				else 
				{
					cogSounds[2].Play();
				}
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			pastMousePosition = currentMousePosition;
			currentMousePosition = Mouse.GetState();
			past = currentK;
			currentK = Keyboard.GetState();


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
			//text = new BoundingRectangle(game.GraphicsDevice.Viewport.Width / 4, game.GraphicsDevice.Viewport.Height / 4, 513 * 2, 234 * 2);
			Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);

			if (mouse.collidesWith(text))
			{
				colliding = true;
				if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
				{

					foreach (var screen in ScreenManager.GetScreens())
						screen.ExitScreen();

					option.Play();

					ScreenManager.AddScreen(new BattleScreen_TTO(game, (int)current, i, currentA, cogs), null);
				}
			}
			else 
			{
				colliding = false;
			}

			if (currentK.IsKeyDown(Keys.Right))
			{
				foreach (var screen in ScreenManager.GetScreens())
					screen.ExitScreen();

				option.Play();

				ScreenManager.AddScreen(new BattleScreen_TTO(game, (int)current, i, currentA, cogs), null);
			}


			mouse.X = mousePosition.X;
			mouse.Y = mousePosition.Y;
		}

	
		private int GetBackground()
		{
			if (current == PlayGround.TTC)
			{
				if (currentA == Area.Loopy)
				{
					return 0;
				}
				else if (currentA == Area.Punchline)
				{
					return 1;
				}
				else if (currentA == Area.Silly)
				{
					return 2;
				}
			}
			else if (current == PlayGround.DD)
			{
				if (currentA == Area.Barnacle)
				{
					return 3;
				}
				else if (currentA == Area.Lighthouse)
				{
					return 4;
				}
				else if (currentA == Area.Seaweed)
				{
					return 5;
				}
			}
			else if (current == PlayGround.DG)
			{
				if (currentA == Area.Elm)
				{
					return 6;
				}
				else if (currentA == Area.Maple)
				{
					return 7;
				}
				else if (currentA == Area.Oak)
				{
					return 8;
				}
			}
			else if (current == PlayGround.MML)
			{
				if (currentA == Area.Alto)
				{
					return 9;
				}
				else if (currentA == Area.Baritone)
				{
					return 10;
				}
				else if (currentA == Area.Tenor)
				{
					return 11;
				}
			}
			else if (current == PlayGround.BRRRGH)
			{
				if (currentA == Area.Walrus)
				{
					return 12;
				}
				else if (currentA == Area.Sleet)
				{
					return 13;
				}
				else if (currentA == Area.Polar)
				{
					return 14;
				}
			}
			else if (current == PlayGround.DDL)
			{
				if (currentA == Area.Lullaby)
				{
					return 15;
				}
				else if (currentA == Area.Pajama)
				{
					return 16;
				}
			}
			else if (current == PlayGround.SBHQ)
			{
				if (currentA == Area.SBHQCourt)
				{
					return 17;
				}
				else if (currentA == Area.SBHQFactoryExterior)
				{
					return 18;
				}
			}
			else if (current == PlayGround.CBHQ)
			{
				if (currentA == Area.CBHQTrainyard)
				{
					return 19;
				}
			}
			else if (current == PlayGround.LBHQ)
			{
				if (currentA == Area.LBHQCourtyard)
				{
					return 20;
				}
			}
			return 1;
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

			Rectangle arrowSource;
			Rectangle boxSource = new Rectangle(0, 0, 513, 234);

			spriteBatch.Begin();
			Vector2 destination = new Vector2((graphics.Viewport.Width - 500), (graphics.Viewport.Height - 100));
			var destinationRectangle = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);

			Color textColor;

			if (colliding == false)
			{
				arrowSource = new Rectangle(392, 119, 24, 23);
				textColor = Color.Black;
			}
			else
			{
				arrowSource = new(446, 64, 24, 23);
				textColor = Color.BlueViolet;
			}


			spriteBatch.Draw(backgrounds[i], Vector2.Zero, destinationRectangle, Color.White);

			spriteBatch.Draw(textBox, new Vector2(graphics.Viewport.Width / 4, graphics.Viewport.Height / 4), boxSource, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

			spriteBatch.Draw(arrowButton, new Vector2((graphics.Viewport.Width + 800) / 2, (graphics.Viewport.Height + 100) / 2), arrowSource, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0);

			spriteBatch.DrawString(font, cogs[0].Taunt, new Vector2((graphics.Viewport.Width + 225) / 4, (graphics.Viewport.Height + 500) / 4), textColor, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);

			spriteBatch.DrawString(font, "Press the right arrow to continue!", new Vector2(50, graphics.Viewport.Height - 100), Color.Black);

			spriteBatch.End();

		}
	}
}
