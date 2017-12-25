using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Project.Utils;

namespace Project.Utils
{
	public class ArrayData
	{
		public string Name = "";
		public string Id = "";
		public string Year = "";
		public string path = "";
	}

	public class GameWebData
	{
		public List<ArrayData> Data = new List<ArrayData>();

		public ArrayData GetRandomItem()
		{
			ArrayData rndItem = new ArrayData();
			int rndIndex = Random.Range(0, Data.Count - 1);
			rndItem = Data[rndIndex];
			return rndItem;
		}

		public List <ArrayData> GetRandomItems( int count, List<string> excludeNames)
		{
			List<ArrayData> rndList = new List<ArrayData>();
			List<string> idList = new List<string>();
			int capCount = Data.Count - 1;
			for (int i = 0; i < count; i++)
			{
				int rndIndex = Random.Range(0, capCount);
				ArrayData rndItem = Data[rndIndex];
				while (idList.Contains(rndItem.Name) || excludeNames.Contains(rndItem.Name))
				{
					rndIndex = Random.Range(0, capCount);
					rndItem = Data[rndIndex];
				}
				rndList.Add(rndItem);
				idList.Add(rndItem.Name);
			}
			return rndList;
		}

		public void AddData(List<ArrayData> newData)
		{
			if (newData != null)
			{
				foreach (var item in newData)
				{
					if (!Data.Contains(item))
						Data.Add(item);
				}
			}
		}
	}
}
