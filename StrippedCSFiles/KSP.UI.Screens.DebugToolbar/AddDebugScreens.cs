using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens.DebugToolbar;

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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ScreenWrapper()
		{
			throw null;
		}
	}

	public List<ScreenWrapper> screens;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AddDebugScreens()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}
}
