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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json.Serialization;

namespace TTBattleSim.Rooms
{
	public enum gagTypes
	{
		trap = 0,
		lure,
		sound,
		throw_,
		squirt,
		drop,
		pass
	}

	public class BattleScreen_TTO : GameScreen
	{

		Game game;

		Song[] songs = new Song[9];

		Texture2D[] backgrounds = new Texture2D[21];

		Texture2D gear;

		Texture2D exitButton;

		Texture2D stats;

		Texture2D toonIcons;

		Texture2D GagSelection;

		Texture2D gags1;

		Texture2D[] gagIcons = new Texture2D[42];

		int currentLevel;

		int currentCog;

		string[] gags = { "Trap", "Lure", "Sound", "Throw", "Squirt", "Drop" };

		string[] levels = { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6" };

		string[] descriptions = { "Traps a cog\nin order to\nspring the \ntrap you\nwill need\nto lure a\ncog.\nDON'T DOUBLE\nTRAP!", 
			"Brings the \ncog forward\nto stun it.\nAllows traps \nand allows\nfor knock\nback bonuses \non Throw \nand Squirt", 
			"An attack\nthat hits\nall COGS.\nLow damage\nand unlures\nlured cogs.", 
			"An attack\nthat does\nmedium damage\non a\nsingle cog.\nCombine with\nlure for\nmassive\ndamage.",
			"An attack\nthat does\nlow damage\non a\nsingle cog.\nCombine with\nlure for\nmore damage.", 
			"An attack\nthat does\nhigh damage\non a\nsingle cog.\nMisses on\nlured cogs.", "Passes for\nthis toon."
		};

		string[] trapDescriptions = { "Banana Peel\nDamage: 12",
			"Rake\nDamage: 20",
			"Marbles\nDamage: 35",
			"Quicksand\nDamage: 50",
			"Trap Door\nDamage: 70",
			"TNT\nDamage: 180"};

		string[] lureDescriptions = { "$1 bill",
			"Small Magnet",
			"$5 bill",
			"Big Magnet",
			"$10 bill",
			"Hypno Goggles" };

		string[] soundDescriptions = { "Bike Horn\nDamage: 4",
			"Whistle\nDamage: 7",
			"Bugle\nDamage: 11",
			"Aoogah\nDamage: 16",
			"Elephant Trunk\nDamage: 21",
			"Fog Horn\nDamage: 50"};

		string[] throwDescriptions = { "Cupcake\nDamage: 6",
			"Fruit Slice\nDamage: 10",
			"Cream Slice\nDamage: 17",
			"Fruit Pie\nDamage: 27",
			"Cream Pie\nDamage: 40",
			"BDay Cake\nDamage: 100"};

		string[] squirtDescriptions = { "Squirt Flower\nDamage: 4",
			"Glass\nDamage: 8",
			"Squirt Gun\nDamage: 12",
			"Seltzer\nDamage: 21",
			"Fire Hose\nDamage: 30",
			"Storm Cloud\nDamage: 80"};

		string[] dropDescriptions = { "Flower pot\nDamage: 10",
			"Sandbag\nDamage: 18",
			"Anvil\nDamage: 30",
			"Big Weight\nDamage: 45",
			"Safe\nDamage: 60",
			"Grand Piano\nDamage: 170"};

		Color[] colors = { Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Pink, Color.LightBlue };

		BoundingRectangle mouse = new(0, 0, 32, 32);

		public BoundingRectangle Bounds => mouse;

		BoundingRectangle[] trackSelection = new BoundingRectangle[7];

		BoundingRectangle[] levelSelection = new BoundingRectangle[6];

		BoundingRectangle[] cogSelection = new BoundingRectangle[4];

		string fileName = Environment.CurrentDirectory + "\\ToonList.txt";

		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		CustomColor customColor;

		ContentManager _content;


		BoundingRectangle exit;

		bool[] toonSelecting = { true, false, false, false };

		bool[] whichDescription = { false, false, false, false, false, false, false };

		bool trackSelecting = true;

		bool levelSelecting;

		bool cogSelecting;

		COG[] cogs = new COG[4];

		Toon[] party = new Toon[4];

		int song;

		int back;

		private SoundEffect option;

		SpriteFont font;

		Area currentA;

		int i = 0;


		public BattleScreen_TTO(Game game, int songToPlay, int background, Area area, COG[] cogs)
		{
			this.game = game;
			song = songToPlay;
			back = background;
			this.cogs = cogs;
			currentA = area;

			if (File.Exists(fileName))
			{
				string[] line;
				StreamReader reader = new(fileName);
				for (int i = 0; i < 4; i++)
				{
					line = reader.ReadLine().Split(",");
					party[i] = new Toon();
					party[i].name = line[0];
					party[i].species = (ToonSpecies)(Convert.ToInt32(line[1]));
					party[i].maximumHP = (Convert.ToInt32(line[2]));
					for (int j = 0; j < 7; j++)
					{
						bool gagCheck = false;
						if (line[3 + j] == "true")
						{
							gagCheck = true;
						}
						party[i].gagTracks[j] = gagCheck;
						party[i].gagLevels[j] = Convert.ToInt32(line[10 + j]);
					}
					party[i].color = (ToonColors)Convert.ToInt32(line[17]);
				}
				reader.Close();
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					party[i] = new Toon();
					if (i == 0)
					{
						party[i].name = "Aidan";
						party[i].species = ToonSpecies.mouse;
						party[i].color = ToonColors.Blue;
					}
					else if (i == 1)
					{
						party[i].name = "Floppy";
						party[i].species = ToonSpecies.dog;
						party[i].color = ToonColors.Aqua;
					}
					else if (i == 2)
					{
						party[i].name = "Toony Stank";
						party[i].species = ToonSpecies.bear;
						party[i].color = ToonColors.Seagreen;
					}
					else
					{
						party[i].name = "Korhi";
						party[i].species = ToonSpecies.cat;
						party[i].color = ToonColors.Orange;
					}
					party[i].maximumHP = 137;
					for (int j = 0; j < 7; j++)
					{
						party[i].gagTracks[j] = true;
						party[i].gagLevels[j] = 7;
					}
				}
			}
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

			customColor = new();
			exitButton = _content.Load<Texture2D>("Textures/phase_3_palette_4alla_1");
			GagSelection = _content.Load<Texture2D>("Textures/gag_selection_panels_palette_4allc_1");
			gags1 = _content.Load<Texture2D>("Textures/gag_selection_panels_palette_4allc_3");
			toonIcons = _content.Load<Texture2D>("Textures/gamegui_palette_2tlla_1");
			gear = _content.Load<Texture2D>("Textures/cog");
			stats = _content.Load<Texture2D>("Textures/stats");
			option = _content.Load<SoundEffect>("SoundEffects/Generic/Select");
			font = _content.Load<SpriteFont>("menuFont");
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(songs[song]);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			pastMousePosition = currentMousePosition;
			currentMousePosition = Mouse.GetState();

			exit = new BoundingRectangle(0, 0, 125, 125);
			//text = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width - 500), (ScreenManager.GraphicsDevice.Viewport.Height - 100), 118 * 2, 52 * 2);
			Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);


