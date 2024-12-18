﻿using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTBattleSim.Rooms;

namespace TTBattleSim.Entities
{
	public enum ToonColors
	{
		White,
		Peach,
		BrightRed,
		Red,
		Maroon,
		Sienna,
		Brown,
		Tan,
		Coral,
		Orange,
		Yellow,
		Cream,
		Citrine,
		Lime,
		Seagreen,
		Green,
		LightBlue,
		Aqua,
		Blue,
		Periwinkle,
		RoyalBlue,
		SlateBlue,
		Purple,
		Lavender,
		Pink,
		Plum,
		Black
	}

	public enum ToonSpecies
	{
		dog,
		cat,
		rabbit,
		horse,
		mouse,
		duck,
		monkey,
		pig,
		bear
	}

	public class Toon
	{
		public string name = "Toon";

		public ToonSpecies species = ToonSpecies.dog;

		public int maximumHP = 15;

		public int currentHP;

		public bool[] gagTracks = { false, false, false, false, true, true, false }; //Each bool represents which track has been unlocked or not, true for unlocked and false for locked.

		public int[] gagLevels = { 1, 1, 1, 1, 1, 1, 1 }; //The level of the gag the toon has for a given track.

		public ToonColors color = ToonColors.White;

		public int hittingCog;

		public int levelGag;

		public gagTypes gag;
	}
}
