#if !UNITY_WSA
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Project.Core
{
	public static class SaveLoad
	{

		public static List<Game> savedGames = new List<Game>();
		public static Game savedGame = new Game();

		private static List<string> gamesList = new List<string>();

		public static List<string> GamesList
		{
			get { return gamesList; }

		}

		public static void Save()
		{
			SaveLoad.savedGames.Add(Game.current);
			BinaryFormatter bf = new BinaryFormatter();
			//Debug.Log("Application.persistentDataPath = " + Application.persistentDataPath.ToString());
			FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
			bf.Serialize(file, SaveLoad.savedGames);
			file.Close();
		}

		public static void Load()
		{
			if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
				SaveLoad.savedGames = (List<Game>)bf.Deserialize(file);
				file.Close();
			}
		}

		public static void SaveGame(string name)
		{
			if (!gamesList.Contains(name))
				gamesList.Add(name);
			BinaryFormatter bf = new BinaryFormatter();
			//Debug.Log("Application.persistentDataPath = " + Application.persistentDataPath.ToString());
			FileStream file = File.Create(Application.persistentDataPath + "/savedGame" + name + ".gd");
			bf.Serialize(file, Game.current);
			file.Close();
			SaveGamesList();
		}

		public static bool LoadGame(string name)
		{
			if (File.Exists(Application.persistentDataPath + "/savedGame" + name + ".gd"))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/savedGame" + name + ".gd", FileMode.Open);
				savedGame = (Game)bf.Deserialize(file);
				file.Close();
				return true;
			}
			return false;
		}

		public static void SaveGamesList()
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(Application.persistentDataPath + "/savedGamesList.gd");
			bf.Serialize(file, gamesList);
			file.Close();
		}

		public static void LoadGamesList()
		{
			if (File.Exists(Application.persistentDataPath + "/savedGamesList.gd"))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/savedGamesList.gd", FileMode.Open);
				gamesList = (List<string>)bf.Deserialize(file);
				file.Close();
			}
		}
	}
}
#endif