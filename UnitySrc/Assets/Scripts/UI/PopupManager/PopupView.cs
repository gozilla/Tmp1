using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.UI
{
    public abstract class PopupView : UIView {
        protected virtual void OnCreate() { }
        private void Awake() {
            PopupManager.Instance.RegisterPopup(this);
            OnCreate();
        }

        protected virtual void OnEnable() {
            PopupManager.Instance.OnShowPopup(this);
        }

        protected virtual void OnDisable() {
            if (PopupManager.IsAlive)
                PopupManager.Instance.OnClosePopup(this);
        }

        public abstract void Close();

        protected override void OnReleaseResource() {
            base.OnReleaseResource();
            if (PopupManager.IsAlive) {
                PopupManager.Instance.UnRegisterPopup(this);
            }
        }
    }
}
