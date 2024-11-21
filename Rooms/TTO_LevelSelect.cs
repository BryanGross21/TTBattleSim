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

namespace TTBattleSim.Rooms
{
	public enum PlayGround 
	{
		TTC = 0,
		DD,
		DG,
		MML,
		BRRRGH,
		DDL,
		SBHQ,
		CBHQ,
		LBHQ,
		BBHQ
	}

	public enum Area 
	{
		Loopy = 0,
		Punchline,
		Silly,
		Barnacle,
		Lighthouse,
		Seaweed,
		Elm,
		Maple,
		Oak,
		Alto,
		Baritone,
		Tenor,
		Walrus,
		Sleet,
		Polar,
		Lullaby,
		Pajama,
		Building,
		SBHQCourt,
		SBHQFactoryExterior,
		SBHQFrontFactory,
		SBHQSideFactory,
		VP,
		CBHQTrainyard,
		CBHQCoin,
		CBHQDollar,
		CBHQBullion,
		CFO,
		LBHQCourtyard,
		LBHQOfficeA,
		LBHQOfficeB,
		LBHQOfficeC,
		LBHQOfficeD,
		CJ,
		BBHQFront,
		BBHQMiddle,
		BBHQBack,
		CEO
	}

	public class TTO_LevelSelect : GameScreen
	{
		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		string[] playgroundDescriptions = { "The original game, this mode \nfocuses on preserving the\nauthentic experience of \nToontown.\n(currently only \nunder contruction)", "DUMMY", "A reimagined Toontown, with\ndifferent mechanics and\nrepresents a \ndifferent experience.\n(NOT IMPLEMENTED, \nJUST LEADS TO UNDER \nCONSTRUCTION SCREEN)" };

		Song[] songs = new Song[24];

		Texture2D[] backgrounds = new Texture2D[42];

		Texture2D overlay;

		Texture2D buttons;

		Texture2D arrowButton;

		/// <summary>
		/// Decides what background to use
		/// </summary>
		int i = 0;


		BoundingRectangle mouse = new(0, 0, 32, 32);

		public BoundingRectangle Bounds => mouse;

		BoundingRectangle text;

		BoundingRectangle leftArrow;

		BoundingRectangle rightArrow;

		bool colliding;

		bool collidingLeftArrowButton;

		bool collidingRightArrowButton;

		bool selectingPlayground = true;

		bool showLeft;

		bool showRight;

		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		ContentManager _content;

		PlayGround current = PlayGround.TTC;

		Area currentA = Area.Loopy;

		private SoundEffect option;

		SpriteFont font;

		public TTO_LevelSelect(Game game, PlayGround ground)
		{
			this.game = game;
			current = ground;
		}


		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			backgrounds[0] = _content.Load<Texture2D>("Thumbnails/TTC/TTC");
			backgrounds[1] = _content.Load<Texture2D>("Thumbnails/TTC/Loopy");
			backgrounds[2] = _content.Load<Texture2D>("Thumbnails/TTC/Punchline");
			backgrounds[3] = _content.Load<Texture2D>("Thumbnails/TTC/Silly");
			backgrounds[4] = _content.Load<Texture2D>("Thumbnails/TTC/CogBuildingTTC");
			backgrounds[5] = _content.Load<Texture2D>("Thumbnails/DD/DD");
			backgrounds[6] = _content.Load<Texture2D>("Thumbnails/DD/Anchor");
			backgrounds[7] = _content.Load<Texture2D>("Thumbnails/DD/Lighthouse");
			backgrounds[8] = _content.Load<Texture2D>("Thumbnails/DD/Seaweed");
			backgrounds[9] = _content.Load<Texture2D>("Thumbnails/DD/CogBuildingDD");
			backgrounds[10] = _content.Load<Texture2D>("Thumbnails/DG/DG");
			backgrounds[11] = _content.Load<Texture2D>("Thumbnails/DG/Elm");
			backgrounds[12] = _content.Load<Texture2D>("Thumbnails/DG/Maple");
			backgrounds[13] = _content.Load<Texture2D>("Thumbnails/DG/Oak");
			backgrounds[14] = _content.Load<Texture2D>("Thumbnails/DG/CogBuildingDG");
			backgrounds[15] = _content.Load<Texture2D>("Thumbnails/MML/MML");
			backgrounds[16] = _content.Load<Texture2D>("Thumbnails/MML/Alto");
			backgrounds[17] = _content.Load<Texture2D>("Thumbnails/MML/Baritone");
			backgrounds[18] = _content.Load<Texture2D>("Thumbnails/MML/Tenor");
			backgrounds[19] = _content.Load<Texture2D>("Thumbnails/MML/CogBuildingMML");
			backgrounds[20] = _content.Load<Texture2D>("Thumbnails/BRRRGH/Brrrgh");
			backgrounds[21] = _content.Load<Texture2D>("Thumbnails/BRRRGH/Walrus");
			backgrounds[22] = _content.Load<Texture2D>("Thumbnails/BRRRGH/Sleet");
			backgrounds[23] = _content.Load<Texture2D>("Thumbnails/BRRRGH/Polar");
			backgrounds[24] = _content.Load<Texture2D>("Thumbnails/BRRRGH/CogBuildingBrrrgh");
			backgrounds[25] = _content.Load<Texture2D>("Thumbnails/DDL/DDL");
			backgrounds[26] = _content.Load<Texture2D>("Thumbnails/DDL/Lullaby");
			backgrounds[27] = _content.Load<Texture2D>("Thumbnails/DDL/Pajama_Place");
			backgrounds[28] = _content.Load<Texture2D>("Thumbnails/DDL/CogBuildingDDL");
			backgrounds[29] = _content.Load<Texture2D>("Thumbnails/COG/SellbotHQ");
			backgrounds[30] = _content.Load<Texture2D>("Thumbnails/COG/FactCourt");
			backgrounds[31] = _content.Load<Texture2D>("Thumbnails/COG/Fact");
			backgrounds[32] = _content.Load<Texture2D>("Thumbnails/COG/VP");
			backgrounds[33] = _content.Load<Texture2D>("Thumbnails/COG/CashbotHQ");
			backgrounds[34] = _content.Load<Texture2D>("Thumbnails/COG/Mints");
			backgrounds[35] = _content.Load<Texture2D>("Thumbnails/COG/CFO");
			backgrounds[36] = _content.Load<Texture2D>("Thumbnails/COG/LawbotHQ");
			backgrounds[37] = _content.Load<Texture2D>("Thumbnails/COG/DAOffice");
			backgrounds[38] = _content.Load<Texture2D>("Thumbnails/COG/CJ");
			backgrounds[39] = _content.Load<Texture2D>("Thumbnails/COG/BossbotHQ");
			backgrounds[40] = _content.Load<Texture2D>("Thumbnails/COG/Golf");
			backgrounds[41] = _content.Load<Texture2D>("Thumbnails/COG/CEO");

