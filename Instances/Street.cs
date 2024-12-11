using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTBattleSim.Rooms;

namespace TTBattleSim.Instances
{
	public class Street 
	{
		public int minLevel { get; set; } = 1;
		public int maxLevel { get; set; } = 3;
		public int minTier { get; set; } = 1;
		public int maxTier { get; set; } = 3;

		//Double holds  spawn chances, 0 is bossbot, 1 is lawbot, etc.
		public virtual double[] cogSpawnChance { get; set; } = new double[4];
		public Area currentStreet;
		public PlayGround currentGround;
		public string name;

		public Street(PlayGround playground, Area streetSelection)
		{
			currentGround = playground;
			currentStreet = streetSelection;
			StreetNames names = new();
			name = names.streetNames[(int)streetSelection];
			setStreetInfo();
		}

		private void setStreetInfo()
		{
			if (currentGround > PlayGround.TTC)
			{
				if (currentGround < PlayGround.BRRRGH)
				{
					if (currentStreet == Area.Elm || currentStreet == Area.Barnacle || currentStreet == Area.Alto)
					{
						minLevel = 2;
						maxLevel = 4;
						maxTier = 4;
					}
					else
					{
						minLevel = 3;
						maxLevel = 6;
						maxTier = 6;
					}
				}
				else if (currentGround < PlayGround.SBHQ)
				{
					if (currentGround == PlayGround.BRRRGH)
					{
						if (currentStreet != Area.Polar)
						{
							minLevel = 5;
							maxLevel = 7;
							maxTier = 6;
						}
						else
						{
							minLevel = 7;
							maxLevel = 9;
							minTier = 3;
							maxTier = 7;
						}
					}
					else
					{
						minLevel = 6;
						maxLevel = 9;
						minTier = 2;
						maxTier = 7;
					}
				}
				else if (currentGround == PlayGround.SBHQ)
				{
					if (currentStreet == Area.SBHQCourt)
					{
						minLevel = 4;
						maxLevel = 6;
						maxTier = 6;
					}
					else
					{
						minLevel = 5;
						maxLevel = 8;
						maxTier = 8;
					}
				}
				else if (currentGround == PlayGround.CBHQ)
				{
					minLevel = 5;
					maxLevel = 9;
					maxTier = 8;
				}
				else 
				{
					minLevel = 5;
					maxLevel = 10;
					maxTier = 8;
				}
			}

			//Sets cog department spawn chances, all chances out of	100
			if (currentStreet == Area.Loopy) 
			{
				cogSpawnChance[0] = 10; //Bossbot
				cogSpawnChance[1] = 70; //Lawbot
				cogSpawnChance[2] = 10; //Cashbot
				cogSpawnChance[3] = 10; //Sellbot
			}
			else if (currentStreet == Area.Punchline)
			{
				cogSpawnChance[0] = 10; //Bossbot
				cogSpawnChance[1] = 10; //Lawbot
				cogSpawnChance[2] = 40; //Cashbot
				cogSpawnChance[3] = 40; //Sellbot
			}
			else if (currentStreet == Area.Silly)
			{
				cogSpawnChance[0] = 25; //Bossbot
				cogSpawnChance[1] = 25; //Lawbot
				cogSpawnChance[2] = 25; //Cashbot
				cogSpawnChance[3] = 25; //Sellbot
			}
			else if (currentStreet == Area.Barnacle)
			{
				cogSpawnChance[0] = 90; //Bossbot
				cogSpawnChance[1] = 10; //Lawbot
				cogSpawnChance[2] = 0; //Cashbot
				cogSpawnChance[3] = 0; //Sellbot
			}
			else if (currentStreet == Area.Lighthouse)
			{
				cogSpawnChance[0] = 40; //Bossbot
				cogSpawnChance[1] = 40; //Lawbot
				cogSpawnChance[2] = 10; //Cashbot
				cogSpawnChance[3] = 10; //Sellbot
			}
			else if (currentStreet == Area.Seaweed)
			{
				cogSpawnChance[0] = 10; //Bossbot
				cogSpawnChance[1] = 70; //Lawbot
				cogSpawnChance[2] = 10; //Cashbot
				cogSpawnChance[3] = 10; //Sellbot
			}
			else if (currentStreet == Area.Elm)
			{
				cogSpawnChance[0] = 0; //Bossbot
				cogSpawnChance[1] = 20; //Lawbot
				cogSpawnChance[2] = 10; //Cashbot
				cogSpawnChance[3] = 70; //Sellbot
			}
			else if (currentStreet == Area.Maple)
			{
				cogSpawnChance[0] = 10; //Bossbot
				cogSpawnChance[1] = 70; //Lawbot
				cogSpawnChance[2] = 0; //Cashbot
				cogSpawnChance[3] = 20; //Sellbot
			}
			else if (currentStreet == Area.Oak)
			{
				cogSpawnChance[0] = 5; //Bossbot
				cogSpawnChance[1] = 5; //Lawbot
				cogSpawnChance[2] = 5; //Cashbot
				cogSpawnChance[3] = 85; //Sellbot
			}
			else if (currentStreet == Area.Alto)
			{
				cogSpawnChance[0] = 0; //Bossbot
				cogSpawnChance[1] = 0; //Lawbot
				cogSpawnChance[2] = 50; //Cashbot
				cogSpawnChance[3] = 50; //Sellbot
			}
			else if (currentStreet == Area.Baritone)
			{
				cogSpawnChance[0] = 0; //Bossbot
				cogSpawnChance[1] = 0; //Lawbot
				cogSpawnChance[2] = 90; //Cashbot
				cogSpawnChance[3] = 10; //Sellbot
			}
			else if (currentStreet == Area.Tenor)
			{
				cogSpawnChance[0] = 50; //Bossbot
				cogSpawnChance[1] = 50; //Lawbot
				cogSpawnChance[2] = 0; //Cashbot
				cogSpawnChance[3] = 0; //Sellbot
			}
			else if (currentStreet == Area.Walrus)
			{
				cogSpawnChance[0] = 90; //Bossbot
				cogSpawnChance[1] = 10; //Lawbot
				cogSpawnChance[2] = 0; //Cashbot
				cogSpawnChance[3] = 0; //Sellbot
			}
			else if (currentStreet == Area.Sleet)
			{
				cogSpawnChance[0] = 10; //Bossbot
				cogSpawnChance[1] = 20; //Lawbot
				cogSpawnChance[2] = 30; //Cashbot
				cogSpawnChance[3] = 40; //Sellbot
			}
			else if (currentStreet == Area.Polar)
			{
				cogSpawnChance[0] = 5; //Bossbot
				cogSpawnChance[1] = 85; //Lawbot
				cogSpawnChance[2] = 5; //Cashbot
				cogSpawnChance[3] = 5; //Sellbot
			}
			else if (currentStreet == Area.Lullaby)
			{
				cogSpawnChance[0] = 25; //Bossbot
				cogSpawnChance[1] = 25; //Lawbot
				cogSpawnChance[2] = 25; //Cashbot
				cogSpawnChance[3] = 25; //Sellbot
			}
			else if (currentStreet == Area.Pajama)
			{
				cogSpawnChance[0] = 5; //Bossbot
				cogSpawnChance[1] = 5; //Lawbot
				cogSpawnChance[2] = 85; //Cashbot
				cogSpawnChance[3] = 5; //Sellbot
			}
			else if (currentStreet == Area.SBHQCourt)
			{
				cogSpawnChance[0] = 0; //Bossbot
				cogSpawnChance[1] = 0; //Lawbot
				cogSpawnChance[2] = 0; //Cashbot
				cogSpawnChance[3] = 100; //Sellbot
			}
			else if (currentStreet == Area.SBHQFactoryExterior)
			{
				cogSpawnChance[0] = 0; //Bossbot
				cogSpawnChance[1] = 0; //Lawbot
				cogSpawnChance[2] = 0; //Cashbot
				cogSpawnChance[3] = 100; //Sellbot
			}
			else if (currentStreet == Area.CBHQTrainyard)
			{
				cogSpawnChance[0] = 0; //Bossbot
				cogSpawnChance[1] = 0; //Lawbot
				cogSpawnChance[2] = 100; //Cashbot
				cogSpawnChance[3] = 0; //Sellbot
			}
			else if (currentStreet == Area.LBHQCourtyard)
			{
				cogSpawnChance[0] = 0; //Bossbot
				cogSpawnChance[1] = 100; //Lawbot
				cogSpawnChance[2] = 0; //Cashbot
				cogSpawnChance[3] = 0; //Sellbot
			}
		}
	}
}
