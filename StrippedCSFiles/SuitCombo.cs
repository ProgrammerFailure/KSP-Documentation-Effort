using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class SuitCombo
{
	public enum MaterialProperty
	{
		BumpMap,
		MainTex,
		All
	}

	public enum TextureTarget
	{
		Helmet,
		Body,
		Neckring,
		Normal
	}

	public string name;

	public string displayName;

	public string primaryColor;

	public string secondaryColor;

	public string suitType;

	public string gender;

	public string suitTexture;

	public string normalTexture;

	public string sprite;

	public Texture defaultSuitTexture;

	public Texture defaultNormalTexture;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SuitCombo()
	{
		throw null;
	}
}
