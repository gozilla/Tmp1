
using Project.App;

namespace Project.UI.InGameMenu
{
	public class EndMenuController : UIController<EndMenuView>
	{
		protected override void OnStart()
		{
			base.OnStart();

		}

		protected override void _OnEnable()
		{
			View.OnQuitEvent += OnQuit;
			View.OnNewEvent += OnNew;
			SetScore();
			SetHints();

            Advertising.AdsManager.Instance.ShowInterestial();
		}

		protected override void _OnDisable()
		{
			View.OnQuitEvent -= OnQuit;
			View.OnNewEvent -= OnNew;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
		}

		private void OnQuit()
		{
			View.MainMenu.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}

		private void OnNew()
		{
			gameObject.SetActive(false);
			View.MainMenu.OnBtnStart();
		}

		private void SetScore()
		{
			int bestScore = AppData.Instance.PlayerBestScore;
			int newScore = AppData.Instance.PlayerScore;

			bool newRecord = bestScore < newScore;
			View.NewScore.SetActive(newRecord);
			View.BestScore.text = bestScore.ToString();
			View.YourScore.text = newScore.ToString();

			if (newRecord)
			{
				AppData.Instance.PlayerBestScore = newScore;
			}
		}

		private void SetHints()
		{
			View.Hint1.SetActive(!AppData.Instance.Hint50Active);
			View.Hint2.SetActive(!AppData.Instance.HintNextActive);
		}
	}
}

