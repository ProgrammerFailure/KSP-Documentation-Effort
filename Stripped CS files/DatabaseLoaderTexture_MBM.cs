using System.Collections;
using System.IO;
using UnityEngine;

[DatabaseLoaderAttrib(new string[] { "mbm" })]
public class DatabaseLoaderTexture_MBM : DatabaseLoader<GameDatabase.TextureInfo>
{
	public override IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		bool isNormalMap = false;
		Texture2D texture2D = MBMReader.Read(file.FullName, compress: true, setReadOnly: true, out isNormalMap);
		if (texture2D != null)
		{
			GameDatabase.TextureInfo textureInfo = new GameDatabase.TextureInfo(urlFile, texture2D, isNormalMap, isReadable: true, isCompressed: true);
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
