using System.IO;
using PartToolsLib;
using UnityEngine;

public static class MBMReader
{
	public static Texture2D Read(string path, bool compress, bool setReadOnly, out bool isNormalMap)
	{
		try
		{
			byte[] bytes = MemoryCache.GetBytes(path);
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(bytes));
			Texture2D result = ReadTexture2D(bytes, binaryReader, compress, setReadOnly, out isNormalMap);
			binaryReader.Close();
			return result;
		}
		catch
		{
			Debug.Log("Cannot open MBM file '" + path + "'");
			isNormalMap = false;
			return null;
		}
	}

	public static Texture2D ReadTexture2D(byte[] data, BinaryReader br, bool compress, bool setReadOnly, out bool isNormalMap)
	{
		br.ReadString();
		int num = br.ReadInt32();
		int num2 = br.ReadInt32();
		TextureType textureType = (TextureType)br.ReadInt32();
		int num3 = br.ReadInt32();
		Texture2D texture2D = null;
		Color32 color = default(Color32);
		int num4 = num * num2;
		Color32[] color2 = MemoryCache.GetColor32(num4);
		switch (textureType)
		{
		case TextureType.Texture:
		{
			bool flag = false;
			switch (num3)
			{
			case 32:
				flag = true;
				texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, mipChain: true);
				break;
			case 24:
				flag = false;
				texture2D = new Texture2D(num, num2, TextureFormat.RGB24, mipChain: true);
				break;
			}
			int num5 = (int)br.BaseStream.Position;
			if (flag)
			{
				for (int j = 0; j < num4; j++)
				{
					color.r = data[num5];
					num5++;
					color.g = data[num5];
					num5++;
					color.b = data[num5];
					num5++;
					color.a = data[num5];
					num5++;
					color2[j] = color;
				}
			}
			else
			{
				for (int k = 0; k < num4; k++)
				{
					color.r = data[num5];
					num5++;
					color.g = data[num5];
					num5++;
					color.b = data[num5];
					num5++;
					color2[k] = color;
				}
			}
			break;
		}
		case TextureType.NormalMap:
		{
			texture2D = new Texture2D(num, num2, TextureFormat.ARGB32, mipChain: true);
			for (int i = 0; i < num4; i++)
			{
				color.r = br.ReadByte();
				color.g = br.ReadByte();
				color.b = br.ReadByte();
				color.a = br.ReadByte();
				color2[i] = color;
			}
			break;
		}
		}
		texture2D.SetPixels32(color2);
		color2 = null;
		if (textureType != TextureType.NormalMap)
		{
			isNormalMap = false;
			texture2D.Apply();
			if (compress)
			{
				texture2D.Compress(highQuality: false);
			}
			texture2D.Apply(updateMipmaps: false, setReadOnly);
		}
		else
		{
			isNormalMap = true;
			texture2D.Apply(updateMipmaps: true, setReadOnly);
		}
		return texture2D;
	}
}