			songs[0] = _content.Load<Song>("TTOMusic/TTC/TTC");
			songs[1] = _content.Load<Song>("TTOMusic/TTC/TTC_Street");
			songs[2] = _content.Load<Song>("TTOMusic/COG_HQ/Shared/Build_Battle");
			songs[3] = _content.Load<Song>("TTOMusic/DD/DD");
			songs[4] = _content.Load<Song>("TTOMusic/DD/DD_Street");
			songs[5] = _content.Load<Song>("TTOMusic/DG/DG");
			songs[6] = _content.Load<Song>("TTOMusic/DG/DG_Street");
			songs[7] = _content.Load<Song>("TTOMusic/MML/mml");
			songs[8] = _content.Load<Song>("TTOMusic/MML/mml_street");
			songs[9] = _content.Load<Song>("TTOMusic/BRRRGH/brrrgh");
			songs[10] = _content.Load<Song>("TTOMusic/BRRRGH/brrrgh_street");
			songs[11] = _content.Load<Song>("TTOMusic/DDL/ddl");
			songs[12] = _content.Load<Song>("TTOMusic/DDL/ddl_street");
			songs[13] = _content.Load<Song>("TTOMusic/COG_HQ/SBHQCBHQ/SBHQ");
			songs[14] = _content.Load<Song>("TTOMusic/COG_HQ/Shared/factory");
			songs[15] = _content.Load<Song>("TTOMusic/COG_HQ/LBHQ/lbhq");
			songs[16] = _content.Load<Song>("TTOMusic/COG_HQ/LBHQ/cj_boss");
			songs[17] = _content.Load<Song>("TTOMusic/COG_HQ/BBHQ/bbhq_1");
			songs[18] = _content.Load<Song>("TTOMusic/COG_HQ/BBHQ/bbhq_2");
			songs[19] = _content.Load<Song>("TTOMusic/COG_HQ/BBHQ/bbhq_3");
			songs[20] = _content.Load<Song>("TTOMusic/COG_HQ/BBHQ/bbhq_golf_1");
			songs[21] = _content.Load<Song>("TTOMusic/COG_HQ/BBHQ/bbhq_golf_2");
			songs[22] = _content.Load<Song>("TTOMusic/COG_HQ/BBHQ/bbhq_golf_3");
			songs[23] = _content.Load<Song>("TTOMusic/COG_HQ/BBHQ/CEO");

			overlay = _content.Load<Texture2D>("MenuBackgrounds/overlay");
			buttons = _content.Load<Texture2D>("Textures/Buttons");
			arrowButton = _content.Load<Texture2D>("Textures/makeatoon_palette_4alla_2");
			option = _content.Load<SoundEffect>("SoundEffects/Generic/Select");
			font = _content.Load<SpriteFont>("menuFont");
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(songs[0]);
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

