using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[DatabaseLoaderAttrib(new string[] { "png" })]
public class DatabaseLoaderTexture_PNG : DatabaseLoader<GameDatabase.TextureInfo>
{
	public override IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		string path = KSPUtil.ApplicationFileProtocol + file.FullName;
		UnityWebRequest imageWWW = UnityWebRequestTexture.GetTexture(path);
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
		Texture2D texture2D = DownloadHandlerTexture.GetContent(imageWWW);
		string[] array = new string[3] { "Icons", "Tutorials", "SimpleIcons" };
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (path.Contains(Path.DirectorySeparatorChar + array[i] + Path.DirectorySeparatorChar))
			{
				flag = true;
			}
		}
		if (path.Contains(Path.DirectorySeparatorChar + "Flags" + Path.DirectorySeparatorChar))
		{
			Texture2D texture2D2 = new Texture2D(texture2D.width, texture2D.height, texture2D.format, mipChain: true);
			texture2D2.LoadImage(imageWWW.downloadHandler.data);
			texture2D = texture2D2;
			flag2 = true;
		}
		if (flag)
		{
			Texture2D texture2D3 = new Texture2D(texture2D.width, texture2D.height, TextureFormat.ARGB32, mipChain: false);
			texture2D3.SetPixels32(texture2D.GetPixels32());
			texture2D = texture2D3;
		}
		if (texture2D.width % 4 == 0 && texture2D.height % 4 == 0)
		{
			texture2D.Compress(flag2 ? true : false);
		}
		else
		{
			Debug.LogWarning("Texture resolution is not valid for compression: '" + file.FullName + "' - consider changing the image's width and height to enable compression");
		}
		texture2D.Apply(updateMipmaps: true);
		GameDatabase.TextureInfo textureInfo2 = new GameDatabase.TextureInfo(urlFile, texture2D, isNormalMap: false, isReadable: true, isCompressed: true);
		base.obj = textureInfo2;
		base.successful = true;
	}
}
