using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MEBannerBrowser : MonoBehaviour
{
	public delegate void BannerSelectedCallback(MEBannerEntry selected, MEBannerType t);

	public delegate void BannerDeletedCallback(MEBannerEntry selected, MEBannerType t);

	public BannerSelectedCallback OnBannerSelected;

	public BannerDeletedCallback OnBannerDeleted;

	public Callback OnDismiss;

	private const string uiSkinName = "FlagBrowserSkin";

	private List<MEBannerEntry> banners;

	private List<DialogGUIToggleButton> items;

	private MEBannerEntry selected;

	private UISkinDef skin;

	private PopupDialog dialog;

	private Vector2 windowSize;

	private Vector2 bannerSize;

	private MEBannerType currentBannerType;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEBannerBrowser()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIToggleGroup LoadBannerButtons(Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadGameDataBanners(ref List<MEBannerEntry> bannersList, MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadMissionBanners(ref List<MEBannerEntry> bannersList, MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MEBannerEntry GetDuplicate(string bannerName, ref List<MEBannerEntry> bannerList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PopupDialog CreateBrowser(MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfirmDeleteBanner()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Accept(MEBannerEntry selectedBanner, MEBannerType bannerType)
	{
		throw null;
	}
}
