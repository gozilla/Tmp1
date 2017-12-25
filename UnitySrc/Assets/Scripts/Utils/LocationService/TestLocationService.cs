#if !UNITY_WSA
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TestLocationService : MonoBehaviour
{
	public TextMesh _textMesh;
	public static List<string> locationList = new List<string>();

	void Start()
	{
		StartCoroutine(CheckLocation());
	}

	private void OnBtnRefreshClick()
	{
		StartCoroutine(CheckLocation());
	}

	private void OnBtnSaveClick()
	{
		SaveData();
	}

	private void OnBtnLoadClick()
	{
		LoadData();
		RefreshData();
	}

	IEnumerator CheckLocation()
	{
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
		{
			_textMesh.text = "Location is not enabled by user";
			//yield break;
		}
		_textMesh.text = "Try start location";
		Input.location.Start();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
			_textMesh.text = "Wait for location";
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			_textMesh.text = "Timed out";
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			_textMesh.text = "Unable to determine device location";
			yield break;
		}
		//if (Input.location.status == LocationServiceStatus.Stopped)
		//{
		//	_textMesh.text = "Service stopped";
		//	yield break;
		//}
		else
		{
			// Access granted and location value could be retrieved
			//_textMesh.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
			locationList.Add("Location: latitude = " + Input.location.lastData.latitude + " longitude = " + Input.location.lastData.longitude + " altitude = " + Input.location.lastData.altitude);
			RefreshData();
		}

		// Stop service if there is no need to query location updates continuously
		Input.location.Stop();
	}
	public static void SaveData()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/Locations.gd");
		bf.Serialize(file, locationList);
		file.Close();
	}

	public static void LoadData()
	{
		if (File.Exists(Application.persistentDataPath + "/Locations.gd"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/Locations.gd", FileMode.Open);
			locationList = (List<string>)bf.Deserialize(file);
			file.Close();
		}
	}

	private void RefreshData()
	{
		_textMesh.text = "";
		foreach (var item in locationList)
		{
			_textMesh.text = _textMesh.text + "\n" + item;
		}
	}


}
#endif