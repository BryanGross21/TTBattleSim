using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TTBattleSim
{
	public class CircleCamera : ICamera
	{
		float angle;

		Vector3 position;

		float speed;

		Game game;

		Matrix view;

		Matrix projection;

		public Matrix View => view;

		public Matrix Projection => projection;

		KeyboardState CurrentKeyboardState;

		/// <summary>
		/// Constructs a new camera that circles the origin
		/// </summary>
		/// <param name="game">The game this camera belongs to</param>
		/// <param name="position">The initial position of the camera</param>
		/// <param name="speed">The speed of the camera</param>
		public CircleCamera(Game game, Vector3 position, float speed)
		{
			this.game = game;
			this.position = position;
			this.speed = speed;
			this.projection = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.PiOver4,
				game.GraphicsDevice.Viewport.AspectRatio,
				1,
				1000
			);
			this.view = Matrix.CreateLookAt(
				position,
				Vector3.Zero,
				Vector3.Up
			);
		}

		/// <summary>
		/// Updates the camera's positon
		/// </summary>
		/// <param name="gameTime">The GameTime object</param>
		public void Update(GameTime gameTime)
		{
			CurrentKeyboardState = Keyboard.GetState();
			// update the angle based on the elapsed time and speed
			if (CurrentKeyboardState.IsKeyDown(Keys.D))
			{
				angle += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			if (CurrentKeyboardState.IsKeyDown(Keys.A)) 
			{
				angle -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			// Calculate a new view matrix
			this.view =
				Matrix.CreateRotationY(angle) *
				Matrix.CreateLookAt(position, Vector3.Zero, Vector3.Up);
		}


	}
}
