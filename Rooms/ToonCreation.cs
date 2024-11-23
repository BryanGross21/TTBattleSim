using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using TTBattleSim.Collisions;
using TTBattleSim.Entities;
using TTBattleSim.StateManagement;
using static System.Net.Mime.MediaTypeNames;

namespace TTBattleSim.Rooms
{
	enum screens 
	{
		slots = 0,
		species,
		color,
		name,
		stats
	}
	public class ToonCreation : GameScreen
	{
		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		string fileName = Environment.CurrentDirectory + "\\ToonList.txt";

		string[] titles = { "Choose a Slot to Create/Edit!", "Choose a species!", "Choose your Color!", "Pick a Name!", "Set your Stats!" };

		string[] titleNames = { "Aunt", "Baron", "Captain", "Colonel", "Cool", "Crazy", "Deputy", "Dippy", "Doctor", "Duke", "Fat", "Good Ol'", "Granny", "King", "Lady", "Little", "Loopy", "Loud", "Master", "Miss", "Mister", "Noisy", "Prince", "Princess", "Prof.", "Queen", "Sheriff", "Silly", "Sir", "Skinny", "Super", "Ugly", "Weird" };

		string[] firstNames = { "Alvin", "Astro", "B.D.", "Banjo", "Barney", "Bart", "Batty", "Beany", "Bebop", "Beppo", "Bert", "Bingo", "Binky", "Biscuit", "Bizzy", "Blinky", "Bongo", "Bonkers", "Bonnie", "Bonzo", "Boo Boo", "Bouncey", "Bubbles", "Buford", "Bumpy", "Bunky", "Buster", "Butch", "Buzz", "C.J.", "C.W.", "Candy", "Cecil", "Chester", "Chip", "Chipper", "Chirpy", "Chunky", "Clancy", "Clarence", "Cliff", "Clover", "Clyde", "Coconut", "Comet", "Corky", "Corny", "Cranky", "Crazy", "Cricket", "Crumbly", "Cuckoo", "Cuddles", "Curly", "Daffodil", "Daffy", "Daphne", "Dee Dee", "Dinky", "Dizzy", "Domino", "Dottie", "Drippy", "Droopy", "Dudley", "Duke", "Dusty", "Dynamite", "Ernie", "Fancy", "Fangs", "Felix", "Fireball", "Flapjack", "Flappy", "Fleabag", "Flip", "Fluffy", "Freckles", "Fritz", "Frizzy", "Furball", "Ginger", "Goopy", "Gwen", "Harvey", "Hector", "Huddles", "Huey", "J.C.", "Jacques", "Jake", "Jazzy", "Jellyroll", "Kippy", "Kit", "Knuckles", "Ladybug", "Lancelot", "Lefty", "Leroy", "Lily", "Lionel", "Lloyd", "Lollipop", "Loony", "Loopy", "Louie", "Lucky", "Mac", "Marigold", "Max", "Maxie", "Melody", "Mildew", "Milton", "Moe", "Monty", "Mo Mo", "Murky", "Ned", "Nutmeg", "Nutty", "Olive", "Orville", "Oscar", "Oswald", "Ozzie", "Pancake", "Peaches", "Peanut", "Pearl", "Penny", "Peppy", "Petunia", "Pickles", "Pierre", "Pinky", "Popcorn", "Poppy", "Presto", "Rainbow", "Raven", "Reggie", "Rhubarb", "Ricky", "Rocco", "Robin", "Rollie", "Romeo", "Rosie", "Roxy", "Rusty", "Sadie", "Sally", "Salty", "Sammie", "Sandy", "Scooter", "Skids", "Skimpy", "Skip", "Skipper", "Skippy", "Slappy", "Slippy", "Slumpy", "Smirky", "Snappy", "Sniffy", "Snuffy", "Soupy", "Spiffy", "Spike", "Spotty", "Spunky", "Squeaky", "Star", "Stripey", "Stubby", "Taffy", "Teddy", "Tom", "Tricky", "Trixie", "Tubby", "Ursula", "Valentine", "Vicky", "Violet", "Von", "Wacko", "Wacky", "Waldo", "Whiskers", "Willow", "Wilbur", "Winky", "Yippie", "Z.Z.", "Zany", "Ziggy", "Zilly", "Zippety", "Zippy", "Zowie" };

