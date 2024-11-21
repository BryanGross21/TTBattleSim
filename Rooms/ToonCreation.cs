using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TTBattleSim.Collisions;
using TTBattleSim.Entities;

namespace TTBattleSim.Rooms
{

	public class ToonCreation
	{
		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		string fileName = Environment.CurrentDirectory + "\\ToonList.txt";

		Song MakeAToon;

		BoundingRectangle mouse = new(0, 0, 32, 32);

		public BoundingRectangle Bounds => mouse;




	}
}
