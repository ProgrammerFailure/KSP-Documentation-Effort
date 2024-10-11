using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro.SpriteAssetUtilities;

public class TexturePacker
{
	[Serializable]
	public struct SpriteFrame
	{
		public float x;

		public float y;

		public float w;

		public float h;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override string ToString()
		{
			throw null;
		}
	}

	[Serializable]
	public struct SpriteSize
	{
		public float w;

		public float h;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override string ToString()
		{
			throw null;
		}
	}

	[Serializable]
	public struct SpriteData
	{
		public string filename;

		public SpriteFrame frame;

		public bool rotated;

		public bool trimmed;

		public SpriteFrame spriteSourceSize;

		public SpriteSize sourceSize;

		public Vector2 pivot;
	}

	[Serializable]
	public class SpriteDataObject
	{
		public List<SpriteData> frames;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SpriteDataObject()
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TexturePacker()
	{
		throw null;
	}
}