		string[] lastFirst = { "Bagel", "Banana", "Bean", "Beanie", "Biggen", "Bizzen", "Blubber", "Boingen", "Bumber", "Bumble", "Bumpen", "Cheezy", "Crinkle", "Crumble", "Crunchen", "Crunchy", "Dandy", "Dingle", "Dizzen", "Dizzy", "Doggen", "Dyno", "Electro", "Feather", "Fiddle", "Fizzle", "Flippen", "Flipper", "Frinkel", "Fumble", "Funny", "Fuzzy", "Giggle", "Glitter", "Google", "Grumble", "Gumdrop", "Huckle", "Hula", "Jabber", "Jeeper", "Jinx", "Jumble", "Kooky", "Lemon", "Loopen", "Mac", "Mc", "Mega", "Mizzen", "Nickel", "Nutty", "Octo", "Paddle", "Pale", "Pedal", "Pepper", "Petal", "Pickle", "Pinker", "Poodle", "Poppen", "Precious", "Pumpkin", "Purple", "Rhino", "Robo", "Rocken", "Ruffle", "Smarty", "Sniffle", "Snorkel", "Sour", "Spackle", "Sparkle", "Squiggle", "Super", "Thunder", "Toppen", "Tricky", "Tweedle", "Twiddle", "Twinkle", "Wacky", "Weasel", "Whisker", "Whistle", "Wild", "Witty", "Wonder", "Wrinkle", "Ziller", "Zippen", "Zooble" };

		string[] lastSecond = { "Bee", "Berry", "Blabber", "Bocker", "Boing", "Boom", "Bounce", "Bouncer", "Brains", "Bubble", "Bumble", "Bump", "Bumper", "Chomp", "Corn", "Crash", "Crumbs", "Crump", "Crunch", "Doodle", "Dorf", "Face", "Fidget", "Fink", "Fish", "Flap", "Flapper", "Flinger", "Flip", "Flipper", "Foot", "Fuddy", "Fussen", "Gadget", "Gargle", "Gloop", "Glop", "Goober", "Goose", "Grooven", "Hoffer", "Hopper", "Jinks", "Klunk", "Knees", "Marble", "Mash", "Monkey", "Mooch", "Mouth", "Muddle", "Muffin", "Mush", "Nerd", "Noodle", "Nose", "Nugget", "Phew", "Phooey", "Pocket", "Poof", "Pop", "Pounce", "Pow", "Pretzel", "Quack", "Roni", "Scooter", "Screech", "Smirk", "Snooker", "Snoop", "Snout", "Socks", "Speed", "Spinner", "Splat", "Sprinkles", "Sticks", "Stink", "Swirl", "Teeth", "Thud", "Toes", "Ton", "Toon", "Tooth", "Twist", "Whatsit", "Whip", "Wig", "Woof", "Zaner", "Zap", "Zapper", "Zilla", "Zoom" };

		int title = 0;

		int first = 0;

		int lastF = 0;

		int lastS = 0;

		bool titleSelected;

		bool firstSelected;

		bool lastSelected;

		Toon[] party = new Toon[4];

		Song MakeAToon;

		Texture2D[] backgrounds = new Texture2D[5];

		Texture2D arrowButton;

		Texture2D nameArrows;

		Texture2D toonIcons;

		Texture2D slotOverlay;

		Texture2D selectedButton;

		Texture2D colorOption;

		Texture2D namePlate;



		StringBuilder sr = new();

		bool[] slotSelected = { true, false, false, false };


		bool[] speciesSelected = { true, false, false, false, false, false, false, false, false };

