using Project.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Project.UI;
using Project.UI.MainMenu;
namespace Project.App
{
	public class ImageDownloadData
	{
		public Sprite Sprite = new Sprite();
		public Vector2 SizeDelta = new Vector2();
		public ArrayData WebDataItem = new ArrayData();
		public List<ArrayData> WebDataItems = new List<ArrayData>();
		public bool isReady = false;
	}

	[AppSingleton]
	public class AppData : MonoSingleton<AppData>
	{

		public event Action<uint> OnGoldChangeEvent;
		public event Action<uint> OnGameOptionChangeEvent;

		public GameWebData GameWebData = new GameWebData();
		private uint _gold = 0;
		public uint Gold
		{
			set
			{
				_gold = value;
				SetStringFromPlayerPrefs("GoldCurrensy", _gold.ToString());
				if (OnGoldChangeEvent != null)
				{
					OnGoldChangeEvent(_gold);
				}
			}
			get
			{
				if (_gold == 0)
				{
					string stringGold = GetStringFromPlayerPrefs("GoldCurrensy","30");
					if (uint.TryParse(stringGold, out _gold) == false)
					{
						Debug.Log("Cant parse GoldCurrensy in PlayerPrefs");
						return 0;
					}
				}
				return _gold;
			}
		}

		private uint _gameOption = 0;
		public uint GameOption
		{
			set
			{
				_gameOption = value;
				SetStringFromPlayerPrefs("GameOption", _gameOption.ToString());
				if (OnGameOptionChangeEvent != null)
				{
					OnGameOptionChangeEvent(_gameOption);
				}
			}
			get
			{
				if (_gameOption == 0)
				{
					string stringOption = GetStringFromPlayerPrefs("GameOption");
					if (uint.TryParse(stringOption, out _gameOption) == false)
					{
						Debug.Log("Cant parse GameOption in PlayerPrefs");
						_gameOption = 1;
						SetStringFromPlayerPrefs("GameOption", _gameOption.ToString());
						return 1;
					}
				}
				return _gameOption;
			}
		}

		private bool _adsExist = true;
		public bool AdsExist
		{
			get
			{
				int intAds = GetIntFromPlayerPrefs("RemoveAds");
				_adsExist = intAds == 0 ? true : false;
				return _adsExist;
			}
		}

		private int _imagesMaxCount = 0;
		public int ImagesMaxCount
		{
			set { _imagesMaxCount = value; }
			get { return _imagesMaxCount; }
		}
		private int _imagesCurCount = 0;
		private Dictionary<string,ImageDownloadData> DownloadedDataItems = new Dictionary<string, ImageDownloadData>();

		private int _playerScore = 0;
		public int PlayerScore
		{
			set
			{
				_playerScore = value;
			}
			get
			{
				if (_playerScore < 0)
					_playerScore = 0;
				return _playerScore;
			}
		}

		private int _playerHP = 0;
		public int PlayerHP
		{
			set
			{
				_playerHP = value;
			}
			get
			{
				if (_playerHP < 0)
					_playerHP = 0;
				return _playerHP;
			}
		}

		private bool _hint50Active = true;
		public bool Hint50Active
		{
			set
			{
				_hint50Active = value;
			}
			get
			{
				return _hint50Active;
			}
		}

		private bool _hintNextActive = true;
		public bool HintNextActive
		{
			set
			{
				_hintNextActive = value;
			}
			get
			{
				return _hintNextActive;
			}
		}

		private int playerBestScore = 0;
		private const string playerBestScoreName = "PlayerBestScore";
		public int PlayerBestScore
		{
			set
			{
				playerBestScore = value;
				SetStringFromPlayerPrefs(playerBestScoreName, playerBestScore.ToString());
			}
			get
			{
				string score = GetStringFromPlayerPrefs(playerBestScoreName);
				if (int.TryParse(score, out playerBestScore) == false)
				{
					Debug.Log("Cant parse PlayerBestScore in PlayerPrefs");
					return 0;
				}
				else
				{
					return playerBestScore;
				}
			}
		}

		public string GetStringByName(string name)
		{
			return GetStringFromPlayerPrefs(name);
		}

		private string GetStringFromPlayerPrefs(string name, string key = "")
		{
			if (!PlayerPrefs.HasKey(name))
				PlayerPrefs.SetString(name, key);
			else
				key = PlayerPrefs.GetString(name);
			return key;
		}

		private void SetStringFromPlayerPrefs(string name, string key)
		{
			PlayerPrefs.SetString(name, key);
		}

		private int GetIntFromPlayerPrefs(string name, int key = 0 )
		{
			if (!PlayerPrefs.HasKey(name))
				PlayerPrefs.SetInt(name, key);
			else
				key = PlayerPrefs.GetInt(name);
			return key;
		}

		public void DownloadImages(int count)
		{
			ImagesMaxCount = count;
			if (_imagesCurCount <= ImagesMaxCount)
			{
				DownloadImage();
			}
		}

		public bool CheckImages()
		{
			bool needDownload = _imagesCurCount <= ImagesMaxCount;
			if (needDownload)
			{
				DownloadImage();
			}
			return needDownload;
		}

		public ImageDownloadData GetDownloadedImage()
		{
			string key = "";
			ImageDownloadData nextData = null;
			foreach (var item in DownloadedDataItems)
			{
				if (item.Value.isReady)
				{
					key = item.Key;
					nextData = item.Value;
					break;
				}
			}

			if (nextData != null)
			{
				//Destroy(DownloadedDataItems[key].Sprite);
				DownloadedDataItems.Remove(key);
				_imagesCurCount--;
			}
			return nextData;
		}

		public void DownloadImage()
		{
			ArrayData curData = GameWebData.GetRandomItem();
			ImageDownloadData curDownloadData = new ImageDownloadData();
			curDownloadData.WebDataItem = curData;
			GameWebData.Data.Remove(curData);

			curDownloadData.WebDataItems = GameWebData.GetRandomItems(4, new List<string>() { curData.Name });
			curDownloadData.isReady = false;
			if (curData.Name == null || curData.Name =="" || curData.Name == "null")
			{
				Debug.Log("curData.Name null: " + curData.Id);
				curData.Name = UnityEngine.Random.Range(0, 10000).ToString() + "_" + UnityEngine.Random.Range(0, 10000).ToString();
			}
			DownloadedDataItems.Add(curData.Id, curDownloadData);
			WWWUtils.Instance.InitializeTextureDowload(curData.path, curData.Id, OnImageDownloaded, OnImageNotDownloaded);
			curData = null;
			curDownloadData = null;
		}

		private void OnImageDownloaded(Texture2D tex, string id)
		{
			DownloadedDataItems[id].Sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
			DownloadedDataItems[id].SizeDelta = new Vector2(tex.width, tex.height);
			DownloadedDataItems[id].isReady = true;
			tex = null;
			_imagesCurCount++;
			if (_imagesCurCount <= ImagesMaxCount)
			{
				DownloadImage();
			}
		}

        private void OnImageNotDownloaded()
        {
             PopupNoConnectionView popup = PopupManager.Instance.GetRegisteredPopup<PopupNoConnectionView>();
            if (popup != null)
            {
                popup.Show();
            }
        }

        
        public void ClearWebData()
        {
            GameWebData.Data.Clear();
			foreach (var item in DownloadedDataItems)
			{
				if (item.Value.Sprite != null)
				{
					Destroy(item.Value.Sprite.texture);
				}
			}
            DownloadedDataItems.Clear();
            _imagesCurCount = 0;
        }
    }
}
