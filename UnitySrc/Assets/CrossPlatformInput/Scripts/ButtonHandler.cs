using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput {
    public class ButtonHandler : MonoBehaviour {
        public string Name;
        public bool DisableAfterSetUpState = false;

        [Header("Audio")]
        [SerializeField]
        private AudioClip buttonDownClip = null;
        [SerializeField]
        private AudioClip buttonUpClip = null;
		
        AudioSource audioSource = null;
        bool canSetUpState = true;
        Button button;

        void Awake() {
            button = GetComponent<Button>();
            audioSource = GetComponent<AudioSource>();
        }

        void OnEnable() {
            canSetUpState = true;
            if (button != null) {
                button.enabled = true;
            }
        }

        void OnDisable() {
			CrossPlatformInputManager.UnRegisterVirtualButton(Name);
        }

        void PlayAudioClip(AudioClip clip) {
            if (clip != null && audioSource != null) {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        public void SetDownState() {
			CrossPlatformInputManager.SetButtonDown(Name);
			PlayAudioClip(buttonDownClip);
        }

        IEnumerator SetUpStateWithDelay() {
            if (button != null) {
                button.enabled = false;
            }
            yield return new WaitForSeconds(0.2f);
            CrossPlatformInputManager.SetButtonUp(Name);
            canSetUpState = true;
        }

        public void SetUpState() {
			if (DisableAfterSetUpState) {
				if (canSetUpState) {
					canSetUpState = false;
					StartCoroutine(SetUpStateWithDelay());
				}
			}
			else {
				CrossPlatformInputManager.SetButtonUp(Name);
			}
			PlayAudioClip(buttonUpClip);
        }

        public void SetAxisPositiveState() {
            CrossPlatformInputManager.SetAxisPositive(Name);
        }

        public void SetAxisNeutralState() {
            CrossPlatformInputManager.SetAxisZero(Name);
        }

        public void SetAxisNegativeState() {
            CrossPlatformInputManager.SetAxisNegative(Name);
        }
    }
}
