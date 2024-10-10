using System;
using System.Collections.Generic;
using Expansions;
using UnityEngine;

namespace ns25;

public class AddDebugScreens : MonoBehaviour
{
	[Serializable]
	public class ScreenWrapper
	{
		public string parentName;

		public string name;

		public string text;

		public string expansionNames;

		public RectTransform screen;
	}

	public List<ScreenWrapper> screens = new List<ScreenWrapper>();

	public void Start()
	{
		int i = 0;
		for (int count = screens.Count; i < count; i++)
		{
			ScreenWrapper screenWrapper = screens[i];
			bool flag = false;
			if (string.IsNullOrEmpty(screenWrapper.expansionNames))
			{
				flag = true;
			}
			else
			{
				string[] array = screenWrapper.expansionNames.Split(',');
				for (int j = 0; j < array.Length; j++)
				{
					if (ExpansionsLoader.IsExpansionInstalled(array[j]))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				DebugScreen.AddContentScreen(screenWrapper.parentName, screenWrapper.name, screenWrapper.text, screenWrapper.screen);
			}
		}
	}
}
