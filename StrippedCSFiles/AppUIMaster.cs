using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using UnityEngine;

public class AppUIMaster : MonoBehaviour
{
	public List<GameObject> controlPrefabs;

	public Dictionary<Type, GameObject> controlPrefabByType;

	public static AppUIMaster Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AppUIMaster()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupRowPrefabByType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TryGetAppUIControlPrefab(AppUI_Control controlAttrib, out GameObject prefab)
	{
		throw null;
	}
}