			if (trackSelecting)
			{
				for (int i = 0; i < 6; i++)
				{
					if (i < 3)
					{
						trackSelection[i] = new((game.GraphicsDevice.Viewport.Width - 750) / 2, (game.GraphicsDevice.Viewport.Height - (500 - i * 250)) / 2, 200, 100);
					}
					else
					{
						trackSelection[i] = new((game.GraphicsDevice.Viewport.Width) / 2, (game.GraphicsDevice.Viewport.Height - (500 - (i - 3) * 250)) / 2, 200, 100);
					}
				}
				trackSelection[6] = new((game.GraphicsDevice.Viewport.Width + 1000) / 2, (game.GraphicsDevice.Viewport.Height + 250) / 2, 200, 100);
				for (int i = 0; i < 7; i++)
				{
					if (mouse.collidesWith(trackSelection[i]))
					{
						whichDescription[i] = true;
						for (int j = 0; j < 6; j++)
						{
							if (mouse.collidesWith(trackSelection[j]) && j != i)
							{
								whichDescription[j] = false;
							}
						}
						if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
						{
							option.Play();
							party[0].gag = (gagTypes)i;
							if (party[0].gag == gagTypes.pass)
							{
								trackSelecting = true;
							}
							else
							{
								trackSelecting = false;
								levelSelecting = true;
							}
						}
					}
					else
					{
						whichDescription[i] = false;
					}
				}
			}
			else if (levelSelecting)
			{
				for (int i = 0; i < 6; i++)
				{
					if (i < 3)
					{
						levelSelection[i] = new((game.GraphicsDevice.Viewport.Width - 750) / 2, (game.GraphicsDevice.Viewport.Height - (500 - i * 250)) / 2, 200, 100);
					}
					else
					{
						levelSelection[i] = new((game.GraphicsDevice.Viewport.Width) / 2, (game.GraphicsDevice.Viewport.Height - (500 - (i - 3) * 250)) / 2, 200, 100);
					}
				}

				for (int i = 0; i < 6; i++)
				{
					if (mouse.collidesWith(levelSelection[i]))
					{
						currentLevel = i;

						if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
						{
							option.Play();
							levelSelecting = false;
							if (party[0].gag == gagTypes.lure && (currentLevel + 1) % 2 == 0)
							{
								attackCogs();
								trackSelecting = true;
							}
							else if (party[0].gag == gagTypes.sound) 
							{
								attackCogs();
								trackSelecting = true;
							}
							else
							{
								cogSelecting = true;
							}
						}
					}
					else
					{
						whichDescription[i] = false;
					}
				}
			}
			else 
			{
				for (int i = 0; i < 4; i++)
				{
					if (i < 2)
					{
						cogSelection[i] = new((game.GraphicsDevice.Viewport.Width - 750) / 2, (game.GraphicsDevice.Viewport.Height - (500 - i * 250)) / 2, 200, 100);
					}
					else
					{
						cogSelection[i] = new((game.GraphicsDevice.Viewport.Width) / 2, (game.GraphicsDevice.Viewport.Height - (500 - (i - 2) * 250)) / 2, 200, 100);
					}
				}

				for (int i = 0; i < 4; i++)
				{
					if (mouse.collidesWith(cogSelection[i]))
					{
						if (!cogs[i].isTrapped && party[0].gag == gagTypes.trap)
						{
							currentCog = i;
						}
						else if (!cogs[i].isLured && party[0].gag == gagTypes.lure)
						{
							currentCog = i;
						}
						else if (party[0].gag == gagTypes.throw_ || party[0].gag == gagTypes.squirt || party[0].gag == gagTypes.drop) 
						{
							currentCog = i;
						}

						if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
						{
							option.Play();

							attackCogs();

							trackSelecting = true;
							cogSelecting = false;
						}
					}
					else
					{
						whichDescription[i] = false;
					}
				}
			}

