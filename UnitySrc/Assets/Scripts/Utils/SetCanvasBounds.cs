using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class SetCanvasBounds : MonoBehaviour {

#if UNITY_IPHONE
	[DllImport("__Internal")]
    private extern static void GetSafeAreaImpl(out float x, out float y, out float w, out float h);
#endif

    private Rect GetSafeArea() {
        float x, y, w, h;
#if UNITY_IPHONE
		GetSafeAreaImpl(out x, out y, out w, out h);
#else
        x = 0;
        y = 0;
        w = Screen.width;
        h = Screen.height;
#endif
        if (horizontalOnly) {
            y = 0;
            h = Screen.height;
        }
        return new Rect(x, y, w, h);
    }

    public RectTransform panel;
    public RectTransform framePrefab;
    public bool horizontalOnly = true;
    Rect lastSafeArea = new Rect(0, 0, 0, 0);
    CanvasScaler canvasScaler = null;

    void ApplySafeArea(Rect area) {
        var anchorMin = area.position;
        var anchorMax = area.position + area.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;

        lastSafeArea = area;
    }

    bool IsTooWideScreen {
        get {
            return (Screen.width / Screen.height) >= 2;
        }
    }

    void Update() {
        if (IsTooWideScreen) { // iPhone X
            Rect safeArea = GetSafeArea(); // or Screen.safeArea if you use a version of Unity that supports it

            if (canvasScaler == null) {
                if (framePrefab != null) {
                    Instantiate(framePrefab, panel, false);
                }
                canvasScaler = GetComponent<CanvasScaler>();
                if (canvasScaler.referenceResolution.y > 0) {
                    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                    canvasScaler.scaleFactor = safeArea.height / canvasScaler.referenceResolution.y;
                }
                else {
                    canvasScaler.matchWidthOrHeight = 1;
                }
            }
            if (safeArea != lastSafeArea) {
                ApplySafeArea(safeArea);
            }
        }
    }
}
