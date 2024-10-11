using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.SpaceCenter;

public class BuildingPicker : MonoBehaviour
{
	[Serializable]
	public class FacilityUIInfo
	{
		public string name;

		public ButtonSpritesMgr.ButtonSprites spriteSet;

		[NonSerialized]
		public SpaceCenterBuilding scBuilding;

		internal BuildingPickerItem buildingPickerItem;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public FacilityUIInfo()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BuildingPicker _003C_003E4__this;

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
		public _003CStart_003Ed__5(int _003C_003E1__state)
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

	public BuildingPickerItem itemPrefab;

	public RectTransform uiParent;

	public LayoutGroup layoutGroup;

	public FacilityUIInfo[] faciltyInfos;

	private bool isReady;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BuildingPicker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__5))]
	private IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HideUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ConstructBuildingList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConstructUI()
	{
		throw null;
	}
}