			if (mouse.collidesWith(exit))
			{
				if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
				{
					foreach (var screen in ScreenManager.GetScreens())
						screen.ExitScreen();

					ScreenManager.AddScreen(new TTO_LevelSelect(game, (PlayGround)song), null);

					option.Play();
				}
			}

			mouse.X = mousePosition.X;
			mouse.Y = mousePosition.Y;
		}

		private void attackCogs() 
		{
			if (party[0].gag == gagTypes.sound) 
			{
				int damage = 0;
				currentLevel += 1;
				if (currentLevel == 1)
				{
					damage = 4;
				}
				else if (currentLevel == 2)
				{
					damage = 7;
				}
				else if (currentLevel == 3)
				{
					damage = 11;
				}
				else if (currentLevel == 4)
				{
					damage = 16;
				}
				else if (currentLevel == 5)
				{
					damage = 21;
				}
				else 
				{
					damage = 50;
				}
				for (int i = 0; i < cogs.Length; i++) 
				{
					cogs[i].HP -= damage;
				}
			}
			for (int i = 0; i < cogs.Length; i++) 
			{
				if (cogs[i].HP <= 0) 
				{
					CogController controller = new((PlayGround)song, currentA);
					cogs[i] = controller.GenerateCog();
				}
			}
		}

		private Rectangle getSourceToonIcons(ToonSpecies species)
		{
			if (species == ToonSpecies.dog)
			{
				return new Rectangle(384, 0, 128, 128);
			}
			else if (species == ToonSpecies.cat)
			{
				return new Rectangle(260, 0, 128, 128);
			}
			else if (species == ToonSpecies.duck)
			{
				return new Rectangle(260, 130, 128, 128);
			}
			else if (species == ToonSpecies.horse)
			{
				return new Rectangle(384, 130, 128, 128);
			}
			else if (species == ToonSpecies.rabbit)
			{
				return new Rectangle(260, 254, 128, 128);
			}
			else if (species == ToonSpecies.bear)
			{
				return new Rectangle(384, 254, 128, 128);
			}
			else if (species == ToonSpecies.monkey)
			{
				return new Rectangle(0, 254, 128, 128);
			}
			else if (species == ToonSpecies.mouse)
			{
				return new Rectangle(130, 254, 128, 128);
			}
			else
			{
				return new Rectangle(0, 387, 128, 128);
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

			Rectangle toonStatsSource = new Rectangle(18, 785, 219, 97);

			Color status;

			spriteBatch.Begin();
			Vector2 destination = new Vector2((graphics.Viewport.Width - 500), (graphics.Viewport.Height - 100));
			var destinationRectangle = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);

			spriteBatch.Draw(backgrounds[back], Vector2.Zero, destinationRectangle, Color.White);

			for (int i = 0; i < cogs.Length; i++) 
			{
				spriteBatch.Draw(gear, new Vector2((graphics.Viewport.Width - 1250 + (1550 * i)) / 4, 50), null, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
				spriteBatch.Draw(stats, new Vector2((graphics.Viewport.Width - 1000 + (1550 * i)) / 4, 50), null, Color.White, 0f, Vector2.Zero, new Vector2(1.25f, 1f), SpriteEffects.None, 0);
				if (cogs[i].isLured)
				{
					status = Color.Green;
				}
				else if (cogs[i].isTrapped)
				{
					status = Color.Red;
				}
				else 
				{
					status = Color.Black;
				}
				spriteBatch.DrawString(font, (i+1) + ": " + cogs[i].Name + "\nLevel " + cogs[i].Level + "\nHP: " + cogs[i].HP + "/" + cogs[i].floorHP, new Vector2((graphics.Viewport.Width - 1000 + (1550 * i)) / 4, 50), status, 0f, Vector2.Zero, .45f, SpriteEffects.None, 0);
			}


			for (int i = 0; i < party.Length; i++)
			{
				Rectangle iconSource = getSourceToonIcons(party[i].species);
				spriteBatch.Draw(gags1, new Vector2((graphics.Viewport.Width - 1000 + (1550 * i)) / 4, graphics.Viewport.Height - 200), toonStatsSource, Color.White, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
				spriteBatch.Draw(toonIcons, new Vector2((graphics.Viewport.Width - 1300 + (1550 * i)) / 4, graphics.Viewport.Height - 200), iconSource, customColor.customToonColors[(int)party[i].color]);
				if (toonSelecting[i]) 
				{
					spriteBatch.DrawString(font, "Selecting", new Vector2((graphics.Viewport.Width - 900 + (1550 * i)) / 4, graphics.Viewport.Height - 250), Color.Red, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
				}
				spriteBatch.DrawString(font, party[i].name + "\nHP: " + party[i].maximumHP, new Vector2((graphics.Viewport.Width - 775 + (1550 * i)) / 4, graphics.Viewport.Height - 175), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
			}

			spriteBatch.Draw(GagSelection, new Vector2((graphics.Viewport.Width - 1500) / 2, (graphics.Viewport.Height - 600) / 2), new Rectangle(0, 75, 1024, 365), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);


			if (trackSelecting)
			{
				for (int i = 0; i < 6; i++)
				{
					Vector2 position = Vector2.Zero;
					if (i < 3)
					{
						position = new Vector2((graphics.Viewport.Width - 750) / 2, (graphics.Viewport.Height - (500 - i * 250)) / 2);
					}
					else 
					{
						position = new Vector2((graphics.Viewport.Width) / 2, (graphics.Viewport.Height - (500 - (i - 3) * 250)) / 2);
					}
					spriteBatch.DrawString(font, gags[i], position, colors[i], 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
				}
				spriteBatch.DrawString(font, "Pass", new Vector2((game.GraphicsDevice.Viewport.Width + 1000) / 2, (game.GraphicsDevice.Viewport.Height + 250) / 2), Color.BlueViolet, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
				for (int i = 0; i < whichDescription.Length; i++) 
				{
					if (whichDescription[i])
					{
						spriteBatch.DrawString(font, descriptions[i], new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
					}
				}
			}

			if (levelSelecting)
			{
				for (int i = 0; i < 6; i++)
				{
					Vector2 position = Vector2.Zero;
					if (i < 3)
					{
						position = new Vector2((graphics.Viewport.Width - 750) / 2, (graphics.Viewport.Height - (500 - i * 250)) / 2);
					}
					else
					{
						position = new Vector2((graphics.Viewport.Width) / 2, (graphics.Viewport.Height - (500 - (i - 3) * 250)) / 2);
					}
					spriteBatch.DrawString(font, levels[i], position, Color.Black, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
				}
		

							if (party[0].gag == gagTypes.trap)
							{
								spriteBatch.DrawString(font, trapDescriptions[currentLevel], new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
							}
							if (party[0].gag == gagTypes.lure)
							{
								spriteBatch.DrawString(font, lureDescriptions[currentLevel], new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
							}
							if (party[0].gag == gagTypes.sound)
							{
								spriteBatch.DrawString(font, soundDescriptions[currentLevel], new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
							}
							if (party[0].gag == gagTypes.throw_)
							{
								spriteBatch.DrawString(font, throwDescriptions[currentLevel], new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
							}
							if (party[0].gag == gagTypes.squirt)
							{
								spriteBatch.DrawString(font, squirtDescriptions[currentLevel], new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
							}
							if (party[0].gag == gagTypes.drop)
							{
								spriteBatch.DrawString(font, dropDescriptions[currentLevel], new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
							}
			}

			if (cogSelecting)
			{
				for (int i = 0; i < 4; i++)
				{
					Vector2 position = Vector2.Zero;
					if (i < 2)
					{
						position = new Vector2((graphics.Viewport.Width - 750) / 2, (graphics.Viewport.Height - (500 - i * 250)) / 2);
					}
					else
					{
						position = new Vector2((graphics.Viewport.Width) / 2, (graphics.Viewport.Height - (500 - (i - 2) * 250)) / 2);
					}
					spriteBatch.DrawString(font, "Cog " + (i + 1), position, Color.Black, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
				}


				if (currentCog == 0)
				{
					spriteBatch.DrawString(font, "x---", new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
				}
				else if (currentCog == 1) 
				{
					spriteBatch.DrawString(font, "-x--", new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
				}
				else if (currentCog == 2)
				{
					spriteBatch.DrawString(font, "--x-", new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
				}
				else if (currentCog == 3)
				{
					spriteBatch.DrawString(font, "---x", new Vector2((graphics.Viewport.Width - 1450) / 2, (graphics.Viewport.Height - 500) / 2), Color.Black, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
				}

			}

			Rectangle exitButtonSource = new Rectangle(254, 0, 125, 125);

			spriteBatch.Draw(exitButton, Vector2.Zero, exitButtonSource, Color.White);

			spriteBatch.End();

		}

	}
}
