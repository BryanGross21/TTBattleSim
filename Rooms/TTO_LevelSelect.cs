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
		TTCBuilding,
		Barnacle,
		Lighthouse,
		Seaweed,
		DDBuilding,
		Elm,
		Maple,
		Oak,
		DGBuilding,
		Alto,
		Baritone,
		Tenor,
		MMLBuilding,
		Walrus,
		Sleet,
		Polar,
		BRRRGHBuilding,
		Lullaby,
		Pajama,
		DDLBuilding,
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

		string[] playgroundDescriptions = { "Low level fun to be\nhad in the heart of Toontown.\nCOGS range from\nlevels 1 to\n4 here.", "Riskier encounters in\nthe foggy docks of\nToontown. COGS range\nfrom levels 2 to 6.", "Frolic and fight through \nToontown's gardens. COGS \nrange from levels\n2 to 6.", "Listen to the fabulous beats\nof Toontown's own Melodyland.\nCOGS range from\nlevels 2 to 6.", "BRRRGH, IT'S CHILLY I\nNEED A JACKET, BRRRGH. COGS \nRANGE FROM LEVELS\n5 to 9.", "ZZZZZZZZZ Sleepy ZZZZZZZZ.\nCOGS ZZZZ range from \nZZZZ levels 6 to 9.", "The smog filled factories\nof the dastardly Sellbots.\nDon't fall for these guys\nbad sales pitches. COGS \nrange from levels 4 to 6.", "The skyscrapers and trains\nof the ever present Cashbots.\nDo you feel like these guys\nare compensating for\nsomething? COGS range\nfrom levels 7 to 9.", "Judicious and justice are \nthe Lawbots and that goes\nfor their HQ. I would\nnot get these guys to\nrepresent you in court.\nCOGS range from levels\n 7 to 10.", "Vacant and empty, how odd \nfor the Bossbots? COGS don't \nroam here, where could they\nbe?" };

		string[] areaDescriptions = { "Loopy Lane (Spawn%):\nSellbot: 10%\nCashbot: 10%\nLawbot: 70%\nBossbot: 10%", "Punchline Place (Spawn%):\nSellbot: 40%\nCashbot: 40%\nLawbot: 10%\nBossbot: 10%", "Silly Street (Spawn%):\nSellbot: 25%\nCashbot: 25%\nLawbot: 25%\nBossbot: 25%", "Fight a random\n1 to 3 story building.", "Barnacle Boulevard (Spawn%):\nSellbot: 0%\nCashbot: 0%\nLawbot: 10%\nBossbot: 90%", "Lighthouse Lane (Spawn%):\nSellbot: 10%\nCashbot: 10%\nLawbot: 40%\nBossbot: 40%", "Seaweed Street (Spawn%):\nSellbot: 10%\nCashbot: 90%\nLawbot: 0%\nBossbot: 0%", "Take on a random\n2 to 4 story building." , "Elm Street (Spawn%):\nSellbot: 70%\nCashbot: 10%\nLawbot: 20%\nBossbot: 0%", "Maple Street (Spawn%):\nSellbot: 20%\nCashbot: 0%\nLawbot: 70%\nBossbot: 10%", "Oak Street (Spawn%):\nSellbot: 85%\nCashbot: 5%\nLawbot: 5%\nBossbot: 5%", "Take on a random \n3 to to 4 story building.", "Alto Avenue (Spawn%):\nSellbot: 50%\nCashbot: 50%\nLawbot: 0%\nBossbot: 0%", "Baritone Boulevard (Spawn%):\nSellbot: 0%\nCashbot: 90%\nLawbot: 0%\nBossbot: 10%", "Tenor Terrace (Spawn%):\nSellbot: 0%\nCashbot: 0%\nLawbot: 50%\nBossbot: 50%", "Take on a random \n3 to 4 story building", "Walrus Way (Spawn%):\nSellbot: 0%\nCashbot: 0%\nLawbot: 10%\nBossbot: 90%", "Sleet Street (Spawn%):\nSellbot: 40%\nCashbot: 30%\nLawbot: 20%\nBossbot: 10%", "Polar Place (Spawn%):\nSellbot: 5%\nCashbot: 5%\nLawbot: 85%\nBossbot: 5%", "Take on a random\n3 to 5 story building.", "Lullaby Lane (Spawn%):\nSellbot: 25%\nCashbot: 25%\nLawbot: 25%\nBossbot: 25%", "Pajama Place (Spawn%):\nSellbot: 5%\nCashbot: 85%\nLawbot: 5%\nBossbot: 5%", "Take on a random\n 4 to 5 story building.", "Sellbot Courtyard (Spawn%):\nSellbot: 100%\nCashbot: 0%\nLawbot: 0%\nBossbot: 0%", "Factory Exterior (Spawn%):\nSellbot: 100%\nCashbot: 0%\nLawbot: 0%\nBossbot: 0%", "Fight your way through\nthe Sellbot Factory. This\none starts from\nthe front enterance.", "Fight your way through\nthe Sellbot Factory. This\none starts from\nthe side enterance.", "Take on 2 waves of cogs in\norder to fight the VP." };

		Song[] songs = new Song[24];

		Texture2D[] backgrounds = new Texture2D[42];

		Texture2D overlay;

		Texture2D buttons;

		Texture2D arrowButton;

		Texture2D exitButton;

		/// <summary>
		/// Decides what background to use
		/// </summary>
		int i = 0;

		Area[] currentAreas;

		BoundingRectangle mouse = new(0, 0, 32, 32);

		public BoundingRectangle Bounds => mouse;

		BoundingRectangle text;

		BoundingRectangle back;

		BoundingRectangle exit;

		BoundingRectangle leftArrow;

		BoundingRectangle rightArrow;

		bool colliding;

		bool collidingLeftArrowButton;

		bool collidingRightArrowButton;

		bool collidingBackButton;

		bool collidingExitButton;

		bool showBackButton;

		bool selectingPlayground = true;

		bool showLeft;

		bool showRight;

		int amountOfAreas;

		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		ContentManager _content;

		PlayGround current = PlayGround.TTC;

		Area currentA = Area.Loopy;

		private SoundEffect option;

		SpriteFont font;

		int areaSelection = 1;

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

			exitButton = _content.Load<Texture2D>("Textures/phase_3_palette_4alla_1");
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

			if (showBackButton)
			{
				back = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width - 750), (ScreenManager.GraphicsDevice.Viewport.Height - 100), 118 * 2, 52 * 2);
			}
			else 
			{
				back = new BoundingRectangle(0, 0, 0, 0);
			}

			exit = new BoundingRectangle(0, 0, 125, 125);

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
				else
				{
					rightArrow = new BoundingRectangle(0, 0, 0, 0);
				}
				if (showLeft)
				{
					leftArrow = new BoundingRectangle(50, (ScreenManager.Game.GraphicsDevice.Viewport.Height - 200), 127 * 1.5f, 127 * 1.5f);
				}
				else
				{
					leftArrow = new BoundingRectangle(0, 0, 0, 0);
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
			else 
			{
				if (areaSelection == 1)
				{
					showLeft = false;
					showRight = true;
				}
				else if (areaSelection == amountOfAreas)
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
				else
				{
					rightArrow = new BoundingRectangle(0, 0, 0, 0);
				}
				if (showLeft)
				{
					leftArrow = new BoundingRectangle(50, (ScreenManager.Game.GraphicsDevice.Viewport.Height - 200), 127 * 1.5f, 127 * 1.5f);
				}
				else
				{
					leftArrow = new BoundingRectangle(0, 0, 0, 0);
				}

				if (mouse.collidesWith(leftArrow))
				{
					collidingLeftArrowButton = true;
					collidingRightArrowButton = false;
					if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
					{
						currentA--;
						areaSelection--;
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
						currentA++;
						areaSelection++;
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


			if (mouse.collidesWith(text))
			{
				colliding = true;
				if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
				{
					if (selectingPlayground)
					{
						selectingPlayground = false;
						showBackButton = true;
					}
					else if (selectingPlayground == false)
					{
						selectingPlayground = true;
						foreach (var screen in ScreenManager.GetScreens())
							screen.ExitScreen();

						ScreenManager.AddScreen(new UnderConstruction(game, gameOptions.TTO), null);
					}


					option.Play();

					if (selectingPlayground == false)
					{
						if (current == PlayGround.TTC || current == PlayGround.DD || current == PlayGround.DG || current == PlayGround.MML || current == PlayGround.BRRRGH || current == PlayGround.BBHQ)
						{
							amountOfAreas = 4;
							if (current == PlayGround.TTC)
							{
								currentA = Area.Loopy;
							}
							else if (current == PlayGround.DD)
							{
								currentA = Area.Barnacle;
							}
							else if (current == PlayGround.DG)
							{
								currentA = Area.Elm;
							}
							else if (current == PlayGround.MML)
							{
								currentA = Area.Alto;
							}
							else if (current == PlayGround.BRRRGH)
							{
								currentA = Area.Walrus;
							}
							else if (current == PlayGround.BBHQ)
							{
								currentA = Area.BBHQFront;
							}
							i = GetBackground();
							MediaPlayer.Play(GetSong());
							MediaPlayer.IsRepeating = true;
						}
						else if (current == PlayGround.SBHQ || current == PlayGround.CBHQ)
						{
							amountOfAreas = 5;
							if (current == PlayGround.SBHQ)
							{
								currentA = Area.SBHQCourt;
							}
							else
							{
								currentA = Area.CBHQTrainyard;
							}
							i = GetBackground();
							MediaPlayer.Play(GetSong());
							MediaPlayer.IsRepeating = true;
						}
						else if (current == PlayGround.DDL)
						{
							amountOfAreas = 3;
							currentA = Area.Lullaby;
							i = GetBackground();
							MediaPlayer.Play(GetSong());
							MediaPlayer.IsRepeating = true;
						}
						else
						{
							amountOfAreas = 6;
							currentA = Area.LBHQCourtyard;
							i = GetBackground();
							MediaPlayer.Play(GetSong());
							MediaPlayer.IsRepeating = true;
						}

					}
				}

			}
			else
			{
				colliding = false;
			}

			if (mouse.collidesWith(back))
			{
				collidingBackButton = true;
				if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
				{
					showBackButton = false;
					selectingPlayground = true;
					areaSelection = 1;

					option.Play();

					i = GetBackground();
					MediaPlayer.Play(GetSong());
					MediaPlayer.IsRepeating = true;
				}
			}
			else
			{
				collidingBackButton = false;
			}

			if (mouse.collidesWith(exit))
			{
				collidingExitButton = true;
				if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
				{
					selectingPlayground = true;
					foreach (var screen in ScreenManager.GetScreens())
						screen.ExitScreen();

					ScreenManager.AddScreen(new GameSelect(game, gameOptions.TTO), null);

					option.Play();
				}
			}
			else
			{
				collidingExitButton = false;
			}

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
			else 
			{
				if (current == PlayGround.TTC)
				{
					if (currentA == Area.Loopy || currentA == Area.Punchline || currentA == Area.Silly)
					{
						return songs[1];
					}
					else
					{
						return songs[2];
					}
				}
				else if (current == PlayGround.DD)
				{
					if (currentA == Area.Barnacle || currentA == Area.Seaweed || currentA == Area.Lighthouse)
					{
						return songs[4];
					}
					else
					{
						return songs[2];
					}
				}
				else if (current == PlayGround.DG)
				{
					if (currentA == Area.Elm || currentA == Area.Oak || currentA == Area.Maple)
					{
						return songs[6];
					}
					else
					{
						return songs[2];
					}
				}
				else if (current == PlayGround.MML)
				{
					if (currentA == Area.Alto || currentA == Area.Baritone || currentA == Area.Tenor)
					{
						return songs[8];
					}
					else
					{
						return songs[2];
					}
				}
				else if (current == PlayGround.BRRRGH)
				{
					if (currentA == Area.Walrus || currentA == Area.Sleet || currentA == Area.Polar)
					{
						return songs[10];
					}
					else
					{
						return songs[2];
					}
				}
				else if (current == PlayGround.DDL)
				{
					if (currentA == Area.Lullaby || currentA == Area.Pajama)
					{
						return songs[12];
					}
					else
					{
						return songs[2];
					}
				}
				else if (current == PlayGround.SBHQ)
				{
					if (currentA == Area.SBHQCourt || currentA == Area.SBHQFactoryExterior || currentA == Area.VP)
					{
						return songs[13];
					}
					else if(currentA == Area.SBHQFrontFactory || currentA == Area.SBHQSideFactory)
					{
						return songs[14];
					}
				}
				else if (current == PlayGround.CBHQ)
				{
					if (currentA == Area.CBHQTrainyard || currentA == Area.CFO)
					{
						return songs[13];
					}
					else
					{
						return songs[14];
					}
				}
				else if (current == PlayGround.LBHQ)
				{
					if (currentA == Area.LBHQCourtyard)
					{
						return songs[15];
					}
					else if (currentA == Area.LBHQOfficeA || currentA == Area.LBHQOfficeB || currentA == Area.LBHQOfficeC || currentA == Area.LBHQOfficeD) 
					{
						return songs[14];
					}
					else
					{
						return songs[16];
					}
				}
				else
				{
					
					if (currentA == Area.BBHQFront || currentA == Area.BBHQMiddle || currentA == Area.BBHQBack)
					{
						Random ran = new();
						int random = ran.Next(0, 3);
						if (random == 0)
						{
							return songs[20];
						}
						else if (random == 1)
						{
							return songs[21];
						}
						else
						{
							return songs[22];
						}
					}
					else
					{
						return songs[23];
					}
				}
			}
			return songs[0];
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
			else 
			{
				if (current == PlayGround.TTC)
				{
					if (currentA == Area.Loopy)
					{
						return 1;
					}
					else if (currentA == Area.Punchline)
					{
						return 2;
					}
					else if (currentA == Area.Silly)
					{
						return 3;
					}
					else 
					{
						return 4;
					}
				}
				else if (current == PlayGround.DD)
				{
					if (currentA == Area.Barnacle)
					{
						return 6;
					}
					else if (currentA == Area.Lighthouse)
					{
						return 7;
					}
					else if (currentA == Area.Seaweed)
					{
						return 8;
					}
					else
					{
						return 9;
					}
				}
				else if (current == PlayGround.DG)
				{
					if (currentA == Area.Elm)
					{
						return 11;
					}
					else if (currentA == Area.Maple)
					{
						return 12;
					}
					else if (currentA == Area.Oak)
					{
						return 13;
					}
					else
					{
						return 14;
					}
				}
				else if (current == PlayGround.MML)
				{
					if (currentA == Area.Alto)
					{
						return 16;
					}
					else if (currentA == Area.Baritone)
					{
						return 17;
					}
					else if (currentA == Area.Tenor)
					{
						return 18;
					}
					else
					{
						return 19;
					}
				}
				else if (current == PlayGround.BRRRGH)
				{
					if (currentA == Area.Walrus)
					{
						return 21;
					}
					else if (currentA == Area.Sleet)
					{
						return 22;
					}
					else if (currentA == Area.Polar)
					{
						return 23;
					}
					else
					{
						return 24;
					}
				}
				else if (current == PlayGround.DDL)
				{
					if (currentA == Area.Lullaby)
					{
						return 26;
					}
					else if (currentA == Area.Pajama) 
					{
						return 27;
					}
					else 
					{
						return 28;
					}
				}
				else if (current == PlayGround.SBHQ)
				{
					if (currentA == Area.SBHQCourt)
					{
						return 29;
					}
					else if (currentA == Area.SBHQFactoryExterior)
					{
						return 30;
					}
					else if (currentA == Area.SBHQFrontFactory || currentA == Area.SBHQSideFactory)
					{
						return 31;
					}
					else
					{
						return 32;
					}
				}
				else if (current == PlayGround.CBHQ)
				{
					if (currentA == Area.CBHQTrainyard)
					{
						return 33;
					}
					else if (currentA == Area.CBHQCoin || currentA == Area.CBHQDollar || currentA == Area.CBHQBullion)
					{
						return 34;
					}
					else 
					{
						return 35;
					}
				}
				else if (current == PlayGround.LBHQ)
				{
					if (currentA == Area.LBHQCourtyard)
					{
						return 36;
					}
					else if (currentA == Area.LBHQOfficeA || currentA == Area.LBHQOfficeB || currentA == Area.LBHQOfficeC || currentA == Area.LBHQOfficeD)
					{
						return 37;
					}
					else 
					{
						return 38;
					}
				}
				else
				{
					if (currentA == Area.BBHQFront || currentA == Area.BBHQMiddle || currentA == Area.BBHQBack)
					{
						return 40;
					}
					else 
					{
						return 41;
					}
				}
			}
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
			Rectangle buttonBackSource;
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

			if (collidingBackButton == false)
			{
				buttonBackSource = new Rectangle(389, 265, 118, 52);
			}
			else
			{
				buttonBackSource = new(263, 265, 118, 52);
			}

			spriteBatch.Draw(backgrounds[i], Vector2.Zero, destinationRectangle, Color.White);


			spriteBatch.Draw(overlay, Vector2.Zero, destinationRectangle, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);

			if (showBackButton) 
			{
				spriteBatch.Draw(buttons, destination - new Vector2(250,0), buttonBackSource, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
				spriteBatch.DrawString(font, "Back", destination - new Vector2(200, 0), Color.Black);
			}

			spriteBatch.Draw(buttons, destination, buttonSource, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

			spriteBatch.DrawString(font, "Select", destination + new Vector2(10, 0), Color.Black);


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
			if (selectingPlayground)
			{
				spriteBatch.DrawString(font, playgroundDescriptions[(int)current], destination, Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
			}
			else 
			{
				spriteBatch.DrawString(font, areaDescriptions[(int)currentA], destination, Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
			}

			Rectangle exitButtonSource;

			if (collidingExitButton == false)
			{
				exitButtonSource = new Rectangle(254, 0, 125, 125);
			}
			else 
			{
				exitButtonSource = new Rectangle(132, 0, 125, 125);
			}

			spriteBatch.Draw(exitButton, Vector2.Zero, exitButtonSource, Color.White);

			spriteBatch.End();

		}
	}
}
