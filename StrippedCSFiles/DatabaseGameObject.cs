using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.FX.Fireworks;
using UnityEngine;

public class DatabaseGameObject : MonoBehaviour
{
	private static List<string> databases;

	public string databaseName;

	[SerializeField]
	public static FireworkFXList fwFXList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DatabaseGameObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DatabaseGameObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadFireworkFXDefinitions()
	{
		throw null;
	}
}
