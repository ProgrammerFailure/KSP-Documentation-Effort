using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public class UIStateImage : MonoBehaviour
{
	[Serializable]
	public class ImageState
	{
		public string name;

		public Sprite sprite;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ImageState()
		{
			throw null;
		}
	}

	public Image image;

	public ImageState[] states;

	public Sprite paused;

	public string startState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIStateImage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGamePause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGameUnPause()
	{
		throw null;
	}
}