			text = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width - 500), (ScreenManager.GraphicsDevice.Viewport.Height - 100), 118 * 2, 52 * 2);
			Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);

			if (selectingPlayground)
			{
				if (current == 0)
				{
					showLeft = false;
					showRight = true;
				}
				else if (current == PlayGround.BBHQ)
				{
					showRight = false;
					showLeft = true;
				}
				else 
				{
					showLeft = true;
					showRight = true;
				}

				if (showRight)
				{
					rightArrow = new BoundingRectangle((ScreenManager.Game.GraphicsDevice.Viewport.Width - 200), (ScreenManager.Game.GraphicsDevice.Viewport.Height - 200), 127 * 1.5f, 127 * 1.5f);
				}
				if (showLeft)
				{
					leftArrow = new BoundingRectangle(50, (ScreenManager.Game.GraphicsDevice.Viewport.Height - 200), 127 * 1.5f, 127 * 1.5f);
				}

				if (mouse.collidesWith(leftArrow))
				{
					collidingLeftArrowButton = true;
					collidingRightArrowButton = false;
					if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
					{
						current--;
						option.Play();

						i = GetBackground();
						MediaPlayer.Play(GetSong());
						MediaPlayer.IsRepeating = true;
					}
				}
				else if (mouse.collidesWith(rightArrow))
				{
					collidingLeftArrowButton = false;
					collidingRightArrowButton = true;
					if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
					{
						current++;
						option.Play();

						i = GetBackground();
						MediaPlayer.Play(GetSong());
						MediaPlayer.IsRepeating = true;
					}
				}
				else
				{
					collidingLeftArrowButton = false;
					collidingRightArrowButton = false;
				}

			}


			/*if (mouse.collidesWith(text))
			{
				colliding = true;
				option.Play();
			}
			else
			{
				colliding = false;
			}*/

			mouse.X = mousePosition.X;
			mouse.Y = mousePosition.Y;
		}

		private Song GetSong() 
		{
			if (selectingPlayground) 
			{
				if (current == PlayGround.TTC)
				{
					return songs[0];
				}
				else if (current == PlayGround.DD)
				{
					return songs[3];
				}
				else if (current == PlayGround.DG)
				{
					return songs[5];
				}
				else if (current == PlayGround.MML)
				{
					return songs[7];
				}
				else if (current == PlayGround.BRRRGH)
				{
					return songs[9];
				}
				else if (current == PlayGround.DDL)
				{
					return songs[11];
				}
				else if (current == PlayGround.SBHQ || current == PlayGround.CBHQ)
				{
					return songs[13];
				}
				else if (current == PlayGround.LBHQ)
				{
					return songs[15];
				}
				else 
				{
					Random ran = new();
					int random = ran.Next(0, 3);
					if (random == 0)
					{
						return songs[17];
					}
					else if (random == 1) 
					{
						return songs[18];
					}
					else 
					{
						return songs[19];
					}
				}
			}
			return null;
		}

		private int GetBackground()
		{
			if (selectingPlayground)
			{
				if (current == PlayGround.TTC)
				{
					return 0;
				}
				else if (current == PlayGround.DD)
				{
					return 5;
				}
				else if (current == PlayGround.DG)
				{
					return 10;
				}
				else if (current == PlayGround.MML)
				{
					return 15;
				}
				else if (current == PlayGround.BRRRGH)
				{
					return 20;
				}
				else if (current == PlayGround.DDL)
				{
					return 25;
				}
				else if (current == PlayGround.SBHQ)
				{
					return 29;
				}
				else if (current == PlayGround.CBHQ) 
				{
					return 33;
				}
				else if (current == PlayGround.LBHQ)
				{
					return 36;
				}
				else
				{
					return 39;
				}
			}
			return 0;
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

			Rectangle arrowSource = new Rectangle(0, 0, 127, 127);
			Color leftArrowColor;
			Color rightArrowColor;

			if (collidingLeftArrowButton)
			{
				leftArrowColor = Color.SkyBlue;
				rightArrowColor = Color.White;
			}
			else if (collidingRightArrowButton)
			{
				leftArrowColor = Color.White;
				rightArrowColor = Color.SkyBlue;
			}
			else
			{
				leftArrowColor = Color.White;
				rightArrowColor = Color.White;
			}

			Rectangle buttonSource;
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

			spriteBatch.Draw(backgrounds[i], Vector2.Zero, destinationRectangle, Color.White);


			spriteBatch.Draw(overlay, Vector2.Zero, destinationRectangle, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);

			spriteBatch.Draw(buttons, destination, buttonSource, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

			spriteBatch.DrawString(font, "PLAY", destination + new Vector2(50, 0), Color.Black);


			if (showRight)
			{
				destination = new Vector2((graphics.Viewport.Width - 200), (graphics.Viewport.Height - 200));
				spriteBatch.Draw(arrowButton, destination, arrowSource, rightArrowColor, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
			}
			if(showLeft)
			{
				destination = new Vector2((50), (graphics.Viewport.Height - 200));
				spriteBatch.Draw(arrowButton, destination, arrowSource, leftArrowColor, 0f, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0);
			}

			destination = new Vector2((graphics.Viewport.Width - 1800) / 2, graphics.Viewport.Height - 750);
			//spriteBatch.DrawString(font, descriptions[(int)current], destination, Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);

			spriteBatch.End();

		}
	}
}
