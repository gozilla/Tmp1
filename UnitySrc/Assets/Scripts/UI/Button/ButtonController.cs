
using Project.App;

namespace Project.UI.InGameMenu
{
	public class ButtonController : UIController<ButtonView>
	{
		protected override void OnStart()
		{
			base.OnStart();
		}

		protected override void _OnEnable()
		{
			//View.OnNewEvent += OnQuit;
		}

		protected override void _OnDisable()
		{
			//View.OnNewEvent -= OnQuit;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
		}

		public void SetActivity(bool state)
		{
			View.Button.interactable = state;
			View.BgInactive.SetActive(!state);
		}
	}
}

