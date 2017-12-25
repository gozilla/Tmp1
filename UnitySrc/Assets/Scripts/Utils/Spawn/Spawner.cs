using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Project.Utils;

namespace Project.Utils.Spawn
{
	public class Spawner : MonoBehaviour
	{
		[SerializeField]
		AutoRespawnData _autoRespawnData;
		
		[SerializeField]
		private int _spawnUnitsCount = 1;
		public int SpawnUnitsCount
		{
			set
			{
				_spawnUnitsCount = value;
			}
		}

		private string _spawnUnitsName = "Unit";

		private int curUnit = -1;
		[SerializeField]
		private GameObject _spawnPrefab;

		[SerializeField]
		private List<GameObject> _spawnPrefabs = null;
		public List<GameObject> SpawnPrefabs
		{
			get
			{
				return _spawnPrefabs;
			}
			set
			{
				_spawnPrefabs = value;
			}
		}

		void Start()
		{
			if (_spawnPrefabs == null)
			{
				_spawnPrefabs = new List<GameObject>();
			}
			_autoRespawnData.respawnTime = Time.time + _autoRespawnData.respawnDelay;
		}

		void Update()
		{
			if (_autoRespawnData.needRespawn)
			{
				if (_autoRespawnData.respawnOnTime && Time.time > _autoRespawnData.respawnTime)
				{
					SpawnUnits();
					_autoRespawnData.respawnTime = Time.time + _autoRespawnData.respawnDelay;
				}
			else if (Time.time > _autoRespawnData.respawnTime)
				{
					RespawnUnits();
					_autoRespawnData.respawnTime = Time.time + _autoRespawnData.respawnDelay;
				}
			}
		}

		public GameObject SpawnUnit(string name = "")
		{
			if (name != "")
				_spawnUnitsName = name;

			GameObject unit = Instantiate(_spawnPrefab) as GameObject;
			unit.transform.position = transform.position;
			_spawnPrefabs.Add(unit);
			unit.name = _spawnUnitsName + _spawnPrefabs.Count;
			SpawnPoints spawnPoints = GetComponent<SpawnPoints>();
			if (spawnPoints != null)
				unit.transform.position = spawnPoints.GetNextSpawnPoint().position;
			_spawnUnitsName = "Unit";
			return unit;
		}

		public List<GameObject> SpawnUnits()
		{
			if (_spawnPrefabs == null)
				_spawnPrefabs = new List<GameObject>();
			for (int i = 0; i < _spawnUnitsCount; i++)
			{
				SpawnUnit();
			}
			return _spawnPrefabs;
		}

		public void RespawnUnits()
		{
			CheckPrefabs();
			if (_spawnPrefabs == null)
				_spawnPrefabs = new List<GameObject>();
			int curCount = _spawnPrefabs.Count;
			if (_spawnUnitsCount > curCount)
			{
				curCount = _spawnUnitsCount - curCount;
				for (int i = 0; i < curCount; i++)
				{
					SpawnUnit();
				}
			}
		}

		public GameObject GetNextSpawnedUnit()
		{
			curUnit++;
			if (curUnit > _spawnPrefabs.Count)
				curUnit = 0;
			return _spawnPrefabs[curUnit];
		}

		public void CheckPrefabs()
		{
			for (int i = 0; i < _spawnPrefabs.Count; i++)
			{
				if (_spawnPrefabs[i])
				{
				}
				else
				{
					_spawnPrefabs.RemoveRange(i, 1);
					i--;
				}
			}
		}

	}
}
