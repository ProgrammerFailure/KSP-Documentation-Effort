using System.Runtime.CompilerServices;
using UnityEngine;

public class MapSO : ScriptableObject
{
	public class HeightAlpha
	{
		public float height;

		public float alpha;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public HeightAlpha()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public HeightAlpha(float height, float alpha)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static HeightAlpha Lerp(HeightAlpha a, HeightAlpha b, float dt)
		{
			throw null;
		}
	}

	public enum MapDepth
	{
		Greyscale = 1,
		HeightAlpha,
		RGB,
		RGBA
	}

	[SerializeField]
	protected bool _isCompiled;

	[SerializeField]
	protected string _name;

	[SerializeField]
	protected int _width;

	[SerializeField]
	protected int _height;

	[SerializeField]
	protected int _bpp;

	[SerializeField]
	[HideInInspector]
	protected byte[] _data;

	[SerializeField]
	protected int _rowWidth;

	protected static float Byte2Float;

	protected static float Float2Byte;

	protected float centerX;

	protected float centerY;

	protected float midX;

	protected float midY;

	protected int minX;

	protected int maxX;

	protected int minY;

	protected int maxY;

	protected int index;

	protected int itr;

	protected float retVal;

	protected byte val;

	protected double centerXD;

	protected double centerYD;

	protected double midXD;

	protected double midYD;

	protected double retValD;

	public bool IsCompiled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string MapName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Width
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Height
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int BitsPerPixel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int RowWidth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MapDepth Depth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Size
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string SizeString
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapSO()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MapSO()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CreateMap(MapDepth depth, Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CreateMap(MapDepth depth, string name, int width, int height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color[] CreateMapRGBQuantized(Texture2D tex, float quantize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color[] CreateMapRGBQuantized(Texture2D tex, int quantize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CreateMapRGBQuantized(Texture2D tex, Color[] quantize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateRGB(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Color[] CreateRGBQuantized(Texture2D tex, int quantize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Color[] CreateRGBQuantized(Texture2D tex, float quantize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateRGBQuantized(Texture2D tex, Color[] quantize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateRGBA(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateGreyscaleFromRGB(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateGreyscaleFromAlpha(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateHeightAlpha(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected int PixelIndex(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Texture2D CompileToTexture()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Texture2D CompileGreyscale()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Texture2D CompileHeightAlpha()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Texture2D CompileRGB()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Texture2D CompileRGBA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Texture2D CompileToTexture(byte filter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual byte GreyByte(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual byte[] PixelByte(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GreyFloat(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual byte GetPixelByte(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetPixelFloat(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetPixelColor(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color32 GetPixelColor32(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual HeightAlpha GetPixelHeightAlpha(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetPixelColor(float x, float y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetPixelColor32(float x, float y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetPixelFloat(float x, float y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual HeightAlpha GetPixelHeightAlpha(float x, float y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetPixelColor(double x, double y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetPixelColor32(double x, double y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetPixelFloat(double x, double y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual HeightAlpha GetPixelHeightAlpha(double x, double y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetPixel(int x, int y, byte b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetPixel(float x, float y, byte b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ConstructBilinearCoords(float x, float y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ConstructBilinearCoords(double x, double y)
	{
		throw null;
	}
}
