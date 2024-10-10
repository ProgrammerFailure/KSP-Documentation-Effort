using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[DatabaseLoaderAttrib(new string[] { "wav", "ogg" })]
public class DatabaseLoaderAudio : DatabaseLoader<AudioClip>
{
	public override IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(KSPUtil.ApplicationFileProtocol + file.FullName, AudioType.UNKNOWN);
		yield return www.SendWebRequest();
		while (!www.isDone)
		{
			yield return null;
		}
		if (!www.isNetworkError && !www.isHttpError)
		{
			base.obj = DownloadHandlerAudioClip.GetContent(www);
			base.successful = base.obj != null;
		}
		else
		{
			Debug.LogWarning("Audio file: " + urlFile.name + " load error: " + www.error);
			base.obj = null;
			base.successful = false;
		}
	}
}
