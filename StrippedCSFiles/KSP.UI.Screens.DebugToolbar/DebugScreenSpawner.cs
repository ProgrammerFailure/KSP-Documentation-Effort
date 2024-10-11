using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KSP.UI.Screens.DebugToolbar;

public class DebugScreenSpawner : MonoBehaviour
{
	public static DebugScreenSpawner Instance;

	public DebugScreen screenPrefab;

	private DebugScreen screen;

	private AddDebugScreens debugScreens;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DebugScreenSpawner()
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ShowDebugScreen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ShowTab(string name)
	{
		throw null;
	}
}
