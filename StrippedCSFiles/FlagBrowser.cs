using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlagBrowser : MonoBehaviour
{
	[Serializable]
	public class FlagEntry
	{
		public GameDatabase.TextureInfo textureInfo;

		public string name;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public FlagEntry()
		{
			throw null;
		}
	}

	public delegate void FlagSelectedCallback(FlagEntry selected);

	public List<FlagEntry> Flags;

	public List<FlagEntry> OrganizationFlags;

	public List<FlagEntry> AgencyFlags;

	public FlagEntry selected;

	public string uiSkinName;

	private UISkinDef skin;

	private PopupDialog dialog;

	private MenuNavigation menuNav;

	public Rect windowRect;

	private Vector2 scrollPos;

	public float width;

	public float height;

	public float iconSize;

	public float iconSpacing;

	public FlagSelectedCallback OnFlagSelected;

	public Callback OnDismiss;

	private DialogGUIToggleGroup agencyToggleGroup;

	private DialogGUIToggleGroup organizationToggleGroup;

	private DialogGUIToggleGroup generalToggleGroup;

	private List<DialogGUIToggleButton> generalItems;

	private List<DialogGUIToggleButton> agencyItems;

	private List<DialogGUIToggleButton> organizationItems;

	private bool toggleInProgress;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlagBrowser()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<FlagEntry> ProcessFlagFolder(List<GameDatabase.TextureInfo> textures)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PopupDialog CreateFlagBrowser()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<DialogGUIToggleButton> ProcessButtons(List<FlagEntry> flags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Toggled(DialogGUIToggleButton t, bool b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleAllOff(DialogGUIToggleGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Accept(FlagEntry sel)
	{
		throw null;
	}
}
