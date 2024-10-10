using System.Collections;
using System.IO;
using UnityEngine;

[DatabaseLoaderAttrib(new string[] { "truecolor" })]
public class DatabaseLoaderTexture_TRUECOLOR : DatabaseLoader<GameDatabase.TextureInfo>
{
	public override IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		byte[] data = File.ReadAllBytes(file.FullName);
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipChain: false);
		if (!texture2D.LoadImage(data))
		{
			Debug.LogWarning("Texture load error in '" + file.FullName + "'");
			base.obj = null;
			base.successful = false;
		}
		else if (Path.GetFileNameWithoutExtension(file.Name).EndsWith("NRM"))
		{
			GameDatabase.TextureInfo textureInfo = new GameDatabase.TextureInfo(urlFile, GameDatabase.BitmapToUnityNormalMap(texture2D), isNormalMap: true, isReadable: false, isCompressed: false);
			base.obj = textureInfo;
			base.successful = true;
		}
		else
		{
			texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			GameDatabase.TextureInfo textureInfo2 = new GameDatabase.TextureInfo(urlFile, texture2D, isNormalMap: false, isReadable: false, isCompressed: false);
			base.obj = textureInfo2;
			base.successful = true;
		}
		yield break;
	}
}
