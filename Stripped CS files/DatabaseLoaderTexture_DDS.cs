using System.Collections;
using System.IO;
using DDSHeaders;
using UnityEngine;

[DatabaseLoaderAttrib(new string[] { "dds" })]
public class DatabaseLoaderTexture_DDS : DatabaseLoader<GameDatabase.TextureInfo>
{
	public bool isNormalMap;

	public override IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		Texture2D texture2D = Read(file.FullName);
		if (texture2D != null)
		{
			GameDatabase.TextureInfo textureInfo = new GameDatabase.TextureInfo(urlFile, texture2D, isNormalMap, isReadable: false, isCompressed: true);
			base.obj = textureInfo;
			base.successful = true;
		}
		else
		{
			Debug.LogWarning("Texture load error in '" + file.FullName + "'");
			base.obj = null;
			base.successful = false;
		}
		yield break;
	}

	public Texture2D Read(string filename)
	{
		BinaryReader binaryReader = new BinaryReader(new MemoryStream(File.ReadAllBytes(filename)));
		if (binaryReader.ReadUInt32() != DDSValues.uintMagic)
		{
			Debug.LogError("DDS: File is not a DDS format file!");
			return null;
		}
		DDSHeader dDSHeader = new DDSHeader(binaryReader);
		if (dDSHeader.ddspf.dwFourCC == DDSValues.uintDX10)
		{
			new DDSHeaderDX10(binaryReader);
		}
		bool mipChain = (dDSHeader.dwCaps & DDSPixelFormatCaps.MIPMAP) != 0;
		isNormalMap = (dDSHeader.ddspf.dwFlags & 0x80000u) != 0 || (dDSHeader.ddspf.dwFlags & 0x80000000u) != 0;
		Texture2D texture2D = null;
		if (dDSHeader.ddspf.dwFourCC == DDSValues.uintDXT1)
		{
			texture2D = new Texture2D((int)dDSHeader.dwWidth, (int)dDSHeader.dwHeight, TextureFormat.DXT1, mipChain);
			texture2D.LoadRawTextureData(binaryReader.ReadBytes((int)(binaryReader.BaseStream.Length - binaryReader.BaseStream.Position)));
			texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		}
		else if (dDSHeader.ddspf.dwFourCC == DDSValues.uintDXT3)
		{
			Debug.LogError("DDS: DXT3(" + dDSHeader.dwWidth + "x" + dDSHeader.dwHeight + ", MipMap=" + mipChain.ToString() + ") - DXT3 format is NOT supported. Use DXT5");
		}
		else if (dDSHeader.ddspf.dwFourCC == DDSValues.uintDXT5)
		{
			texture2D = new Texture2D((int)dDSHeader.dwWidth, (int)dDSHeader.dwHeight, TextureFormat.DXT5, mipChain);
			texture2D.LoadRawTextureData(binaryReader.ReadBytes((int)(binaryReader.BaseStream.Length - binaryReader.BaseStream.Position)));
			texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		}
		else if (dDSHeader.ddspf.dwFourCC == DDSValues.uintDXT2)
		{
			Debug.Log("DDS: DXT2 is not supported!");
		}
		else if (dDSHeader.ddspf.dwFourCC == DDSValues.uintDXT4)
		{
			Debug.Log("DDS: DXT4 is not supported!");
		}
		else if (dDSHeader.ddspf.dwFourCC == DDSValues.uintDX10)
		{
			Debug.Log("DDS: DX10 formats not supported");
		}
		return texture2D;
	}
}
