using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[Serializable]
public class CBTextureAtlasSO : MapSO
{
	public class CBTextureAtlasPoint
	{
		public List<int> TextureAtlasIndices;

		public List<float> TextureAtlasStrengths;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CBTextureAtlasPoint()
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CBTextureAtlasSO()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CBTextureAtlasPoint GetPixelCBTextureAtlasPoint(double x, double y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual CBTextureAtlasPoint GetCBTextureAtlasPoint(double lat, double lon)
	{
		throw null;
	}
}
