using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Actions;

public class ActionPartResourceDrain : ActionModule
{
	[CompilerGenerated]
	private sealed class _003CFire_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ActionPartResourceDrain _003C_003E4__this;

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
		public _003CFire_003Ed__21(int _003C_003E1__state)
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

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 40, resetValue = "0", guiName = "#autoLOC_8000008", Tooltip = "#autoLOC_8000009")]
	public float timePeriod;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 30, resetValue = "0", guiName = "#autoLOC_8000010", Tooltip = "#autoLOC_8000011")]
	public float amountToDrain;

	[MEGUI_VesselPartSelect(order = 10, resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000012", Tooltip = "#autoLOC_8000013")]
	public VesselPartIDPair vesselPartIDs;

	[MEGUI_Dropdown(order = 20, SetDropDownItems = "APRD_SetDropDownValues", guiName = "#autoLOC_8000014", Tooltip = "#autoLOC_8000015")]
	public string resourceName;

	private Part part;

	private ProtoPartSnapshot protopart;

	private PartResourceDefinition resourcedef;

	private bool vesselDrain;

	private Vessel vessel;

	private bool vesselFound;

	private bool partLoaded;

	private bool partFound;

	private bool setup;

	private float drainPerSecond;

	private static float drainEveryXSecs;

	private static double epsilon;

	private float leftToDrain;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionPartResourceDrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ActionPartResourceDrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CFire_003Ed__21))]
	public override IEnumerator Fire()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool findVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool findPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void drainVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void drainPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float drainUnloadedPart(ProtoPartSnapshot InProtoPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> APRD_SetDropDownValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RunValidation(MissionEditorValidator validator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}
}
