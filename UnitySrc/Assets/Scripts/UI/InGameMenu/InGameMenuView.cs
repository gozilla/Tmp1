using System;

using UnityEngine;
using Project.UI.MainMenu;
using UnityEngine.UI;

namespace Project.UI.InGameMenu
{
	[Serializable]
	public class OptionData
	{
		public Image Image;
		public Text Text;
		public Button Button;
		public RectTransform RectTransform;
		public bool IsRight = false;
	}

	[Serializable]
	public class StatusData
	{
		public Text Count;
		public GameObject Panel;
	}

	public class InGameMenuView : UIView
	{
		public event Action OnBackEvent;
		public event Action OnNewEvent;
		public event Action OnPauseOnEvent;
		public event Action OnPauseOffEvent;
        public event Action OnEndGameEvent;
        public event Action OnHintNextEvent;
		public event Action OnHint50Event;

		public bool NeedCapBonus = false;

		[SerializeField]
		private MainMenuController _mainMenu = null;

		public MainMenuController MainMenu
		{
			get { return _mainMenu; }
		}

		[SerializeField]
		private EndMenuController _endMenu = null;

		public EndMenuController EndMenu
		{
			get { return _endMenu; }
		}

		[SerializeField]
		private Image _progressImage = null;

		public Image ProgressImage
		{
			get { return _progressImage; }
		}

		public TimeSpan TimeRemain;

		public TimeSpan BonusTimeRemain;

		[SerializeField]
		private Text _timer = null;

		public Text Timer
		{
			get { return _timer; }
		}

		[SerializeField]
		private GameObject _panelMid = null;

		public GameObject PanelMid
		{
			get { return _panelMid; }
		}

		[SerializeField]
		private GameObject _btnRight = null;

		public GameObject BtnRight
		{
			get { return _btnRight; }
		}

		[SerializeField]
		private GameObject _btnLeft = null;

		public GameObject BtnLeft
		{
			get { return _btnLeft; }
		}

		[SerializeField]
		private HorizontalPanelMovier _panelRight = null;

		public HorizontalPanelMovier PanelRight
		{
			get { return _panelRight; }
		}

		[SerializeField]
		private Image _imageIngameBack = null;

		public Image ImageIngameBack
		{
			get { return _imageIngameBack; }
			set { _imageIngameBack = value; }
		}

		[SerializeField]
		private Image _imageEndGameBack = null;
		public Image ImageEndGameBack
		{
			get { return _imageEndGameBack; }
			set { _imageEndGameBack = value; }
		}

		[SerializeField]
		public Sprite TextureBackSprite
		{
			set
			{
				_imageIngameBack.sprite = value;
				_imageEndGameBack.sprite = value;
			}
		}

		[SerializeField]
		public Vector2 TextureBackVec
		{
			set
			{
				_imageIngameBack.rectTransform.sizeDelta = value;
				_imageEndGameBack.rectTransform.sizeDelta = value;
			}
		}

		[SerializeField]
		private ButtonController _buttonHint50 = null;
		public ButtonController ButtonHint50
		{
			get { return _buttonHint50; }
			set { _buttonHint50 = value; }
		}

		[SerializeField]
		private ButtonController _buttonHintNext = null;
		public ButtonController ButtonHintNext
		{
			get { return _buttonHintNext; }
			set { _buttonHintNext = value; }
		}

		public OptionData[] Options = null;

		public int RAnswersCountBonus1 = 0;
		public int RAnswersCountBonus2 = 0;
		public float Bonus2Time = 0;

		public bool CanLastChance = true;

		[Space]
		[Header("Game Types")]
		public RectTransform ScorePanel = null;
		public RectTransform BonusPanel1 = null;
		public RectTransform BonusPanel2 = null;

		[SerializeField]
		public Text _scoreText1 = null;
		public Text _scoreText2 = null;
		public string ScoreText
		{
			set
			{
				_scoreText1.text = value;
				_scoreText2.text = value;
			}
		}
		public StatusData HPData = null;
		public StatusData TimeData = null;


		private void Awake()
		{
		}

		public void OnBtnMenuClick()
		{
			if (OnBackEvent != null )
				OnBackEvent();
		}

		public void OnBtnNewClick()
		{
			if (OnNewEvent != null)
				OnNewEvent();
		}

		public void OnBtnPauseOnClick()
		{
			if (OnPauseOnEvent != null)
				OnPauseOnEvent();
		}

		public void OnBtnPauseOffClick()
		{
			if (OnPauseOffEvent != null)
				OnPauseOffEvent();
		}

        public void OnEndGame()
        {
            if (OnEndGameEvent != null)
                OnEndGameEvent();
        }

        public void OnBtnHint50Click()
		{
			if (OnHint50Event != null)
				OnHint50Event();
		}

		public void OnBtnHintNextClick()
		{
			if (OnHintNextEvent != null)
				OnHintNextEvent();
		}

	}
}

