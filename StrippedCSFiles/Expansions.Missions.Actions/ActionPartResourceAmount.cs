using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Actions;

public class ActionPartResourceAmount : ActionVessel
{
	[CompilerGenerated]
	private sealed class _003CFire_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ActionPartResourceAmount _003C_003E4__this;

		private PartResourceDefinition _003Cdef_003E5__2;

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
		public _003CFire_003Ed__4(int _003C_003E1__state)
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

	[MEGUI_Dropdown(onControlSetupComplete = "OnResourceNameControlSetup", order = 10, SetDropDownItems = "SetResourceNames", guiName = "#autoLOC_8000014", Tooltip = "#autoLOC_8000018")]
	public string resourceName;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 20, resetValue = "0", guiName = "#autoLOC_8000019", Tooltip = "#autoLOC_8000020")]
	public float adjustAmount;

	private static double epsilon;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionPartResourceAmount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ActionPartResourceAmount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CFire_003Ed__4))]
	public override IEnumerator Fire()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double adjustUnloadedPart(ProtoPartSnapshot InProtoPart, double adjustAmount, bool pulling)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> SetResourceNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnResourceNameControlSetup(MEGUIParameterDropdownList parameter)
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
