using UnityEngine;

namespace EdyCommonTools;

public static class ApplicationUtility
{
	public static bool IsActivated()
	{
		return Application.isFocused;
	}
}
