using System.Collections;
using System.IO;
using UnityEngine;

[DatabaseLoaderAttrib(new string[] { "mu" })]
public class DatabaseLoaderModel_MU : DatabaseLoader<GameObject>
{
	public override IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		GameObject gameObject = PartReader.Read(urlFile);
		if (gameObject != null)
		{
			base.obj = gameObject;
			base.successful = true;
		}
		else
		{
			Debug.LogWarning("Model load error in '" + file.FullName + "'");
			base.obj = null;
			base.successful = false;
		}
		yield break;
	}
}
