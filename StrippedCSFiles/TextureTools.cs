using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("KSP/Texture Tools")]
public class TextureTools : MonoBehaviour
{
	public class Vector3Curve
	{
		private AnimationCurve x;

		private AnimationCurve y;

		private AnimationCurve z;

		public float minTime
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		public float maxTime
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3Curve()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Add(float time, Vector3 value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Add(float time, Color value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 EvaluateVector(float time)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Color EvaluateColor(float time)
		{
			throw null;
		}
	}

	public class Vector4Curve
	{
		private AnimationCurve x;

		private AnimationCurve y;

		private AnimationCurve z;

		private AnimationCurve w;

		public float minTime
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		public float maxTime
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector4Curve()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Add(float time, Vector4 value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Add(float time, Color value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector4 EvaluateVector(float time)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Color EvaluateColor(float time)
		{
			throw null;
		}
	}

	public string outputFilename;

	public string inputPath;

	public string fileName;

	public string fileExtension;

	public string suffixDiffuse;

	public string suffixSpecular;

	public string suffixOcclusion;

	public string suffixNormal;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TextureTools()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Convert!")]
	public void Convert()
	{
		throw null;
	}
}
