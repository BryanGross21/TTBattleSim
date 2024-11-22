using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TTBattleSim.Collisions;
using TTBattleSim.Entities;
using TTBattleSim.StateManagement;

namespace TTBattleSim.Rooms
{

	public class ToonCreation : GameScreen
	{
		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		string fileName = Environment.CurrentDirectory + "\\ToonList.txt";

		Song MakeAToon;

		Texture[] backgrounds = new Texture2D[6];

		Texture2D arrowButton;

		BoundingRectangle mouse = new(0, 0, 32, 32);

		public BoundingRectangle Bounds => mouse;

		BoundingRectangle leftArrow;

		BoundingRectangle rightArrow;

		bool collidingLeftArrowButton;

		bool collidingRightArrowButton;

		PlayGround groundToReturn;

		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		ContentManager _content;

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
			backgrounds[4] = _content.Load<Texture2D>("Textures/tb");
			backgrounds[5] = _content.Load<Texture2D>("Textures/DDL");

			MakeAToon = _content.Load<Song>("TTOMusic/TTC/MakeAToon");

			/*exitButton = _content.Load<Texture2D>("Textures/phase_3_palette_4alla_1");
			overlay = _content.Load<Texture2D>("MenuBackgrounds/overlay");
			buttons = _content.Load<Texture2D>("Textures/Buttons");
			arrowButton = _content.Load<Texture2D>("Textures/makeatoon_palette_4alla_2");
			option = _content.Load<SoundEffect>("SoundEffects/Generic/Select");
			font = _content.Load<SpriteFont>("menuFont");*/
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(MakeAToon);
		}



	}
}