		BoundingRectangle mouse = new(0, 0, 32, 32);

		public BoundingRectangle Bounds => mouse;

		BoundingRectangle leftArrow;

		BoundingRectangle rightArrow;

		BoundingRectangle[] slots = new BoundingRectangle[4];

		BoundingRectangle[] species = new BoundingRectangle[9];

		BoundingRectangle[] colors = new BoundingRectangle[27];

		BoundingRectangle[] nameSelect = new BoundingRectangle[3];

		Toon currentToon;

		bool collidingLeftArrowButton;

		bool collidingRightArrowButton;

		PlayGround groundToReturn;

		screens currentScreen = screens.slots; 

		SpriteFont font;

		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		ContentManager _content;

		private SoundEffect option;
		private SoundEffect selected;

		CustomColor customColor;

		public ToonCreation(Game game, PlayGround ground) 
		{
			this.game = game;

			groundToReturn = ground;
		}

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			backgrounds[0] = _content.Load<Texture2D>("Textures/ttc");
			backgrounds[1] = _content.Load<Texture2D>("Textures/DD");
			backgrounds[2] = _content.Load<Texture2D>("Textures/DG");
			backgrounds[3] = _content.Load<Texture2D>("Textures/MML");
			backgrounds[4] = _content.Load<Texture2D>("Textures/DDL");

