using UnityEngine;

namespace Project.Utils.Spawn
{
	[System.Serializable]
	public class AutoSpawnData
	{
		public bool needAutoCreate = false;
		public byte width = 0;
		public byte length = 0;
		public byte height = 0;
		public float widthDelta = 1;
		public float lengthDelta = 1;
		public float heightDelta = 1;
		public GameObject spawnObject = null;
	}
}

