using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Project.Core
{
	[System.Serializable]
	public class Game
	{

		public static Game current;
		public string name = "";

		//[SerializeField]
		//private SaveTerrainData _curSaveTerrainData = new SaveTerrainData();
		//public SaveTerrainData CurSaveTerrainData
		//{
		//	set { _curSaveTerrainData = value; }
		//	get { return _curSaveTerrainData; }
		//}

		public Game()
		{
			//_curSaveTerrainData = new SaveTerrainData();
		}
	}
}
