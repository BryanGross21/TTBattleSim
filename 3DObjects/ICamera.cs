using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TTBattleSim
{
	public interface ICamera
	{
		/// <summary>
		/// The view matrix
		/// </summary>
		Matrix View { get; }

		/// <summary>
		/// The projection matrix
		/// </summary>
		Matrix Projection { get; }
	}
}
