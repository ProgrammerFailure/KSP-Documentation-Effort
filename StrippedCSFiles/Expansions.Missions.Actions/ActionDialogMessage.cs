using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using KSP.UI.Screens;

namespace Expansions.Missions.Actions;

public class ActionDialogMessage : ActionModule
{
	public enum DialogMessageArea
	{
		[Description("#autoLOC_8002036")]
		Left,
		[Description("#autoLOC_8002037")]
		Center,
		[Description("#autoLOC_8002038")]
		Right
	}

	[CompilerGenerated]
	private sealed class _003CFire_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ActionDialogMessage _003C_003E4__this;

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
		public _003CFire_003Ed__13(int _003C_003E1__state)
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

	[MEGUI_TextArea(tabStop = true, checkpointValidation = CheckpointValidationType.None, resetValue = "", guiName = "#autoLOC_8006000")]
	public string messageHeading;

	[MEGUI_TextArea(tabStop = true, checkpointValidation = CheckpointValidationType.None, resetValue = "", guiName = "#autoLOC_8006001")]
	public string message;

	[MEGUI_MissionInstructor(gapDisplay = true, guiName = "#autoLOC_8006002")]
	public MissionInstructor missionInstructor;

	[MEGUI_NumberRange(minValue = 135f, checkpointValidation = CheckpointValidationType.None, canBePinned = false, maxValue = 300f, resetValue = "135", guiName = "#autoLOC_8006004", Tooltip = "#autoLOC_8006005")]
	public int textAreaSize;

	[MEGUI_Dropdown(checkpointValidation = CheckpointValidationType.None, canBePinned = false, resetValue = "", guiName = "#autoLOC_8002034", Tooltip = "#autoLOC_8002035")]
	public DialogMessageArea screenArea;

	[MEGUI_Checkbox(guiName = "#autoLOC_8002031", Tooltip = "#autoLOC_8002032")]
	public bool pauseMission;

	[MEGUI_Checkbox(onValueChange = "OnautoClose", canBePinned = false, checkpointValidation = CheckpointValidationType.None, guiName = "#autoLOC_8002039", Tooltip = "#autoLOC_8002040")]
	public bool autoClose;

	[MEGUI_NumberRange(minValue = 10f, canBePinned = false, checkpointValidation = CheckpointValidationType.None, onControlCreated = "OnautoCloseTimeoutCreated", maxValue = 120f, resetValue = "20", guiName = "#autoLOC_8002041", Tooltip = "#autoLOC_8002042")]
	public int autoCloseTimeOut;

	public bool isBriefingMessage;

	public bool autoGrowDialogHeight;

	protected MEGUIParameterNumberRange autoCloseTimeOutRange;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionDialogMessage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CFire_003Ed__13))]
	public override IEnumerator Fire()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void dialogClosed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SendStateMessage(string title, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnautoCloseTimeoutCreated(MEGUIParameterNumberRange parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnautoClose(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ParameterSetupComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
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
