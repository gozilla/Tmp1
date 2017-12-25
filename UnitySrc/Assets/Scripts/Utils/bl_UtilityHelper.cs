using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_5_3|| UNITY_5_4 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using PathologicalGames;
using System;

public class bl_UtilityHelper
{

    public static void LoadLevel(string scene)
    {
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_3_OR_NEWER
        SceneManager.LoadScene(scene);
#else
        Application.LoadLevel(scene);
#endif
    }

    public static void LoadLevel(int scene)
    {
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_3_OR_NEWER
        SceneManager.LoadScene(scene);
#else
        Application.LoadLevel(scene);
#endif
    }

    /// <summary>
    /// Call this to capture a custom, screenshot
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Texture2D CaptureCustomScreenshot(int width, int height)
    {
        Texture2D textured = new Texture2D(width, height, TextureFormat.RGB24, true, false);
        textured.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
        int miplevel = Screen.width / 800;
        Texture2D textured2 = new Texture2D(width >> miplevel, height >> miplevel, TextureFormat.RGB24, false, false);
        textured2.SetPixels32(textured.GetPixels32(miplevel));
        textured2.Apply();
        return textured2;
    }
    /// <summary>
    /// Call this to capture a screenshot Automatic size
    /// </summary>
    /// <returns></returns>
    public static byte[] CaptureScreenshot()
    {
        Texture2D textured = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false, false);
        textured.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0);
        return textured.EncodeToPNG();
    }
    /// <summary>
    /// Call this to capture a custom size screenshot
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static byte[] CaptureScreenshot(int width, int height)
    {
        Texture2D textured = new Texture2D(width, height, TextureFormat.RGB24, false, false);
        textured.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
        return textured.EncodeToPNG();
    }

    /// <summary>
    /// Get ClampAngle
    /// </summary>
    /// <param name="ang"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float ClampAngle(float ang, float min, float max)
    {
        if (ang < -360f)
        {
            ang += 360f;
        }
        if (ang > 360f)
        {
            ang -= 360f;
        }
        return Mathf.Clamp(ang, min, max);
    }

    /// <summary>
    /// Obtained distance between two positions.
    /// </summary>
    /// <param name="posA"></param>
    /// <param name="posB"></param>
    /// <returns></returns>
    public static float GetDistance(Vector3 posA, Vector3 posB)
    {
        return Vector3.Distance(posA, posB);
    }

    /// <summary>
    /// obtain only the first two values
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static string GetDoubleChar(float f)
    {
        return f.ToString("00");
    }
    /// <summary>
    /// obtain only the first three values
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static string GetThreefoldChar(float f) {
        return f.ToString("000");
    }

    public static string GetTimeFormat(float m, float s) {
        return string.Format("{0:00}:{1:00}", m, s);
    }

    public static string GetTimeFormat(float h, float m, float s) {
        return string.Format("{0:00}:{1:00}:{2:00}", h, m, s);
    }

	public static string GetTimeFormat(TimeSpan span)
	{
		int hours = span.Days * 24 + span.Hours;

		return string.Format("{0:00}:{1:00}:{2:00}", hours, span.Minutes, span.Seconds);
	}

	private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static double GetUnixTimestamp(DateTime value) {
        return value.Subtract(Epoch).TotalSeconds;
    }

    public static DateTime GetDateTime(double unixTimeStamp) {
        return Epoch.AddSeconds(unixTimeStamp).ToUniversalTime();
    }

    public static string priceSep = ".";
    public static string FormatPrice(int price) {
        int highPrice = price / 100;
        int lowPrice = price % 100;
        return highPrice + priceSep + lowPrice;
    }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="force"></param>
	/// <returns></returns>
	public static Vector3 CorrectForceSize(Vector3 force)
    {
        float num = (1.2f / Time.timeScale) - 0.2f;
        force = (Vector3)(force * num);
        return force;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="option"></param>
    public static void ShadowLabel(string text, params GUILayoutOption[] option)
    {
        Color color = GUI.color;
        Color black = Color.black;
        black.a = color.a;
        GUI.color = black;
        GUILayout.Label(text, option);
        Rect lastRect = GUILayoutUtility.GetLastRect();
        lastRect.x--;
        lastRect.y--;
        GUI.color = color;
        GUI.Label(lastRect, text);
    }
    public static void ShadowLabel(Rect rect, string text)
    {
        ShadowLabel(rect, text, null);
    }

    public static void ShadowLabel(string text, GUIStyle style, params GUILayoutOption[] option)
    {
        Color color = GUI.color;
        Color black = Color.black;
        black.a = color.a;
        GUI.color = black;
        GUILayout.Label(text, style, option);
        Rect lastRect = GUILayoutUtility.GetLastRect();
        lastRect.x--;
        lastRect.y--;
        GUI.color = color;
        GUI.Label(lastRect, text, style);
    }

    public static void ShadowLabel(Rect rect, string text, GUIStyle style)
    {
        Rect position = new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height);
        Color color = GUI.color;
        Color color2 = !(color == Color.black) ? Color.black : Color.white;
        color2.a = color.a;
        GUI.color = color2;
        if (style != null)
        {
            GUI.Label(position, text, style);
        }
        else
        {
            GUI.Label(position, text);
        }
        GUI.color = color;
        if (style != null)
        {
            GUI.Label(rect, text, style);
        }
        else
        {
            GUI.Label(rect, text);
        }
    }
    /// <summary>
    /// Helper for Cursor locked in Unity 5
    /// </summary>
    /// <param name="mLock">cursor state</param>
    public static void LockCursor(bool mLock)
    {
#if !MOBILE_INPUT
        if (mLock == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool GetCursorState
    {
        get
        {
#if !MOBILE_INPUT
			if (Cursor.visible && Cursor.lockState != CursorLockMode.Locked)
            {
                return false;
            }
            else
#endif
            {
                return true;
            }
        }
    }

    // The angle between dirA and dirB around axis
    public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
        // Project A and B onto the plane orthogonal target axis
        dirA = dirA - Vector3.Project(dirA, axis);
        dirB = dirB - Vector3.Project(dirB, axis);

        // Find (positive) angle between A and B
        float angle = Vector3.Angle(dirA, dirB);

        // Return angle multiplied with 1 or -1
        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
    }

    public static int CalcAttackRatio(float bpc, float fr, float rt, float d) {
        return Mathf.RoundToInt((((60 / ((bpc / fr) + rt)) * bpc) * d) / 60);
    }
}