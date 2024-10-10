using System.Collections;
using System.IO;
using UnityEngine;

[DatabaseLoaderAttrib(new string[] { "tga" })]
public class DatabaseLoaderTexture_TGA : DatabaseLoader<GameDatabase.TextureInfo>
{
	public override IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		TGAImage tGAImage = new TGAImage();
		tGAImage.ReadImage(file);
		Texture2D texture2D = tGAImage.CreateTexture(mipmap: true, linear: false, compress: true, compressHighQuality: false, allowRead: true);
		if (texture2D != null)
		{
			GameDatabase.TextureInfo textureInfo;
			if (Path.GetFileNameWithoutExtension(file.Name).EndsWith("NRM"))
			{
				texture2D = GameDatabase.BitmapToUnityNormalMap(texture2D);
				textureInfo = new GameDatabase.TextureInfo(urlFile, texture2D, isNormalMap: true, isReadable: false, isCompressed: true);
			}
			else
			{
				textureInfo = new GameDatabase.TextureInfo(urlFile, texture2D, isNormalMap: false, isReadable: true, isCompressed: true);
			}
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

	public override void CleanUp()
	{
		MemoryCache.CleanUp();
	}
}
