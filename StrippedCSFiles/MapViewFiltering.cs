using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

public class MapViewFiltering : MonoBehaviour
{
	[Serializable]
	public class FilterButton
	{
		public TrackingStationObjectButton button;

		public VesselTypeFilter filterValue;

		public int typeCount;

		public bool showCount;

		private MapViewFiltering Host;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public FilterButton()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Init(MapViewFiltering host)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetSolo()
		{
			throw null;
		}
	}

	[Flags]
	public enum VesselTypeFilter
	{
		None = 0,
		Debris = 1,
		Unknown = 2,
		SpaceObjects = 4,
		Probes = 8,
		Rovers = 0x10,
		Landers = 0x20,
		Ships = 0x40,
		Stations = 0x80,
		Bases = 0x100,
		EVAs = 0x200,
		Flags = 0x400,
		Plane = 0x800,
		Relay = 0x1000,
		Site = 0x2000,
		DeployedScienceController = 0x4000,
		All = -1
	}

	[CompilerGenerated]
	private sealed class _003CRefreshInOneFrame_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MapViewFiltering _003C_003E4__this;

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
		public _003CRefreshInOneFrame_003Ed__20(int _003C_003E1__state)
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

	public static MapViewFiltering Instance;

	public UIPanelTransitionToggle panel;

	public FilterButton[] FilterButtons;

	public Button showHideButton;

	[SerializeField]
	private RectTransform backgroundImageRect;

	[SerializeField]
	private RectTransform commNetRect;

	public static VesselTypeFilter vesselTypeFilter;

	private bool started;

	public bool showObjectCounts;

	private Dictionary<VesselTypeFilter, FilterButton> vCounts;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapViewFiltering()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MapViewFiltering()
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
	public static int GetFilterState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadFilterState(int filterState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CountVessels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RefreshCounts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CRefreshInOneFrame_003Ed__20))]
	private IEnumerator RefreshInOneFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselCountChanged(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselCountChanged(ProtoVessel protoVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselCountChanged(ProtoVessel protoVessel, bool quick)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVesselCounts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onKnowledgeChanged(GameEvents.HostedFromToAction<IDiscoverable, DiscoveryLevels> dsc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselRename(GameEvents.HostedFromToAction<Vessel, string> nChg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private VesselTypeFilter GetFilterFromButtonStates()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void updateFilterFromButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void updateButtonsToFilter(VesselTypeFilter filter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void setFilter(VesselTypeFilter filter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool checkAgainstFilter(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleFiltersPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetFilter(VesselTypeFilter filter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CheckAgainstFilter(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Initialize()
	{
		throw null;
	}
}
