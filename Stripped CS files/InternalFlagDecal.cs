using System.Collections;
using UnityEngine;

public class InternalFlagDecal : InternalModule
{
	[KSPField]
	public string textureQuadName = "";

	public Renderer textureQuadRenderer;

	public Texture2D flagTexture;

	public IEnumerator WaitForPart()
	{
		while (!base.part)
		{
			yield return null;
		}
		UpdateFlagTexture();
	}

	public override void OnAwake()
	{
		StartCoroutine(WaitForPart());
	}

	public void OnDestroy()
	{
	}

	public void UpdateFlagTexture()
	{
		if (base.part.flagURL != "")
		{
			flagTexture = GameDatabase.Instance.GetTexture(base.part.flagURL, asNormalMap: false);
			if (flagTexture == null)
			{
				Debug.LogWarning("[FlagDecal Warning!]: Flag URL is given as " + base.part.flagURL + ", but no texture found in database with that name", base.gameObject);
			}
		}
		if (!(textureQuadName != ""))
		{
			return;
		}
		Transform transform = internalProp.FindModelTransform(textureQuadName);
		if (transform != null)
		{
			textureQuadRenderer = transform.GetComponent<Renderer>();
			if (textureQuadRenderer != null)
			{
				if (flagTexture != null)
				{
					textureQuadRenderer.material.mainTexture = flagTexture;
				}
			}
			else
			{
				Debug.LogWarning("[FlagDecal Warning!]: Flag quad object is given as " + textureQuadName + ", but has no renderer attached", transform.gameObject);
			}
		}
		else
		{
			Debug.LogWarning("[FlagDecal Warning!]: Flag quad object is given as " + textureQuadName + ", but no object found in model with that name", base.gameObject);
		}
	}
}
