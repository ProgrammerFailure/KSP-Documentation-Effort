using System.IO;
using UnityEngine;

public class TGAImage
{
	public static Color32[] colorData;

	public static TGAHeader header;

	public bool ReadImage(string filePath)
	{
		if (!File.Exists(filePath))
		{
			Debug.LogError("TGA image does not exist at path '" + filePath + "'");
			return false;
		}
		return ReadImage(new FileInfo(filePath));
	}

	public bool ReadImage(FileInfo file)
	{
		byte[] bytes = MemoryCache.GetBytes(file);
		if (bytes == null)
		{
			Debug.LogError("TGA: data error");
			return false;
		}
		if (bytes.Length < 18)
		{
			Debug.LogError("TGA invalid length of only " + bytes.Length + "bytes");
			return false;
		}
		header = new TGAHeader(bytes);
		colorData = ReadImage(header, bytes);
		if (colorData == null)
		{
			return false;
		}
		return true;
	}

	public Color32[] ReadImage(TGAHeader header, byte[] data)
	{
		switch (header.imageType)
		{
		case TGAImageType.RTE_TrueColor:
			return ReadRTETrueColorImage(header, data);
		default:
			Debug.Log("Image type of " + header.imageType.ToString() + " is not supported.");
			return null;
		case TGAImageType.Uncompressed_TrueColor:
			return ReadTrueColorImage(header, data);
		}
	}

	public Color32[] ReadTrueColorImage(TGAHeader header, byte[] data)
	{
		bool num = header.pixelDepth / 8 == 4;
		Color32[] color = MemoryCache.GetColor32(header.width * header.height);
		int num2 = 18;
		int num3 = 0;
		Color32 color2 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		int height = header.height;
		int width = header.width;
		if (num)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					color2.b = data[num2];
					num2++;
					color2.g = data[num2];
					num2++;
					color2.r = data[num2];
					num2++;
					color2.a = data[num2];
					num2++;
					color[num3] = color2;
					num3++;
				}
			}
		}
		else
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					color2.b = data[num2];
					num2++;
					color2.g = data[num2];
					num2++;
					color2.r = data[num2];
					num2++;
					color[num3] = color2;
					num3++;
				}
			}
		}
		return color;
	}

	public Color32[] ReadRTETrueColorImage(TGAHeader header, byte[] data)
	{
		if (header.pixelDepth / 8 == 4)
		{
			return ReadRTETrueColorImageAlpha(header, data);
		}
		return ReadRTETrueColorImageNoAlpha(header, data);
	}

	public Color32[] ReadRTETrueColorImageAlpha(TGAHeader header, byte[] data)
	{
		Color32[] color = MemoryCache.GetColor32(header.width * header.height);
		int num = 18;
		int num2 = 0;
		Color32 color2 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		int nPixels = header.nPixels;
		while (num2 < nPixels)
		{
			int num3 = data[num];
			num++;
			if (((uint)num3 & 0x80u) != 0)
			{
				num3 = num3 - 128 + 1;
				color2.b = data[num];
				num++;
				color2.g = data[num];
				num++;
				color2.r = data[num];
				num++;
				color2.a = data[num];
				num++;
				for (int i = 0; i < num3; i++)
				{
					color[num2] = color2;
					num2++;
				}
			}
			else
			{
				num3++;
				for (int i = 0; i < num3; i++)
				{
					color2.b = data[num];
					num++;
					color2.g = data[num];
					num++;
					color2.r = data[num];
					num++;
					color2.a = data[num];
					num++;
					color[num2] = color2;
					num2++;
				}
			}
		}
		return color;
	}

	public Color32[] ReadRTETrueColorImageNoAlpha(TGAHeader header, byte[] data)
	{
		Color32[] color = MemoryCache.GetColor32(header.width * header.height);
		int num = 18;
		int num2 = 0;
		Color32 color2 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		int nPixels = header.nPixels;
		while (num2 < nPixels)
		{
			int num3 = data[num];
			num++;
			if (((uint)num3 & 0x80u) != 0)
			{
				num3 = num3 - 128 + 1;
				color2.b = data[num];
				num++;
				color2.g = data[num];
				num++;
				color2.r = data[num];
				num++;
				for (int i = 0; i < num3; i++)
				{
					color[num2] = color2;
					num2++;
				}
			}
			else
			{
				num3++;
				for (int i = 0; i < num3; i++)
				{
					color2.b = data[num];
					num++;
					color2.g = data[num];
					num++;
					color2.r = data[num];
					num++;
					color[num2] = color2;
					num2++;
				}
			}
		}
		return color;
	}

	public Texture2D CreateTexture()
	{
		return CreateTexture(mipmap: true, linear: true, compress: true, compressHighQuality: false, allowRead: false);
	}

	public Texture2D CreateTexture(bool mipmap, bool linear, bool compress, bool compressHighQuality, bool allowRead)
	{
		if (header == null)
		{
			Debug.Log("Cannot create texture: No header created");
			return null;
		}
		if (colorData == null)
		{
			Debug.Log("Cannot create texture: No color data present");
			return null;
		}
		Texture2D texture2D = null;
		if (header.bpp == 4)
		{
			texture2D = new Texture2D(header.width, header.height, TextureFormat.RGBA32, mipmap, linear);
		}
		else if (header.bpp == 3)
		{
			texture2D = new Texture2D(header.width, header.height, TextureFormat.RGB24, mipmap, linear);
		}
		if (texture2D == null)
		{
			Debug.Log("Cannot create texture: Header denotes incorrect format");
			return null;
		}
		texture2D.SetPixels32(colorData);
		texture2D.Apply(mipmap);
		if (compressHighQuality)
		{
			texture2D.Compress(compressHighQuality);
			texture2D.Apply(mipmap, !allowRead);
		}
		return texture2D;
	}

	~TGAImage()
	{
		header = null;
		colorData = null;
	}
}
