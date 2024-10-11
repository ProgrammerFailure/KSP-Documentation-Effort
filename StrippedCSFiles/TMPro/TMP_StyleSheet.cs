using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

[Serializable]
public class TMP_StyleSheet : ScriptableObject
{
	private static TMP_StyleSheet s_Instance;

	[SerializeField]
	private List<TMP_Style> m_StyleList;

	private Dictionary<int, TMP_Style> m_StyleDictionary;

	public static TMP_StyleSheet instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_StyleSheet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static TMP_StyleSheet LoadDefaultStyleSheet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static TMP_Style GetStyle(int hashCode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private TMP_Style GetStyleInternal(int hashCode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateStyleDictionaryKey(int old_key, int new_key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RefreshStyles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadStyleDictionaryInternal()
	{
		throw null;
	}
}
