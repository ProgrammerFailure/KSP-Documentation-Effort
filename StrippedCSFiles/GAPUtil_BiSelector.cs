using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GAPUtil_BiSelector : MonoBehaviour
{
	public Button leftButton;

	public Button rightButton;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI footerText;

	public GameObject containerSelector;

	public GameObject containerLeftButtons;

	public GameObject containerFooter;

	public Transform sideBarTransform;

	public Sprite sideBarDividerSprite;

	public Sprite sideBarDividerEndSprite;

	protected DictionaryValueList<string, TrackingStationObjectButton> sideBarControls;

	protected DictionaryValueList<string, MEPartCategoryButton> sideBarGAPControls;

	protected Image lastDivider;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPUtil_BiSelector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTitleText(string newText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetFooterText(string newText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TrackingStationObjectButton AddSidebarButton(string id, string icon, string toolTip, bool startState = false, int count = 0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEPartCategoryButton AddSidebarGAPButton(string id, string icon, string toolTip, bool startState = false, int count = 0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TrackingStationObjectButton GetSidebarButton(string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEPartCategoryButton GetSidebarGapButton(string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearSidebar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddDivider()
	{
		throw null;
	}
}
