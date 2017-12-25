using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathologicalGames;

namespace Project.Core
{
	public class ResourceLoader<T> where T : UnityEngine.Object
	{
		protected Dictionary <string, string> _stringResourcesPath = new Dictionary<string, string>();
		
		protected Dictionary <string, T> _loadedObjects = new Dictionary<string, T>();

		public T this[string name] 
		{
			get 
			{
				return GetObjectByName(name);
			}
		}

		public void Add(string name, string path)
		{
			_stringResourcesPath.Add(name, path);
		}

		public void Add(IDictionary<string, string> resourcesPath)
		{
			foreach (var kv in resourcesPath)
			{
				Add(kv.Key, kv.Value);
			}
		}

		public T GetObjectByName(string name)
		{
			string path;
			if (!_stringResourcesPath.TryGetValue(name, out path))
			{
				Debug.LogWarningFormat( "ResourceLoader does not have string key {0}", name );
				return null;
			}

			T obj;
			if (!_loadedObjects.TryGetValue(name, out obj))
			{
				obj = Resources.Load<T> (path);
				_loadedObjects [name] = obj;
			}
			
			return obj;
		}

		public virtual void Clear()
		{
			_stringResourcesPath.Clear();
            _loadedObjects.Clear();
		}
	}

	public class ResourceLoaderPooled: ResourceLoader<GameObject>
	{
		private Dictionary <string, bool> _pooled = new Dictionary<string, bool>();
		private SpawnPool _pool;

		public ResourceLoaderPooled(string poolName)
		{
			_pool = PoolManager.Pools.Create(poolName);
		}

		public void Add(string name, string path, bool pooled)
		{
			base.Add(name, path);
			if (pooled)
				_pooled[name] = false;
		}

		public GameObject Create(string name, Vector3 position, Quaternion rotation)
		{
			GameObject prefab = GetObjectByName(name);
			Transform instance = null;

			if (prefab != null)
			{
				bool pooled;
				if (_pooled.TryGetValue(name, out pooled))
				{
					if (!pooled)
					{
						CreatePrefabPool(prefab);
						_pooled[name] = true;
					}

					instance = _pool.Spawn(prefab, position, rotation);
				} else 
				{
					GameObject go = GameObject.Instantiate(prefab, position, rotation) as GameObject;
					if (go != null)
						instance = go.transform;
				}
			}

			return instance != null ? instance.gameObject : null;
		}

		public void Delete(GameObject instance, float time = 0f)
		{
			if (instance != null)
			{
				time = Mathf.Clamp(time, 0f, Mathf.Infinity);

				if (_pool.GetPrefab(instance) != null)
				{
					if (time != 0f)
						_pool.Despawn(instance.transform, time);
					else
						_pool.Despawn(instance.transform);
				} else {
					if (time != 0f)
						GameObject.Destroy(instance, time);
					else
						GameObject.Destroy(instance);
				}
			}
		}

		private void CreatePrefabPool(GameObject prefab)
		{
			PrefabPool prefabPool = new PrefabPool(prefab.transform);
			prefabPool.preloadAmount = 0;
			_pool.CreatePrefabPool(prefabPool);
		}

		public override void Clear()
		{
			if (_pool != null)
			{
				PoolManager.Pools.Destroy(_pool.poolName);
			}

			_pooled.Clear();

			base.Clear();
		}
	}
}
