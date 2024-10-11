using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace KSPAssets.KSPedia;

public class KSPediaSpawner : MonoBehaviour
{
	public KSPedia prefab;

	private float canvasBorder;

	private KSPedia kspedia;

	private ApplicationLauncherButton applauncherButton;

	private Button toolbarButton;

	protected static KSPediaSpawner Instance
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
	public KSPediaSpawner()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Show(string screen = null, Button toolbarButton = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Show(ApplicationLauncherButton applauncherButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Hide()
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
	private void ShowKSPedia(string screen = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HideKSPedia()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void KSPediaReadyCallback()
	{
		throw null;
	}
}
