using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyFullScreen : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
#if UNITY_WSA
        if (!Screen.fullScreen)
            Screen.fullScreen = true;
#endif
    }
}
