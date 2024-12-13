using System;
using System.Xml.Linq;

namespace TTBattleSim.Entities
{
	public class COG
	{

		public COG(CogDepartments depo, int Tier, int Level) 
		{
			_setTier = Tier;
			_setLevel = Level;
			_setDepo = depo;
			_getHP = (_setLevel + 1) * (_setLevel + 2);
			floorHP = _getHP;
			int cogVal = (8 * (int)Depo) + Tier;
			int nameValue = cogVal - 1;
			Random ran = new();
			int tauntValue = (cogVal - 1) * 4 + ran.Next(0, 4); 
			StandardCOGNames name = new();
			CogTaunts taunt = new();
			Name = name.Names[nameValue];
			Taunt = taunt.taunts[tauntValue];
		}

		public bool isTrapped;

		public bool isLured;

		public int trapDamage;

		public bool canNoLongerBeTrapped;

		private CogDepartments _setDepo; //Sets the cogs current department (0 for bossbot, 1 for lawbot, 2 for cashbot, 3 for sellbot)
		public CogDepartments Depo { get { return _setDepo; } init { _setDepo = value; } }

		private int _setTier;
		public int Tier { get { return _setTier; } init { _setTier = value; } }

		private int _setLevel;
		public int Level { get { return _setLevel; } init { _setLevel = value; } }
		public string Name {
			get;
			init;
		}

		public string Taunt
		{
			get;
			init;
		}

		private int _getHP;
		public int HP { 
			get 
			{
				return _getHP;
			}
			set
			{
				_getHP = value;
			} 
		}

		public int floorHP 
		{
			get;
			init;
		}
	}
}
