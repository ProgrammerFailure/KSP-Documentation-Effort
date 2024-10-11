using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FlagBrowserButton : MonoBehaviour
{
	public FlagBrowser flagBrowserPrefab;

	public RawImage flagRawImage;

	public Button button;

	internal FlagBrowser browser;

	private FlagBrowser.FlagSelectedCallback OnFlagSelected;

	private Callback OnFlagCancelled;

	internal Callback<string> OnFlagSelectedURL;

	internal bool isFlagDecalBrowsing;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlagBrowserButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(Texture texture, Callback onBrowserOpen, FlagBrowser.FlagSelectedCallback onFlagSelect, Callback onFlagCancel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetFlag(Texture texture)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SpawnBrowser()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlagSelect(FlagBrowser.FlagEntry selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlagCancel()
	{
		throw null;
	}
}
