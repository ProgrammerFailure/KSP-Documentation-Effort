using System.Collections.Generic;
using UnityEngine;

public class ExtendedInput : MonoBehaviour
{
	public static string CurrentInputString = "";

	public static string LastInputString = "";

	public float EmtpyFrames;

	public float FrameThreshold;

	public static KeyCode LastKeyDetected = KeyCode.None;

	public void LateUpdate()
	{
		LastInputString = CurrentInputString;
		if (string.IsNullOrEmpty(Input.inputString) && EmtpyFrames <= FrameThreshold)
		{
			EmtpyFrames += Time.deltaTime;
			return;
		}
		CurrentInputString = Input.inputString.ToUpper();
		FrameThreshold = (CurrentInputString.Equals(LastInputString) ? 0.02f : 0.48f);
		EmtpyFrames = 0f;
	}

	public static bool GetKey(KeyCodeExtended key)
	{
		if (Input.GetKey(key.code))
		{
			return true;
		}
		return IsPressed(key.name);
	}

	public static bool GetKeyDown(KeyCodeExtended key)
	{
		if (Input.GetKeyDown(key.code))
		{
			return true;
		}
		if (!WasPressed(key.name))
		{
			return IsPressed(key.name);
		}
		return false;
	}

	public static bool GetKeyUp(KeyCodeExtended key)
	{
		if (Input.GetKeyUp(key.code))
		{
			return true;
		}
		if (WasPressed(key.name))
		{
			return !IsPressed(key.name);
		}
		return false;
	}

	public static bool WasPressed(string key)
	{
		if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(LastInputString))
		{
			return LastInputString.Contains(key.ToUpper());
		}
		return false;
	}

	public static bool IsPressed(string key)
	{
		if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(CurrentInputString))
		{
			return CurrentInputString.Contains(key.ToUpper());
		}
		return false;
	}

	public static bool DetectKeyDown(List<KeyCode> keyValues, out KeyCodeExtended key)
	{
		key = new KeyCodeExtended();
		if (keyValues == null)
		{
			Debug.LogError("Key values collection is null.");
			return false;
		}
		int num = 0;
		int count = keyValues.Count;
		while (true)
		{
			if (num < count)
			{
				if (Input.GetKeyDown(keyValues[num]))
				{
					break;
				}
				num++;
				continue;
			}
			if (!Input.GetKey(LastKeyDetected) && !string.IsNullOrEmpty(CurrentInputString) && !CurrentInputString.Equals(LastInputString))
			{
				int index = CurrentInputString.Length - 1;
				if (CurrentInputString[index] != '\b' && CurrentInputString[index] != '\n')
				{
					key.name = CurrentInputString[index].ToString();
					if (string.IsNullOrEmpty(LastInputString))
					{
						return true;
					}
					int index2 = LastInputString.Length - 1;
					if (LastInputString[index2] != CurrentInputString[index])
					{
						return true;
					}
				}
			}
			return false;
		}
		key.code = keyValues[num];
		LastKeyDetected = keyValues[num];
		return true;
	}
}
