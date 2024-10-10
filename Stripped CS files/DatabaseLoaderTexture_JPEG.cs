using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[DatabaseLoaderAttrib(new string[] { "jpg", "jpeg" })]
public class DatabaseLoaderTexture_JPEG : DatabaseLoader<GameDatabase.TextureInfo>
{
	public override IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		UnityWebRequest imageWWW = UnityWebRequestTexture.GetTexture(KSPUtil.ApplicationFileProtocol + file.FullName);
		yield return imageWWW.SendWebRequest();
		while (!imageWWW.isDone)
		{
			yield return null;
		}
		if (imageWWW.error != null)
		{
			Debug.LogWarning("Texture load error in '" + file.FullName + "': " + imageWWW.error);
			base.obj = null;
			base.successful = false;
			yield break;
		}
		if (Path.GetFileNameWithoutExtension(file.Name).EndsWith("NRM"))
		{
			Texture2D content = DownloadHandlerTexture.GetContent(imageWWW);
			GameDatabase.TextureInfo textureInfo = new GameDatabase.TextureInfo(urlFile, GameDatabase.BitmapToUnityNormalMap(content), isNormalMap: true, isReadable: false, isCompressed: true);
			base.obj = textureInfo;
			base.successful = true;
			yield break;
		}
		Texture2D content2 = DownloadHandlerTexture.GetContent(imageWWW);
		if (content2.width % 4 == 0 && content2.height % 4 == 0)
		{
			content2.Compress(highQuality: false);
		}
		else
		{
			Debug.LogWarning("Texture resolution is not valid for compression: '" + file.FullName + "' - consider changing the image's width and height to enable compression");
		}
		content2.Apply();
		GameDatabase.TextureInfo textureInfo2 = new GameDatabase.TextureInfo(urlFile, content2, isNormalMap: false, isReadable: true, isCompressed: true);
		base.obj = textureInfo2;
		base.successful = true;
	}
}
