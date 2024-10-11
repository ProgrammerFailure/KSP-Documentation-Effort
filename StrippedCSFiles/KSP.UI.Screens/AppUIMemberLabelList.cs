using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

[AppUI_LabelList]
public class AppUIMemberLabelList : AppUIMember
{
	public string newGuiName;

	public TextMeshProUGUI value1;

	public TextMeshProUGUI value2;

	public float preferredHeight;

	[SerializeField]
	private GameObject separator1;

	[SerializeField]
	private GameObject separator2;

	private LayoutElement layout;

	public AppUI_Control.HorizontalAlignment horizontalAlignment;

	public AppUI_Control.VerticalAlignment verticalAlignment;

	public bool showSeparator;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AppUIMemberLabelList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRefreshUI()
	{
		throw null;
	}
}
