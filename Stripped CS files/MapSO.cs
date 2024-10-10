using System;
using System.Collections.Generic;
using UnityEngine;

public class MapSO : ScriptableObject
{
	public class HeightAlpha
	{
		public float height;

		public float alpha;

		public HeightAlpha()
		{
			height = 0f;
			alpha = 0f;
		}

		public HeightAlpha(float height, float alpha)
		{
			this.height = height;
			this.alpha = alpha;
		}

		public static HeightAlpha Lerp(HeightAlpha a, HeightAlpha b, float dt)
		{
			return new HeightAlpha(a.height + (b.height - a.height) * dt, a.alpha + (b.alpha - a.alpha) * dt);
		}
	}

	public enum MapDepth
	{
		Greyscale = 1,
		HeightAlpha,
		const_2,
		RGBA
	}

	[SerializeField]
	public bool _isCompiled;

	[SerializeField]
	public string _name;

	[SerializeField]
	public int _width;

	[SerializeField]
	public int _height;

	[SerializeField]
	public int _bpp;

	[SerializeField]
	[HideInInspector]
	public byte[] _data;

	[SerializeField]
	public int _rowWidth;

	public static float Byte2Float = 0.003921569f;

	public static float Float2Byte = 255f;

	public float centerX;

	public float centerY;

	public float midX;

	public float midY;

	public int minX;

	public int maxX;

	public int minY;

	public int maxY;

	public int index;

	public int itr;

	public float retVal;

	public byte val;

	public double centerXD;

	public double centerYD;

	public double midXD;

	public double midYD;

	public double retValD;

	public bool IsCompiled => _isCompiled;

	public string MapName => _name;

	public int Width => _width;

	public int Height => _height;

	public int BitsPerPixel => _bpp;

	public int RowWidth => _rowWidth;

	public MapDepth Depth => (MapDepth)_bpp;

	public int Size => _data.Length;

	public string SizeString
	{
		get
		{
			if (Size > 1048576)
			{
				return KSPUtil.LocalizeNumber(9.536743E-07f * (float)Size, "F2") + "MB";
			}
			if (Size > 1024)
			{
				return KSPUtil.LocalizeNumber(0.0009765625f * (float)Size, "F1") + "kB";
			}
			return Size + "B";
		}
	}

	public virtual void CreateMap(MapDepth depth, Texture2D tex)
	{
		_name = tex.name;
		_width = tex.width;
		_height = tex.height;
		_bpp = (int)depth;
		_rowWidth = _width * _bpp;
		switch (depth)
		{
		case MapDepth.Greyscale:
			if (tex.format == TextureFormat.Alpha8)
			{
				CreateGreyscaleFromAlpha(tex);
			}
			else
			{
				CreateGreyscaleFromRGB(tex);
			}
			break;
		case MapDepth.HeightAlpha:
			CreateHeightAlpha(tex);
			break;
		case MapDepth.const_2:
			CreateRGB(tex);
			break;
		case MapDepth.RGBA:
			CreateRGBA(tex);
			break;
		}
		_isCompiled = true;
	}

	public virtual void CreateMap(MapDepth depth, string name, int width, int height)
	{
		_name = name;
		_width = width;
		_height = height;
		_bpp = (int)depth;
		_rowWidth = _width * _bpp;
		_data = new byte[_width * _height * _bpp];
		_isCompiled = true;
	}

	public virtual Color[] CreateMapRGBQuantized(Texture2D tex, float quantize)
	{
		_name = tex.name;
		_width = tex.width;
		_height = tex.height;
		_bpp = 3;
		_rowWidth = _width * _bpp;
		_isCompiled = true;
		return CreateRGBQuantized(tex, quantize);
	}

	public virtual Color[] CreateMapRGBQuantized(Texture2D tex, int quantize)
	{
		_name = tex.name;
		_width = tex.width;
		_height = tex.height;
		_bpp = 3;
		_rowWidth = _width * _bpp;
		_isCompiled = true;
		return CreateRGBQuantized(tex, quantize);
	}

