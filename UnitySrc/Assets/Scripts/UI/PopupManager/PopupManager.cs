using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Project.Utils;

namespace Project.UI {
    public class PopupManager : DontDestroyMonoSingleton<PopupManager> {
        private Dictionary<Type, PopupView> _availablePopups = new Dictionary<Type, PopupView>();
        private LinkedList<PopupView> _popups = new LinkedList<PopupView>();

        public bool IsShowing {
            get { return _popups.Count > 0; }
        }

		private void Awake()
		{
			int childCount = gameObject.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				PopupView popup = gameObject.transform.GetChild(i).GetComponent<PopupView>();
				if (popup != null)
				{
					RegisterPopup(popup);
				}
			}
		}

		public void RegisterPopup(PopupView popup)
		{
            if (popup != null)
                _availablePopups[popup.GetType()] = popup;
        }

        public void UnRegisterPopup(PopupView popup) {
            if (popup != null) {
                if (_availablePopups.ContainsKey(popup.GetType()))
                    _availablePopups.Remove(popup.GetType());
            }
        }

        public PopupView GetRegisteredPopup(Type popupType) {
            PopupView popup;
            if (_availablePopups.TryGetValue(popupType, out popup))
                return popup;

            return null;
        }

        public T GetRegisteredPopup<T>() where T : PopupView {
            PopupView popup = GetRegisteredPopup(typeof(T));

            if (popup != null)
                return popup as T;

            return null;
        }

        public void OnShowPopup(PopupView popup) {
            if (popup != null)
                _popups.AddLast(popup);
        }

        public void OnClosePopup(PopupView popup) {
            if (_popups.Count != 0 && popup != null) {
                _popups.Remove(popup);
            }
        }

        public PopupView GetUpper() {
            if (_popups.Count != 0) {
                var node = _popups.Last;
                if (node != null)
                    return node.Value;
            }
            return null;
        }

        public void CloseCurrentPopup() {
            if (_popups.Count != 0) {
                var node = _popups.Last;
                if (node != null)
                    node.Value.Close();
            }
        }

        public void Clear() {
            //_availablePopups.Clear();
            _popups.Clear();
        }
    }
}
