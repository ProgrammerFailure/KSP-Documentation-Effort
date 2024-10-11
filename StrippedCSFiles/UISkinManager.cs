using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Util;
using TMPro;
using UnityEngine;

public class UISkinManager : MonoBehaviour
{
	public List<UISkinDefSO> skins;

	public List<GameObject> prefabs;

	public TMP_FontAsset font;

	private static UISkinManager fetch;

	private static Dictionary<string, UISkinDef> loadedDefs;

	private static Dictionary<string, GameObject> loadedPrefabs;

	public static UISkinDef defaultSkin;

	public static TMP_FontAsset TMPFont
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISkinManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static UISkinManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UISkinDef GetSkin(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GameObject GetPrefab(string prefabName)
	{
		throw null;
	}
}
