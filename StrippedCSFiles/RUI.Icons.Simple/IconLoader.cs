using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RUI.Icons.Simple;

public class IconLoader : MonoBehaviour
{
	[SerializeField]
	private string loaderName;

	[SerializeField]
	private string simpleIconPath;

	[SerializeField]
	private Icon[] publicIcons;

	[SerializeField]
	private Icon[] internalIcons;

	[SerializeField]
	private Icon fallbackIcon;

	[NonSerialized]
	public List<Icon> icons;

	public Dictionary<string, Icon> iconDictionary;

	public Icon FallbackIcon
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IconLoader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupIcons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Icon GetIcon(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void LoadIcon(GameDatabase.TextureInfo texInfo, bool useStockPath)
	{
		throw null;
	}
}
