using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json;
using Project.Core;

namespace Project.Utils
{
	[AppSingleton]
	public class WWWUtils : MonoSingleton<WWWUtils>
	{
		public Action<float> DowloadProgress = null;

		public class CountryCodeData
		{
			public string ip;
			public string country;
			public string country_name;
		}

		public class TimeDataData
		{
			public double time = 0;
		}

		private int[] dateTimeIDs = new int[] { 1, 7, 10, 11, 13, 16 };
		private int curDateTimeID = 0;
		private string _countryCode = "";

		public string CountryCode
		{
			get { return _countryCode; }
			private set { _countryCode = value; }
		}

		private DateTime _timeData;

		public DateTime TimeData
		{
			get
			{
				return _timeData;
			}
			private set { _timeData = value; }
		}

		private bool isGetCountryCodeWork = false;
		private bool isGetDataTimeWork = false;
		private bool isGetGameDataWork = false;
		private bool isGetTextureDataWork = false;

		public void InitializeCountryCode(System.Action callback = null)
		{
			if (!isGetCountryCodeWork)
				StartCoroutine(GetCountryCode(callback));
		}

		public void InitializeDataTime(Action<DateTime> callback = null)
		{
			if (!isGetDataTimeWork)
				StartCoroutine(GetDataTime(callback));
		}

		public void InitializeGameData(Action<GameWebData> callback = null, Action error = null)
		{
			if (!isGetGameDataWork)
				StartCoroutine(GetGameData(callback, error));
		}

		public void InitializeTextureDowload(string texName, string id, Action<Texture2D, string> callback = null, Action error = null)
		{
			if (!isGetTextureDataWork)
				StartCoroutine(GetTextureData(texName ,id , callback, error));
		}

		IEnumerator GetCountryCode(System.Action callback)
		{
			isGetCountryCodeWork = true;

			WWW www = new WWW(AppsConfiguration.coutryCodeUrl);
			yield return www;

			if (string.IsNullOrEmpty(www.error) && www.text.StartsWith("{"))
			{

				CountryCodeData data = JsonConvert.DeserializeObject<CountryCodeData>(www.text);
				CountryCode = data.country.ToLower();

				if (callback != null)
					callback();
			}
			else
			{
				Debug.LogWarning("WWWUtils::GetCountryCode Error " + www.error);
			}

			isGetCountryCodeWork = false;
		}

		IEnumerator GetDataTime(Action<DateTime> callback)
		{
			isGetDataTimeWork = true;
			string dateTimeUrl = "http://appapix" + dateTimeIDs[curDateTimeID].ToString() + ".appsministry.mobi/time.php";
			WWW www = new WWW(dateTimeUrl);
			yield return www;

			if (string.IsNullOrEmpty(www.error) && www.text.StartsWith("{"))
			{
                TimeDataData data = JsonConvert.DeserializeObject<TimeDataData>(www.text);
				TimeData = UnixTimeStampToDateTime(data.time);

				if (callback != null)
					callback(TimeData);
			}
			else
			{
				Debug.LogWarning("WWWUtils::GetDataTime Error " + www.error);
				curDateTimeID++;
				if (curDateTimeID < dateTimeIDs.Length)
					StartCoroutine(GetDataTime(callback));
			}

			isGetDataTimeWork = false;
		}

		IEnumerator GetGameData(Action<GameWebData> callback, Action error = null)
		{
			isGetGameDataWork = true;
            //WWW download = WWW.LoadFromCacheOrDownload(AppsConfiguration.dataUrl, AppsConfiguration.dataUrlCrc);
            WWWForm post = new WWWForm();
            post.AddField("language", Core.Localization.Localization.Instance.Locale);
			WWW download = new WWW(AppsConfiguration.dataUrl,post);
			yield return download;

			if (string.IsNullOrEmpty(download.error) &&(download.text.StartsWith("{") || download.text.StartsWith("[")))
			{
				GameWebData data = new GameWebData();
				data = JsonConvert.DeserializeObject<GameWebData>("{ Data:" + download.text + "}");
				isGetGameDataWork = false;
				if (callback != null)
					callback(data);
			}
			else
			{
				Debug.LogWarning("WWWUtils::GetGameData Error " + download.error);// TODO Нужен попап что не можем скачать!
                if (error != null)
                    error();
				isGetGameDataWork = false;
				//StartCoroutine(GetGameData(callback));
			}
			isGetGameDataWork = false;
			download.Dispose();
			download = null;
		}

		IEnumerator GetTextureData(string texName, string id, Action<Texture2D,string> callback, Action error)
		{
			isGetTextureDataWork = true;
			//WWW download = WWW.LoadFromCacheOrDownload(AppsConfiguration.dataPixUrl + texName, AppsConfiguration.dataUrlCrc);
			WWW download = new WWW(AppsConfiguration.dataPixUrl + texName);
			while (!download.isDone)
			{
				if (DowloadProgress != null)
				{
					DowloadProgress(download.progress);
				}
				yield return new WaitForSeconds(.1f);
			}

			yield return download;
			
			if (string.IsNullOrEmpty(download.error))
			{
				isGetTextureDataWork = false;
				if (callback != null)
					callback(download.texture, id);
				if (DowloadProgress != null)
					DowloadProgress(download.progress);
				//Destroy(download.texture);
			}
			else
			{
				Debug.LogWarning("WWWUtils::GetTextureData Error " + download.error);// TODO Нужен попап что не можем скачать!
                if (error != null)
                    error();
				isGetTextureDataWork = false;
				//StartCoroutine(GetTextureData(texName, id, callback));
			}
			isGetTextureDataWork = false;
			download.Dispose();
			download = null;
		}

		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public static long ConvertToTimestamp(DateTime value)
		{
			TimeSpan elapsedTime = value - Epoch;
			return (long)elapsedTime.TotalSeconds;
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			DateTime dtDateTime = Epoch;
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
			return dtDateTime;
		}
	}
}
