using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI.Screens.Editor;

public class InventoryPartListTooltip : Tooltip, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler
{
	[CompilerGenerated]
	private sealed class _003CWaitAndRefreshHeight_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public InventoryPartListTooltip _003C_003E4__this;

		public Vector2 localCursor;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CWaitAndRefreshHeight_003Ed__25(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public TextMeshProUGUI textName;

	public PartListTooltipWidget extInfoModuleWidgetPrefab;

	public PartListTooltipWidget extInfoRscWidgePrefab;

	private List<PartListTooltipWidget> extInfoModules;

	private List<PartListTooltipWidget> extInfoRscs;

	public RectTransform extInfoListContainer;

	public RectTransform extInfoListSpacer;

	public InventoryPartListTooltipController toolTipController;

	public bool mouseOver;

	public StoredPart inventoryStoredPart;

	private AvailablePart partInfo;

	private PartUpgradeHandler.Upgrade upgrade;

	private Part partRef;

	private PartModule.PartUpgradeState upgradeState;

	private RectTransform prefabTransform;

	private WaitForEndOfFrame wfeof;

	public bool isGrey
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InventoryPartListTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(AvailablePart availablePart, Callback<PartListTooltip> onPurchase, RenderTexture texture = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(AvailablePart availablePart, PartUpgradeHandler.Upgrade up, Callback<PartListTooltip> onPurchase, RenderTexture texture = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshToolTipHeight(Vector2 localCursor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CWaitAndRefreshHeight_003Ed__25))]
	internal IEnumerator WaitAndRefreshHeight(Vector2 localCursor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateInfoWidgets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HidePreviousTooltipWidgets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PartListTooltipWidget GetNewTooltipWidget(PartListTooltipWidget prefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}
}
