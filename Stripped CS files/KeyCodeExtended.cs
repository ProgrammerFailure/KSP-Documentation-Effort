using System;
using UnityEngine;

public class KeyCodeExtended
{
	public KeyCode code;

	public string name;

	public bool isNone
	{
		get
		{
			if (code == KeyCode.None)
			{
				return string.IsNullOrEmpty(name);
			}
			return false;
		}
	}

	public KeyCodeExtended()
	{
		code = KeyCode.None;
		name = "";
	}

	public KeyCodeExtended(KeyCode key)
	{
		code = key;
		name = "";
	}

	public KeyCodeExtended(string key)
	{
		try
		{
			code = (KeyCode)Enum.Parse(typeof(KeyCode), key);
			name = "";
		}
		catch (ArgumentException)
		{
			code = KeyCode.None;
			name = key;
		}
	}

	public KeyCodeExtended(KeyCode key, string keyName)
	{
		code = key;
		name = keyName;
	}

	public override string ToString()
	{
		if (!string.IsNullOrEmpty(name))
		{
			return name;
		}
		return code.ToString();
	}
}
