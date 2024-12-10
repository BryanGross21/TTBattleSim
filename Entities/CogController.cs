using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTBattleSim.Instances;
using TTBattleSim.Rooms;

namespace TTBattleSim.Entities
{
	public class CogController
	{

		public Street streetController;

		public CogController(PlayGround ground, Area currentStreet) 
		{
				streetController = new Street(ground, currentStreet);
		}

		public COG GenerateCog() 
		{
			CogDepartments Depo = CogDepartments.Bossbot;
			Random ranNum = new();
			double randomDepartment = ranNum.Next(0, 101);
			if (randomDepartment <= streetController.cogSpawnChance[0])
			{
				Depo = CogDepartments.Bossbot;
			}
			else if (randomDepartment <= streetController.cogSpawnChance[0] + streetController.cogSpawnChance[1])
			{
				Depo = CogDepartments.Lawbot;
			}
			else if (randomDepartment <= streetController.cogSpawnChance[0] + streetController.cogSpawnChance[1] + streetController.cogSpawnChance[2])
			{
				Depo = CogDepartments.Cashbot;
			}
			else
			{
				Depo = CogDepartments.Sellbot;
			}
			int randomLevel = 1;
			int randomTier;
			randomTier = ranNum.Next(streetController.minTier, streetController.maxTier + 1);
			if (randomTier + 4 < streetController.maxLevel)
			{
				randomLevel = ranNum.Next(streetController.minLevel, (randomTier + 4) + 1);
			}
			else
			{
				randomLevel = ranNum.Next(randomTier, streetController.maxLevel + 1);
			}
			COG cog = new COG(Depo,randomTier, randomLevel);

			return cog;
		}

	}
}
