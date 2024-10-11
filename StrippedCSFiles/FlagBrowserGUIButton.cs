using System.Runtime.CompilerServices;
using UnityEngine;

public class FlagBrowserGUIButton : DialogGUIButton
{
	public GameObject FlagBrowserPrefab;

	public GUISkin skin;

	public Rect buttonRect;

	public Texture contentTexture;

	private FlagBrowser browser;

	private bool locked;

	private Callback flagBrowserOpened;

	private FlagBrowser.FlagSelectedCallback flagSelected;

	private Callback flagCancelled;

	private string _flagURL;

	public string flagURL
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlagBrowserGUIButton(Texture texture, Callback onBrowserOpen, FlagBrowser.FlagSelectedCallback onFlagSelected, Callback onFlagCancelled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Draw(Rect rect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Draw(float width, float height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnBrowser()
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