			MakeAToon = _content.Load<Song>("TTOMusic/TTC/MakeAToon");

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
					party[i].name = "Toon";
					party[i].species = ToonSpecies.dog;
					party[i].maximumHP = 137;
					for (int j = 0; j < 7; j++)
					{
						party[i].gagTracks[j] = true;
						party[i].gagLevels[j] = 1;
					}
					party[i].color = ToonColors.White;
				}
			}

			for (int k = 0; k < 4; k++) { 
				slots[k] = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width - 500) / 2, (ScreenManager.GraphicsDevice.Viewport.Height + k * 500) / 4, 512, 128);
			}

			int column = 0;
			int row = 0;

			for (int k = 0; k < 9; k++)
			{
				species[k] = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width + 400 + (250 * column)) / 2,((ScreenManager.GraphicsDevice.Viewport.Height - 100 + (row * 500)) / 4), 128, 128);
				column++;
				if (column == 3)
				{
					column = 0;
					row++;
				}
			}

			column = 0;
			row = 0;

			for (int k = 0; k < 27; k++)
			{
				colors[k] = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width + 400 + (250 * column)) / 2, ((ScreenManager.GraphicsDevice.Viewport.Height - 100 + (row * 275)) / 4), 64, 64);
				column++;
				if (column == 3)
				{
					column = 0;
					row++;
				}
			}
			row = 0;
			nameSelect[0] = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width - 900) / 2, (ScreenManager.GraphicsDevice.Viewport.Height + 190) / 4, 128, 128);
			nameSelect[1] = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width - 400) / 2, (ScreenManager.GraphicsDevice.Viewport.Height + 190) / 4, 128, 128);
			nameSelect[2] = new BoundingRectangle((ScreenManager.GraphicsDevice.Viewport.Width + 350) / 2, (ScreenManager.GraphicsDevice.Viewport.Height + 190) / 4, 128, 128);

			customColor = new();
			currentToon = party[0];

			leftArrow = new BoundingRectangle(50, (ScreenManager.Game.GraphicsDevice.Viewport.Height - 200), 127 * 1.5f, 127 * 1.5f);
			rightArrow = new BoundingRectangle((ScreenManager.Game.GraphicsDevice.Viewport.Width - 250), (ScreenManager.Game.GraphicsDevice.Viewport.Height - 200), 127 * 1.5f, 127 * 1.5f);

			nameArrows = _content.Load<Texture2D>("Textures/makeatoon_palette_4alla_2");
			namePlate = _content.Load<Texture2D>("Textures/tt_t_gui_mat_namePanel");
			slotOverlay = _content.Load<Texture2D>("Textures/Toon_Slot");
			option = _content.Load<SoundEffect>("SoundEffects/Generic/Next");
			selected = _content.Load<SoundEffect>("SoundEffects/Generic/Select");
			toonIcons = _content.Load<Texture2D>("Textures/gamegui_palette_2tlla_1");
			colorOption = _content.Load<Texture2D>("Textures/particleGlow");
			selectedButton = _content.Load<Texture2D>("Textures/makeatoon_palette_4alla_2");
			arrowButton = _content.Load<Texture2D>("Textures/makeatoon_palette_4alla_2");
			font = _content.Load<SpriteFont>("menuFont");
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(MakeAToon);
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

			if (mouse.collidesWith(leftArrow))
			{
				collidingLeftArrowButton = true;
				collidingRightArrowButton = false;
				if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
				{
					currentScreen--;
					if ((int)currentScreen < 0) 
					{

						using (StreamWriter sw = new StreamWriter(fileName, false)) // 'false' to overwrite the file
						{
							for (int i = 0; i < 4; i++) 
							{
								sr.Append(party[i].name + "," + (int)party[i].species + ",");
								sr.Append(party[i].maximumHP + ",");
								for (int j = 0; j < 7; j++)
								{
									sr.Append(party[i].gagTracks[j] + ",");
								}
								for (int j = 0; j < 7; j++)
								{
									sr.Append(party[i].gagLevels[j] + ",");
								}
								sr.Append((int)party[i].color);
								string line = sr.ToString();
								sw.WriteLine(line);
								sr.Clear();
							}
						}

						foreach (var screen in ScreenManager.GetScreens())
							screen.ExitScreen();

						ScreenManager.AddScreen(new TTO_LevelSelect(game, groundToReturn), null);

					}
					else	
					{
						option.Play();
					}
					selected.Play();
				}
			}
			else if (mouse.collidesWith(rightArrow))
			{
				collidingLeftArrowButton = false;
				collidingRightArrowButton = true;
				if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
				{
					currentScreen++;
					if ((int)currentScreen == 3) 
					{
						currentScreen = 0;
					}
					option.Play();
					selected.Play();
				}
			}
			else
			{
				collidingLeftArrowButton = false;
				collidingRightArrowButton = false;
			}

			if (currentScreen == screens.slots)
			{
				for (int j = 0; j < 9; j++)
				{
					if (j == 0)
					{
						speciesSelected[j] = true;
					}
					else
					{
						speciesSelected[j] = false;
					}
				}

				for (int i = 0; i < 4; i++)
				{
					if (mouse.collidesWith(slots[i]) && slotSelected[i] == false)
					{
						if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
						{
							currentToon = party[i];
							for (int j = 0; j < 4; j++)
							{
								if (j == i)
								{
									slotSelected[j] = true;
								}
								else
								{
									slotSelected[j] = false;
								}
							}
							selected.Play();
						}
					}
				}
			}
			else if (currentScreen == screens.species) 
			{
				for (int i = 0; i < 9; i++)
				{
					if (mouse.collidesWith(species[i]) && speciesSelected[i] == false)
					{
						if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
						{
							currentToon.species = (ToonSpecies)i;
							for (int j = 0; j < 9; j++)
							{
								if (j == i)
								{
									speciesSelected[j] = true;
								}
								else
								{
									speciesSelected[j] = false;
								}
							}
							selected.Play();
						}
						
					}
				}
			}
			else if (currentScreen == screens.color)
			{
				for (int i = 0; i < 27; i++)
				{
					if (mouse.collidesWith(colors[i]))
					{
						if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
						{
							currentToon.color = (ToonColors)i;
							selected.Play();
						}

					}
				}
			}
			else if (currentScreen == screens.name)
			{
				for (int i = 0; i < 3; i++)
				{
					if (mouse.collidesWith(nameSelect[i]))
					{
						if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
						{
							if (i == 0)
							{
								if (titleSelected == true) 
								{
									titleSelected = false;
								}
								else 
								{
									titleSelected = true;
								}
							}
							else if (i == 1)
							{
								if (firstSelected == true)
								{
									firstSelected = false;
								}
								else
								{
									firstSelected = true;
								}
							}
							else 
							{
								if (lastSelected == true)
								{
									lastSelected = false;
								}
								else
								{
									lastSelected = true;
								}
							}
						}

					}
				}
			}

			Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);

			mouse.X = mousePosition.X;
			mouse.Y = mousePosition.Y;
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

			Rectangle arrowSource = new Rectangle(0, 0, 127, 127);
			Rectangle ToonSource = new Rectangle(769, 256, 256, 256); ;
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

			spriteBatch.Begin();
			Vector2 destination = new Vector2((graphics.Viewport.Width - 500), (graphics.Viewport.Height - 100));
			var destinationRectangle = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);


			spriteBatch.Draw(backgrounds[(int)currentScreen], Vector2.Zero, destinationRectangle, Color.White);

			destination = new Vector2((graphics.Viewport.Width - 250), (graphics.Viewport.Height - 200));
			spriteBatch.Draw(arrowButton, destination, arrowSource, rightArrowColor, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

			destination = new Vector2((50), (graphics.Viewport.Height - 200));
			spriteBatch.Draw(arrowButton, destination, arrowSource, leftArrowColor, 0f, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0);

			spriteBatch.DrawString(font, titles[(int)currentScreen], new Vector2((graphics.Viewport.Width - 750) / 2, 0), Color.Black);

			if (currentScreen == screens.slots)
			{

				for (int i = 0; i < 4; i++)
				{
					spriteBatch.Draw(slotOverlay, new Vector2((graphics.Viewport.Width - 500) / 2, (graphics.Viewport.Height + i * 500) / 4), null, Color.White, 0f, Vector2.Zero, new Vector2(2f, 1f), SpriteEffects.None, 0);
					if (slotSelected[i])
					{
						spriteBatch.Draw(selectedButton, new Vector2((graphics.Viewport.Width + 350) / 2, (graphics.Viewport.Height + i * 550) / 4), new Rectangle(133, 68, 53, 58), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
					}
					spriteBatch.DrawString(font, "Toon #" + (i + 1) + ": " + party[i].name + "\nSpecies: " + party[i].species + "\nColor: " + party[i].color, new Vector2((graphics.Viewport.Width - 450) / 2, (graphics.Viewport.Height + i * 500) / 4), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
				}
			}
			else if (currentScreen == screens.species)
			{
				int row = 0;
				int column = 0;
				spriteBatch.Draw(slotOverlay, new Vector2((graphics.Viewport.Width + 400) / 2, (graphics.Viewport.Height - 100) / 4), null, Color.White, 0f, Vector2.Zero, new Vector2(1.5f, 3), SpriteEffects.None, 0);
				for (int i = 0; i < 9; i++)
				{
					Color color = Color.White;
					Rectangle toonSource = getSourceToonIcons((ToonSpecies)i);
					if (speciesSelected[i] == true)
					{
						color = Color.Green;
					}
					spriteBatch.Draw(toonIcons, new Vector2((graphics.Viewport.Width + 400 + (250 * column)) / 2, (graphics.Viewport.Height - 100 + (row * 500)) / 4), toonSource, color);
					column++;
					if (column == 3)
					{
						column = 0;
						row++;
					}
				}
				spriteBatch.Draw(toonIcons, new Vector2((graphics.Viewport.Width - 1000) / 2, (graphics.Viewport.Height - 200) / 4), getSourceToonIcons(currentToon.species), customColor.customToonColors[(int)currentToon.color], 0f, Vector2.Zero, 3f, SpriteEffects.None, 0);
				spriteBatch.DrawString(font, currentToon.species.ToString().ToUpper(), new Vector2((graphics.Viewport.Width - 950) / 2, (graphics.Viewport.Height + 1100) / 4), Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
			}
			else if (currentScreen == screens.color)
			{
				int row = 0;
				int column = 0;
				spriteBatch.Draw(slotOverlay, new Vector2((graphics.Viewport.Width + 400) / 2, (graphics.Viewport.Height - 100) / 4), null, Color.White, 0f, Vector2.Zero, new Vector2(1.5f, 5), SpriteEffects.None, 0);
				for (int i = 0; i < 27; i++)
				{
					Color color = customColor.customToonColors[i];
					spriteBatch.Draw(colorOption, new Vector2((graphics.Viewport.Width + 400 + (250 * column)) / 2, (graphics.Viewport.Height - 100 + (row * 275)) / 4), null, color);
					column++;
					if (column == 3)
					{
						column = 0;
						row++;
					}
				}
				spriteBatch.Draw(toonIcons, new Vector2((graphics.Viewport.Width - 1000) / 2, (graphics.Viewport.Height - 200) / 4), getSourceToonIcons(currentToon.species), customColor.customToonColors[(int)currentToon.color], 0f, Vector2.Zero, 3f, SpriteEffects.None, 0);
				spriteBatch.DrawString(font, currentToon.color.ToString().ToUpper(), new Vector2((graphics.Viewport.Width - 950) / 2, (graphics.Viewport.Height + 1100) / 4), Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
			}
			else if (currentScreen == screens.name) 
			{
				spriteBatch.Draw(namePlate, new Vector2(graphics.Viewport.Width / 4, (graphics.Viewport.Height - 1000) / 4), null, Color.White);

				if (titleSelected) 
				{
					int row = 0;
					spriteBatch.Draw(colorOption, new Vector2((graphics.Viewport.Width - 900 ) / 2, (graphics.Viewport.Height + 190 ) / 4), null, new Color(92, 72, 71), 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
					for (int i = title; i < title + 5; i++) 
					{
						Color color = Color.Black;
						if (title == i) 
						{
							color = Color.SkyBlue;
						}
						spriteBatch.DrawString(font, titleNames[i] , new Vector2((graphics.Viewport.Width - 800) / 2, (graphics.Viewport.Height + 700 + (row * 150 )) / 4), color, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
						row++;
					}
				}
				if (firstSelected)
				{
					int row = 0;
					spriteBatch.Draw(colorOption, new Vector2((graphics.Viewport.Width - 400) / 2, (graphics.Viewport.Height + 190) / 4), null, new Color(92, 72, 71), 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
					for (int i = first; i < first + 5; i++)
					{
						Color color = Color.Black;
						if (first == i)
						{
							color = Color.SkyBlue;
						}
						spriteBatch.DrawString(font, firstNames[i], new Vector2((graphics.Viewport.Width - 300) / 2, (graphics.Viewport.Height + 700 + (row * 150)) / 4), color, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
						row++;
					}
				}
				if (lastSelected)
				{
					int row = 0;
					spriteBatch.Draw(colorOption, new Vector2((graphics.Viewport.Width + 350) / 2, (graphics.Viewport.Height + 190) / 4), null, new Color(92, 72, 71), 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
					for (int i = lastF; i < lastF + 5; i++)
					{
						Color color = Color.Black;
						if (lastF == i)
						{
							color = Color.SkyBlue;
						}
						spriteBatch.DrawString(font, lastFirst[i], new Vector2((graphics.Viewport.Width + 200) / 2, (graphics.Viewport.Height + 700 + (row * 150)) / 4), color, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
						row++;
					}
					row = 0;
					for (int i = lastS; i < lastS + 5; i++)
					{
						Color color = Color.Black;
						if (lastS == i)
						{
							color = Color.SkyBlue;
						}
						spriteBatch.DrawString(font, lastSecond[i], new Vector2((graphics.Viewport.Width + 600) / 2, (graphics.Viewport.Height + 700 + (row * 150)) / 4), color, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
						row++;
					}
				}

			}


			spriteBatch.End();

		}

	}
}
