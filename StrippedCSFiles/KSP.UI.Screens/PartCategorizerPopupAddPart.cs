using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class PartCategorizerPopupAddPart : MonoBehaviour
{
	public delegate bool CriteriaAccept(string partName, PartCategorizer.Category category, out string reason);

	public class MiniCategory
	{
		public PartCategorizer.Category category;

		public PartCategorizerButton buttonCopy;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MiniCategory(PartCategorizer.Category category, bool main)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnIconSelect(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
		{
			throw null;
		}
	}

	public TextMeshProUGUI title;

	public Button btnClose;

	public Button btnAccept;

	public UIList scrollList;

	public Font font;

	private Callback<string, PartCategorizer.Category> onAccept;

	public CriteriaAccept onAcceptCriteria;

	private bool _started;

	private string partName;

	private PartCategorizer.Category selectedCategory;

	public static PartCategorizerPopupAddPart Instance
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

	public bool Showing
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
	public PartCategorizerPopupAddPart()
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
	public void Show(string partName, string partTitle, Callback<string, PartCategorizer.Category> onAccept, CriteriaAccept onAcceptCriteria, bool forceListRebuild)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseinputClose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseinputAccept()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateIconList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateIconList()
	{
		throw null;
	}
}
