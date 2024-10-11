using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

internal class EditorSaveFolderItem : MonoBehaviour
{
	public Toggle toggleSetDefault;

	public string saveFolderPath;

	public TextMeshProUGUI textName;

	public Button buttonSave;

	private UICraftSaveFlyoutController controller;

	public bool IsOpenFileBrowserRequest;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorSaveFolderItem()
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
	public void MouseInput_OnClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MouseInput_SelectDefault(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Create(UICraftSaveFlyoutController controller, string saveFolderPath, string displayName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsPath(string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPathDisplay(string displayName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPath(string fullPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveCraft()
	{
		throw null;
	}
}
