using System.Collections.Generic;

using UnityEngine;

namespace Project.Utils.Spawn
{
	public class SpawnPoints : MonoBehaviour
	{
		[SerializeField]
		public AutoSpawnData _autoSpawnData;

		private int curPoint = -1;

		[SerializeField]
		private Transform[] _spawnPoints;
		public Transform[] CurSpawnPoints
		{
			get
			{
				return _spawnPoints;
			}
			set
			{
				_spawnPoints = value;
			}
		}

		private Transform _cachedTransform;
		public Transform Transform
		{
			get { return _cachedTransform; }
		}

		private void Start()
		{
			_cachedTransform = transform;
			AutoSpawn();
		}

		private void AutoSpawn()
		{
			
			if (_autoSpawnData != null && (_spawnPoints == null || _spawnPoints.Length == 0))
			{
				if (_autoSpawnData.needAutoCreate && _autoSpawnData.spawnObject != null)
				{
					_spawnPoints = new Transform[_autoSpawnData.width * _autoSpawnData.length];
					float halfWidth = _autoSpawnData.width;
					float halfHeight = _autoSpawnData.height;
					float halfLeight = _autoSpawnData.length;
					int k = 0;
					for (int i = 0; i < _autoSpawnData.width; i++)
					{
						for (int j = 0; j < _autoSpawnData.length; j++)
						{
							GameObject go = Instantiate(_autoSpawnData.spawnObject);
							go.name = _autoSpawnData.spawnObject.name + "_" + i.ToString() + "_" + j.ToString();
							go.transform.parent = gameObject.transform;
							float xPos = -halfWidth + _autoSpawnData.widthDelta * i;
							float yPos = -halfHeight + _autoSpawnData.heightDelta * j;
							float zPos = -halfLeight + _autoSpawnData.lengthDelta * j;
							go.transform.localPosition = new Vector3(xPos, yPos, zPos);
							_spawnPoints[k] = go.transform;
							k++;
						}
					}
				}
			}
		}

		public Transform GetNextSpawnPoint()
		{
			curPoint++;
			if (curPoint > _spawnPoints.Length - 1)
				curPoint = 0;
			if (_spawnPoints.Length == 0)
				AutoSpawn();
			return _spawnPoints[curPoint];
		}

		public Transform GetNextSpawnPoint(Vector3 position)
		{
			float min = float.MaxValue;
			for (int i = 0; i < _spawnPoints.Length; i++)
			{
				float cur = (_spawnPoints[i].transform.position - position).magnitude;
				if (cur < min)
				{
					min = cur;
					curPoint = i;
				}
			}

			return _spawnPoints[curPoint];
		}

		public Transform GetNextSpawnPoint(Vector3 position, float radius)
		{
			List<int> points = new List<int>();
			float min = float.MaxValue;
			for (int i = 0; i < _spawnPoints.Length; i++)
			{
				float cur = (_spawnPoints[i].transform.position - position).magnitude;
				if (cur <= radius)
				{
					points.Add(i);
				}
				if (cur < min)
				{
					min = cur;
					curPoint = i;
				}
			}
			if (points.Count > 0)
			{
				int rnd = UnityEngine.Random.Range(0, points.Count - 1);
				curPoint = points[rnd];
			}

			return _spawnPoints[curPoint];
		}

		public void OnDrawGizmos()
		{
			//Gizmos.color = Color.white;
			//foreach (Transform point in _spawnPoints)
			//{
			//	Vector2 result = MathUtil.Rotate(new Vector2(0, 1), point.eulerAngles.y);
			//	Gizmos.DrawLine(point.position + Vector3.up * 0.1f, point.position + new Vector3(result.x, 0, result.y) * 2 + Vector3.up * 0.1f);
			//}
		}
	}
}

