using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Tools/Altitude Multitexture Ramp Creator")]
public class AltitudeMultiTextureRamp : MonoBehaviour
{
	[Serializable]
	public class LerpRange
	{
		public string name;

		public float startStart;

		public float startEnd;

		public float endStart;

		public float endEnd;

		[HideInInspector]
		public float startDelta;

		[HideInInspector]
		public float endDelta;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LerpRange()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LerpRange(float startStart, float startEnd, float endStart, float endEnd)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public float Lerp(float point)
		{
			throw null;
		}
	}

	public int resolution;

	public string filename;

	public LerpRange[] textures;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AltitudeMultiTextureRamp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Create Texture ARGB")]
	public void CreateTextureARGB()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Create Texture GS")]
	public void CreateTextureGS()
	{
		throw null;
	}
}
