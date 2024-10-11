using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AtlasSplitter : MonoBehaviour
{
	[Serializable]
	public class Atlas
	{
		public Texture2D[] tex;

		public int xCount;

		public int yCount;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Atlas()
		{
			throw null;
		}
	}

	public Atlas[] atlases;

	public string outputPath;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AtlasSplitter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}
}
