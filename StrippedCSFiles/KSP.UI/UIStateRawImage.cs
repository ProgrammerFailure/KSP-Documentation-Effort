using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public class UIStateRawImage : MonoBehaviour
{
	[Serializable]
	public class ImageState
	{
		public string name;

		public Texture texture;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ImageState()
		{
			throw null;
		}
	}

	public RawImage image;

	public ImageState[] states;

	public string startState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIStateRawImage()
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
	public void SetState(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(string name)
	{
		throw null;
	}
}