	public virtual void CreateMapRGBQuantized(Texture2D tex, Color[] quantize)
	{
		_name = tex.name;
		_width = tex.width;
		_height = tex.height;
		_bpp = 3;
		_rowWidth = _width * _bpp;
		_isCompiled = true;
		CreateRGBQuantized(tex, quantize);
	}

	public void CreateRGB(Texture2D tex)
	{
		Color32[] pixels = tex.GetPixels32();
		int num = pixels.Length * 3;
		_data = new byte[num];
		int i = 0;
		int num2 = 0;
		for (; i < pixels.Length; i++)
		{
			_data[num2++] = pixels[i].r;
			_data[num2++] = pixels[i].g;
			_data[num2++] = pixels[i].b;
		}
	}

	public Color[] CreateRGBQuantized(Texture2D tex, int quantize)
	{
		Color32[] pixels = tex.GetPixels32();
		int num = pixels.Length * 3;
		_data = new byte[num];
		List<Color32> list = new List<Color32>();
		List<int> list2 = new List<int>();
		int num2 = int.MinValue;
		int i = 0;
		for (int num3 = pixels.Length; i < num3; i++)
		{
			Color color = pixels[i];
			color.a = 1f;
			bool flag = false;
			int j = 0;
			for (int count = list.Count; j < count; j++)
			{
				if (list[j] == color)
				{
					int num4 = j;
					int value = list2[num4] + 1;
					list2[num4] = value;
					if (list2[j] > num2)
					{
						num2 = list2[j];
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(color);
				list2.Add(1);
				if (1 > num2)
				{
					num2 = 1;
				}
			}
		}
		Color32[] array = list.ToArray();
		int[] array2 = list2.ToArray();
		int k = 0;
		for (int num5 = array2.Length; k < num5; k++)
		{
			array2[k] = num2 - array2[k];
		}
		Array.Sort(array2, array, 0, array2.Length);
		string text = "";
		int l = 0;
		for (int num6 = array.Length; l < num6; l++)
		{
			text = text + array2[l] + " : " + array[l].ToString() + "\n";
		}
		Debug.Log(text);
		int num7 = Mathf.Min(quantize, array.Length);
		int num8 = 0;
		int m = 0;
		for (int num9 = pixels.Length; m < num9; m++)
		{
			float num10 = float.MaxValue;
			Color color = pixels[m];
			color.a = 1f;
			Color32 color2 = array[0];
			for (int n = 0; n < num7; n++)
			{
				float sqrMagnitude = ((Vector4)(Color)array[n] - (Vector4)color).sqrMagnitude;
				if (sqrMagnitude < num10)
				{
					color2 = array[n];
					num10 = sqrMagnitude;
				}
			}
			_data[num8++] = color2.r;
			_data[num8++] = color2.g;
			_data[num8++] = color2.b;
		}
		Color[] array3 = new Color[num7];
		for (int num11 = 0; num11 < num7; num11++)
		{
			array3[num11] = array[num11];
		}
		return array3;
	}

	public Color[] CreateRGBQuantized(Texture2D tex, float quantize)
	{
		Color[] pixels = tex.GetPixels();
		int num = pixels.Length * 3;
		_data = new byte[num];
		List<Color> list = new List<Color>();
		int i = 0;
		for (int num2 = pixels.Length; i < num2; i++)
		{
			Color color = pixels[i];
			bool flag = true;
			Vector4 vector = color;
			int count = list.Count;
			while (count-- > 0)
			{
				if (!(((Vector4)list[count] - vector).sqrMagnitude >= quantize))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(color);
			}
		}
		int count2 = list.Count;
		int num3 = 0;
		int j = 0;
		for (int num4 = pixels.Length; j < num4; j++)
		{
			float num5 = float.MaxValue;
			Color color = pixels[j];
			Color color2 = list[0];
			for (int k = 0; k < count2; k++)
			{
				Color color3 = list[k];
				float sqrMagnitude = ((Vector4)color3 - (Vector4)color).sqrMagnitude;
				if (sqrMagnitude < num5)
				{
					color2 = color3;
					num5 = sqrMagnitude;
				}
			}
			_data[num3++] = (byte)Mathf.FloorToInt(Float2Byte * color2.r);
			_data[num3++] = (byte)Mathf.FloorToInt(Float2Byte * color2.g);
			_data[num3++] = (byte)Mathf.FloorToInt(Float2Byte * color2.b);
		}
		return list.ToArray();
	}

	public void CreateRGBQuantized(Texture2D tex, Color[] quantize)
	{
		Color[] pixels = tex.GetPixels();
		int num = pixels.Length * 3;
		_data = new byte[num];
		int num2 = quantize.Length;
		int num3 = 0;
		int i = 0;
		for (int num4 = pixels.Length; i < num4; i++)
		{
			float num5 = float.MaxValue;
			Color color = pixels[i];
			Color color2 = quantize[0];
			for (int j = 0; j < num2; j++)
			{
				Color color3 = quantize[j];
				float sqrMagnitude = ((Vector4)color3 - (Vector4)color).sqrMagnitude;
				if (sqrMagnitude < num5)
				{
					color2 = color3;
					num5 = sqrMagnitude;
				}
			}
			_data[num3++] = (byte)Mathf.FloorToInt(Float2Byte * color2.r);
			_data[num3++] = (byte)Mathf.FloorToInt(Float2Byte * color2.g);
			_data[num3++] = (byte)Mathf.FloorToInt(Float2Byte * color2.b);
		}
	}

	public void CreateRGBA(Texture2D tex)
	{
		Color32[] pixels = tex.GetPixels32();
		int num = pixels.Length * 4;
		_data = new byte[num];
		int i = 0;
		int num2 = 0;
		for (; i < pixels.Length; i++)
		{
			_data[num2++] = pixels[i].r;
			_data[num2++] = pixels[i].g;
			_data[num2++] = pixels[i].b;
			_data[num2++] = pixels[i].a;
		}
	}

	public void CreateGreyscaleFromRGB(Texture2D tex)
	{
		Color32[] pixels = tex.GetPixels32();
		int num = pixels.Length;
		_data = new byte[num];
		for (int i = 0; i < num; i++)
		{
			_data[i] = pixels[i].r;
		}
	}

	public void CreateGreyscaleFromAlpha(Texture2D tex)
	{
		Color32[] pixels = tex.GetPixels32();
		int num = pixels.Length;
		_data = new byte[num];
		for (int i = 0; i < num; i++)
		{
			_data[i] = pixels[i].a;
		}
	}

	public void CreateHeightAlpha(Texture2D tex)
	{
		Color32[] pixels = tex.GetPixels32();
		int num = pixels.Length * 2;
		_data = new byte[num];
		int i = 0;
		int num2 = 0;
		for (; i < pixels.Length; i++)
		{
			_data[num2++] = pixels[i].r;
			_data[num2++] = pixels[i].a;
		}
	}

	public int PixelIndex(int x, int y)
	{
		return x * _bpp + y * _rowWidth;
	}

	public virtual Texture2D CompileToTexture()
	{
		return Depth switch
		{
			MapDepth.Greyscale => CompileGreyscale(), 
			MapDepth.HeightAlpha => CompileHeightAlpha(), 
			MapDepth.const_2 => CompileRGB(), 
			MapDepth.RGBA => CompileRGBA(), 
			_ => null, 
		};
	}

	public virtual Texture2D CompileGreyscale()
	{
		int size = Size;
		Color32[] array = new Color32[size];
		for (int i = 0; i < size; i++)
		{
			byte b = _data[i];
			array[i] = new Color32(b, b, b, byte.MaxValue);
		}
		Texture2D texture2D = new Texture2D(Width, Height, TextureFormat.RGB24, mipChain: false);
		texture2D.SetPixels32(array);
		texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		return texture2D;
	}

	public virtual Texture2D CompileHeightAlpha()
	{
		int size = Size;
		Color32[] array = new Color32[Width * Height];
		int num = 0;
		int num2 = 0;
		while (num < size)
		{
			val = _data[num];
			array[num2] = new Color32(val, val, val, _data[num + 1]);
			num += 2;
			num2++;
		}
		Texture2D texture2D = new Texture2D(Width, Height, TextureFormat.RGBA32, mipChain: false);
		texture2D.SetPixels32(array);
		texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		return texture2D;
	}

	public virtual Texture2D CompileRGB()
	{
		int size = Size;
		Color32[] array = new Color32[Width * Height];
		int num = 0;
		int num2 = 0;
		while (num < size)
		{
			array[num2] = new Color32(_data[num], _data[num + 1], _data[num + 2], byte.MaxValue);
			num += 3;
			num2++;
		}
		Texture2D texture2D = new Texture2D(Width, Height, TextureFormat.RGB24, mipChain: false);
		texture2D.SetPixels32(array);
		texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		return texture2D;
	}

	public virtual Texture2D CompileRGBA()
	{
		int size = Size;
		Color32[] array = new Color32[Width * Height];
		int num = 0;
		int num2 = 0;
		while (num < size)
		{
			array[num2] = new Color32(_data[num], _data[num + 1], _data[num + 2], _data[num + 3]);
			num += 4;
			num2++;
		}
		Texture2D texture2D = new Texture2D(Width, Height, TextureFormat.RGBA32, mipChain: false);
		texture2D.SetPixels32(array);
		texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		return texture2D;
	}

	public virtual Texture2D CompileToTexture(byte filter)
	{
		int size = Size;
		Color32[] array = new Color32[size];
		for (int i = 0; i < size; i++)
		{
			byte b = (byte)(((_data[i] & filter) != 0) ? byte.MaxValue : 0);
			array[i] = new Color32(b, b, b, byte.MaxValue);
		}
		Texture2D texture2D = new Texture2D(Width, Height, TextureFormat.RGB24, mipChain: false);
		texture2D.SetPixels32(array);
		texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		return texture2D;
	}

	public virtual byte GreyByte(int x, int y)
	{
		return _data[PixelIndex(x, y)];
	}

	public virtual byte[] PixelByte(int x, int y)
	{
		byte[] array = new byte[_bpp];
		index = PixelIndex(x, y);
		for (itr = 0; itr < _bpp; itr++)
		{
			array[itr] = _data[index + itr];
		}
		return array;
	}

	public virtual float GreyFloat(int x, int y)
	{
		return Byte2Float * (float)(int)_data[PixelIndex(x, y)];
	}

	public virtual byte GetPixelByte(int x, int y)
	{
		if (x < 0)
		{
			x = Width - x;
		}
		else if (x >= Width)
		{
			x -= Width;
		}
		if (y < 0)
		{
			y = Height - y;
		}
		else if (y >= Height)
		{
			y -= Height;
		}
		return _data[PixelIndex(x, y)];
	}

	public virtual float GetPixelFloat(int x, int y)
	{
		retVal = 0f;
		index = PixelIndex(x, y);
		for (itr = 0; itr < _bpp; itr++)
		{
			retVal += (int)_data[index + itr];
		}
		retVal /= _bpp;
		retVal *= Byte2Float;
		return retVal;
	}

	public virtual Color GetPixelColor(int x, int y)
	{
		index = PixelIndex(x, y);
		if (_bpp == 3)
		{
			return new Color(Byte2Float * (float)(int)_data[index], Byte2Float * (float)(int)_data[index + 1], Byte2Float * (float)(int)_data[index + 2], 1f);
		}
		if (_bpp == 4)
		{
			return new Color(Byte2Float * (float)(int)_data[index], Byte2Float * (float)(int)_data[index + 1], Byte2Float * (float)(int)_data[index + 2], Byte2Float * (float)(int)_data[index + 3]);
		}
		if (_bpp == 2)
		{
			retVal = Byte2Float * (float)(int)_data[index];
			return new Color(retVal, retVal, retVal, Byte2Float * (float)(int)_data[index + 1]);
		}
		retVal = Byte2Float * (float)(int)_data[index];
		return new Color(retVal, retVal, retVal, 1f);
	}

	public virtual Color32 GetPixelColor32(int x, int y)
	{
		index = PixelIndex(x, y);
		if (_bpp == 3)
		{
			return new Color32(_data[index], _data[index + 1], _data[index + 2], byte.MaxValue);
		}
		if (_bpp == 4)
		{
			return new Color32(_data[index], _data[index + 1], _data[index + 2], _data[index + 3]);
		}
		if (_bpp == 2)
		{
			retVal = (int)_data[index];
			return new Color((int)val, (int)val, (int)val, (int)_data[index + 1]);
		}
		byte num = _data[index];
		return new Color32(num, num, num, num);
	}

	public virtual HeightAlpha GetPixelHeightAlpha(int x, int y)
	{
		index = PixelIndex(x, y);
		if (_bpp == 2)
		{
			return new HeightAlpha(Byte2Float * (float)(int)_data[index], Byte2Float * (float)(int)_data[index + 1]);
		}
		if (_bpp == 4)
		{
			val = _data[index];
			return new HeightAlpha(Byte2Float * (float)(int)_data[index], Byte2Float * (float)(int)_data[index + 3]);
		}
		return new HeightAlpha(Byte2Float * (float)(int)_data[index], 1f);
	}

	public virtual Color GetPixelColor(float x, float y)
	{
		ConstructBilinearCoords(x, y);
		return Color.Lerp(Color.Lerp(GetPixelColor(minX, minY), GetPixelColor(maxX, minY), midX), Color.Lerp(GetPixelColor(minX, maxY), GetPixelColor(maxX, maxY), midX), midY);
	}

	public virtual Color GetPixelColor32(float x, float y)
	{
		ConstructBilinearCoords(x, y);
		return Color32.Lerp(Color32.Lerp(GetPixelColor32(minX, minY), GetPixelColor32(maxX, minY), midX), Color32.Lerp(GetPixelColor32(minX, maxY), GetPixelColor32(maxX, maxY), midX), midY);
	}

	public virtual float GetPixelFloat(float x, float y)
	{
		ConstructBilinearCoords(x, y);
		if (_bpp == 1)
		{
			return Mathf.Lerp(Mathf.Lerp(GreyFloat(minX, minY), GreyFloat(maxX, minY), midX), Mathf.Lerp(GreyFloat(minX, maxY), GreyFloat(maxX, maxY), midX), midY);
		}
		return Mathf.Lerp(Mathf.Lerp(GetPixelFloat(minX, minY), GetPixelFloat(maxX, minY), midX), Mathf.Lerp(GetPixelFloat(minX, maxY), GetPixelFloat(maxX, maxY), midX), midY);
	}

	public virtual HeightAlpha GetPixelHeightAlpha(float x, float y)
	{
		ConstructBilinearCoords(x, y);
		return HeightAlpha.Lerp(HeightAlpha.Lerp(GetPixelHeightAlpha(minX, minY), GetPixelHeightAlpha(maxX, minY), midX), HeightAlpha.Lerp(GetPixelHeightAlpha(minX, maxY), GetPixelHeightAlpha(maxX, maxY), midX), midY);
	}

	public virtual Color GetPixelColor(double x, double y)
	{
		ConstructBilinearCoords(x, y);
		return Color.Lerp(Color.Lerp(GetPixelColor(minX, minY), GetPixelColor(maxX, minY), midX), Color.Lerp(GetPixelColor(minX, maxY), GetPixelColor(maxX, maxY), midX), midY);
	}

	public virtual Color GetPixelColor32(double x, double y)
	{
		ConstructBilinearCoords(x, y);
		return Color32.Lerp(Color32.Lerp(GetPixelColor32(minX, minY), GetPixelColor32(maxX, minY), midX), Color32.Lerp(GetPixelColor32(minX, maxY), GetPixelColor32(maxX, maxY), midX), midY);
	}

	public virtual float GetPixelFloat(double x, double y)
	{
		ConstructBilinearCoords(x, y);
		if (_bpp == 1)
		{
			return Mathf.Lerp(Mathf.Lerp(GreyFloat(minX, minY), GreyFloat(maxX, minY), midX), Mathf.Lerp(GreyFloat(minX, maxY), GreyFloat(maxX, maxY), midX), midY);
		}
		return Mathf.Lerp(Mathf.Lerp(GetPixelFloat(minX, minY), GetPixelFloat(maxX, minY), midX), Mathf.Lerp(GetPixelFloat(minX, maxY), GetPixelFloat(maxX, maxY), midX), midY);
	}

	public virtual HeightAlpha GetPixelHeightAlpha(double x, double y)
	{
		ConstructBilinearCoords(x, y);
		return HeightAlpha.Lerp(HeightAlpha.Lerp(GetPixelHeightAlpha(minX, minY), GetPixelHeightAlpha(maxX, minY), midX), HeightAlpha.Lerp(GetPixelHeightAlpha(minX, maxY), GetPixelHeightAlpha(maxX, maxY), midX), midY);
	}

	public virtual void SetPixel(int x, int y, byte b)
	{
		if (x < 0)
		{
			x = Width - x;
		}
		else if (x >= Width)
		{
			x -= Width;
		}
		if (y < 0)
		{
			y = Height - y;
		}
		else if (y >= Height)
		{
			y -= Height;
		}
		_data[PixelIndex(x, y)] = b;
	}

	public virtual void SetPixel(float x, float y, byte b)
	{
		ConstructBilinearCoords(x, y);
		if (_bpp == 1)
		{
			float num = Byte2Float * (float)(int)b;
			float value = num * midX * midY;
			float value2 = num * (1f - midX) * midY;
			float value3 = num * midX * (1f - midY);
			float value4 = num * (1f - midX) * (1f - midY);
			float num2 = (GreyFloat(minX, minY) + Mathf.Clamp01(value)) / 2f;
			float num3 = (GreyFloat(maxX, maxY) + Mathf.Clamp01(value2)) / 2f;
			float num4 = (GreyFloat(minX, minY) + Mathf.Clamp01(value3)) / 2f;
			float num5 = (GreyFloat(maxX, maxY) + Mathf.Clamp01(value4)) / 2f;
			byte b2 = (byte)Mathf.FloorToInt(Float2Byte * num2);
			byte b3 = (byte)Mathf.FloorToInt(Float2Byte * num3);
			byte b4 = (byte)Mathf.FloorToInt(Float2Byte * num4);
			byte b5 = (byte)Mathf.FloorToInt(Float2Byte * num5);
			SetPixel(minX, minY, b2);
			SetPixel(maxX, minY, b3);
			SetPixel(minX, maxY, b4);
			SetPixel(maxX, maxY, b5);
		}
	}

	public void ConstructBilinearCoords(float x, float y)
	{
		x = Mathf.Abs(x - Mathf.Floor(x));
		y = Mathf.Abs(y - Mathf.Floor(y));
		centerX = x * (float)_width;
		minX = Mathf.FloorToInt(centerX);
		maxX = Mathf.CeilToInt(centerX);
		midX = centerX - (float)minX;
		if (maxX == _width)
		{
			maxX = 0;
		}
		centerY = y * (float)_height;
		minY = Mathf.FloorToInt(centerY);
		maxY = Mathf.CeilToInt(centerY);
		midY = centerY - (float)minY;
		if (maxY == _height)
		{
			maxY = 0;
		}
	}

	public virtual void ConstructBilinearCoords(double x, double y)
	{
		x = Math.Abs(x - Math.Floor(x));
		y = Math.Abs(y - Math.Floor(y));
		centerXD = x * (double)_width;
		minX = (int)Math.Floor(centerXD);
		maxX = (int)Math.Ceiling(centerXD);
		midX = (float)(centerXD - (double)minX);
		if (maxX == _width)
		{
			maxX = 0;
		}
		centerYD = y * (double)_height;
		minY = (int)Math.Floor(centerYD);
		maxY = (int)Math.Ceiling(centerYD);
		midY = (float)(centerYD - (double)minY);
		if (maxY == _height)
		{
			maxY = 0;
		}
	}
}
