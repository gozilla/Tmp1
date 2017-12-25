using UnityEngine;

namespace Project.Utils.Spawn
{
	[System.Serializable]
	public class AutoRespawnData
	{
		public bool needRespawn = false;
		public bool respawnOnTime = false;

		public float respawnTime = -1;
		public float respawnDelay = -1f;
		
	}
}

