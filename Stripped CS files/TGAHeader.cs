using System;
using UnityEngine;

public class TGAHeader
{
	public int idLength;

	public bool hasColorMap;

	public TGAImageType imageType;

	public bool rteEncoding;

	public byte[] colorMap;

	public short xOrigin;

	public short yOrigin;

	public ushort width;

	public ushort height;

	public byte pixelDepth;

	public byte imageDesc;

	public int nPixels;

	public int bpp;

	public TGAHeader()
	{
	}

	public TGAHeader(byte[] data)
	{
		if (data.Length < 18)
		{
			Debug.LogError("TGA invalid length of only " + data.Length + "bytes");
			return;
		}
		idLength = data[0];
		hasColorMap = data[1] != 0;
		imageType = (TGAImageType)data[2];
		rteEncoding = (imageType & TGAImageType.const_4) != 0;
		colorMap = new byte[5];
		for (int i = 0; i < 5; i++)
		{
			colorMap[i] = data[i + 3];
		}
		byte[] array = new byte[10];
		for (int j = 0; j < 10; j++)
		{
			array[j] = data[j + 8];
		}
		xOrigin = BitConverter.ToInt16(array, 0);
		yOrigin = BitConverter.ToInt16(array, 2);
		width = BitConverter.ToUInt16(array, 4);
		height = BitConverter.ToUInt16(array, 6);
		pixelDepth = array[8];
		imageDesc = array[9];
		nPixels = width * height;
		bpp = pixelDepth / 8;
	}

	public byte[] GetData()
	{
		byte[] array = new byte[18]
		{
			0,
			0,
			(byte)imageType,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
		BitConverter.GetBytes(width).CopyTo(array, 12);
		BitConverter.GetBytes(height).CopyTo(array, 14);
		array[16] = pixelDepth;
		return array;
	}
}
