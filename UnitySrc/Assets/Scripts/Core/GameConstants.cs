
namespace Project.Core
{
	public class Service
	{
		public const string pushServerApiKey = "af8cd137d2bd194e8e979981c514b449";

		public const string iosVoteURL = "http://rate.appsministry.com/apple/slugterra/vote";
		public const string androidGooglePlayVoteURL = "http://rate.appsministry.com/android/slugterra_gp";
		public const string androidAmazonVoteURL = "http://rate.appsministry.com/android/slugterra_gp";

		public const string feedbackMailAddress = "support@appsministry.com";

		public const string googlePlayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwalJH+sXbT8++wOUgsj61xw5ksRag/9cDBsgqvejh7RpCaUsO5SMaw73Stxe3pm/DAcfEOUph8ueGxgeV08gaxjhhGG5Qquy4JpTKD1PuBvbWny+kvbzX5Icu4nfSVs4/vyNc1cQ2eqNBU+hArPa7v6FuDiIblNvBT381Z2N6P3MdeohJdNTWiHB/PC2GT1FHPO77u4Ip66aVoAOeKCFOlhC6y76hdGnfvL6VB7MBoqQX9At+71LTTbmAlJI7yWvXQf6UcxMR653j8KjHuvZgGiarAMw/Nwv9S8KqG8G1keHpN+LR9tOY8Oa2zm7dTInYlf38MHR/scovXm/gOeA0QIDAQAB";
	}

	public class LocalizationServiceKeys
	{
		public const string alertInternetErrorTitle = "alert_internet_error_title";
		public const string alertInternetErrorTitleDefault = "Error";

		public const string alertInternetErrorBody = "alert_internet_error_body";
		public const string alertInternetErrorBodyDefault = "You are in off-line mode. You need a connection to the Internet to perform this action.";

		public const string alertInternetErrorText = "alert_internet_error_text";
		public const string alertInternetErrorTextDefault = "Try again later.";

		public const string noItemsForRestored = "no_items_for_restored";
		public const string noItemsForRestoredDefault = "Unable to restore any purchases";

		public const string itemsRestored = "items_restored";
		public const string itemsRestoredDefault = "purchase(s) restored";

		public const string itemAvailableAfter = "menu_item_unlock_title";
	}

	public class AppsConfiguration
	{
#if UNITY_IOS || UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE || UNITY_TVOS
		public const int RES_SERVICE_ID = 249;
		public const int OTHER_APPS_RES_SERVICE_ID = 256;
#elif UNITY_ANDROID
		public const int RES_SERVICE_ID = 278;
		public const int OTHER_APPS_RES_SERVICE_ID = 257;
#endif

		public const string API_URL = "http://appapi.neoline.biz/";
		public const string OTHER_APPS = "advertising/list?service_id={0}&heigth={1}&width={2}";

		public const string RES_URL = "platform/v1/jsonRpc";

		public const string pingUrl = "http://google.com";

		public const string coutryCodeUrl = "http://appapi.neoline.biz/platform/helpers/ip.json";
		public const string dateTimeUrl = "http://appapix10.appsministry.mobi/time.php";
		//public const string dataUrl = "http://guessthemovie.mcdir.ru/get.php"; //Тут скрипт базы с фильмами.
		public const string dataUrl = "http://guesstheobject.ru/serials/get.php";
		//public const string dataPixUrl = "http://guessthemovie.mcdir.ru/uploads/"; //Тут скрины с фильмами.
		public const string dataPixUrl = "http://guesstheobject.ru/serials/images/";
		public const int dataUrlCrc = 1;
	}

	public class AppVersion
	{
		public const int major = 0;
		public const int minor = 0;
		public const int build = 1;
	}
	public class StoredInPlayerPrefsParameters
	{
		public const string currentLanguage = "selectedLanguage";
		public const int secPerRound = 10;
		public const int secAllGame = 180;
		public const int secLastChance = 30;
		public const int scorePerRound = 25;
		public const int bonusPerSec = 6;
	}
}
