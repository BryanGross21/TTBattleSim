using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTBattleSim
{
	public class Crate
	{
		// The game this crate belongs to
		Game game;

		// The VertexBuffer of crate vertices
		VertexBuffer vertexBuffer;

		// The IndexBuffer defining the Crate's triangles
		IndexBuffer indexBuffer;

		// The effect to render the crate with
		BasicEffect effect;

		// The texture to apply to the crate
		Texture2D texture;

		/// <summary>
		/// Creates a new crate instance
		/// </summary>
		/// <param name="game">The game this crate belongs to</param>
		/// <param name="type">The type of crate to use</param>
		/// <param name="world">The position and orientation of the crate in the world</param>
		public Crate(Game game, Matrix world)
		{
			this.game = game;
			this.texture = game.Content.Load<Texture2D>("Textures/CBCrate");
			InitializeVertices();
			InitializeIndices();
			InitializeEffect();
			effect.World = world;
		}

		/// <summary>
		/// Initializes the vertex of the cube
		/// </summary>
		public void InitializeVertices()
		{
			var vertexData = new VertexPositionNormalTexture[] { 
            // Front Face
            new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Forward },
			new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Forward },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Forward },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Forward },

            // Back Face
            new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, 1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Backward },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, 1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Backward },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f, 1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Backward },
			new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f, 1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Backward },

            // Top Face
            new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, 1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Up },
			new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, 1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Up },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, 1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Up },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, 1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Up },

            // Bottom Face
            new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Down },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Down },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Down },
			new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Down },

            // Left Face
            new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Left },
			new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Left },
			new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Left },
			new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Left },

            // Right Face
            new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Right },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Right },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Right },
			new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Right },
		};
			vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture), vertexData.Length, BufferUsage.None);
			vertexBuffer.SetData<VertexPositionNormalTexture>(vertexData);
		}

		/// <summary>
		/// Initializes the Index Buffer
		/// </summary>
		public void InitializeIndices()
		{
			var indexData = new short[]
			{
            // Front face
            0, 2, 1,
			0, 3, 2,

            // Back face 
            4, 6, 5,
			4, 7, 6,

            // Top face
            8, 10, 9,
			8, 11, 10,

            // Bottom face 
            12, 14, 13,
			12, 15, 14,

            // Left face 
            16, 18, 17,
			16, 19, 18,

            // Right face 
            20, 22, 21,
			20, 23, 22
			};
			indexBuffer = new IndexBuffer(game.GraphicsDevice, IndexElementSize.SixteenBits, indexData.Length, BufferUsage.None);
			indexBuffer.SetData<short>(indexData);
		}

		/// <summary>
		/// Initializes the BasicEffect to render our crate
		/// </summary>
		void InitializeEffect()
		{
			effect = new BasicEffect(game.GraphicsDevice);
			effect.World = Matrix.CreateScale(2.0f);
			effect.View = Matrix.CreateLookAt(
				new Vector3(8, 9, 12), // The camera position
				new Vector3(0, 0, 0), // The camera target,
				Vector3.Up            // The camera up vector
			);
			effect.Projection = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.PiOver4,                         // The field-of-view 
				game.GraphicsDevice.Viewport.AspectRatio,   // The aspect ratio
				0.1f, // The near plane distance 
				100.0f // The far plane distance
			);
			effect.TextureEnabled = true;
			effect.Texture = texture;

			effect.EnableDefaultLighting();

			effect.AmbientLightColor = new Vector3(.5f, 0.4f, 0.25f);

		}

		/// <summary>
		/// Draws the crate
		/// </summary>
		public void Draw(ICamera camera)
		{
			effect.View = camera.View;
			effect.Projection = camera.Projection;

			// apply the effect 
			effect.CurrentTechnique.Passes[0].Apply();

			game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			// set the vertex buffer
			game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
			// set the index buffer
			game.GraphicsDevice.Indices = indexBuffer;
			// Draw the triangles
			game.GraphicsDevice.DrawIndexedPrimitives(
				PrimitiveType.TriangleList, // Tye type to draw
				0,                          // The first vertex to use
				0,                          // The first index to use
				12                          // the number of triangles to draw
			);

		
		}



	}
}
