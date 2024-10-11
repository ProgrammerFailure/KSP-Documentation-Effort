using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class CheckpointBrowserDialog : MonoBehaviour
{
	public delegate void SelectFileCallback(string fullPath);

	public delegate void CancelledCallback();

	[CompilerGenerated]
	private sealed class _003CValidateCheckpoints_003Ed__33 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CheckpointBrowserDialog _003C_003E4__this;

		private int _003Ci_003E5__2;

		private int _003CcC_003E5__3;

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
		public _003CValidateCheckpoints_003Ed__33(int _003C_003E1__state)
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

	[SerializeField]
	private TextMeshProUGUI headerTitle;

	public CheckpointEntry mePrefab;

	protected string title;

	public SelectFileCallback OnFileSelected;

	public CancelledCallback OnBrowseCancelled;

	[SerializeField]
	private Button btnCancel;

	[SerializeField]
	private Button btnTest;

	[SerializeField]
	private Button btnReset;

	[SerializeField]
	private Button btnRemoveDirty;

	[SerializeField]
	private ToggleGroup listGroup;

	[SerializeField]
	private RectTransform listContainer;

	[SerializeField]
	private ScrollRect scrollRect;

	[SerializeField]
	protected UISkinDefSO uiSkin;

	protected UISkinDef skin;

	protected List<CheckpointEntry> checkpointList;

	protected CheckpointEntry selectedEntry;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CheckpointBrowserDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CheckpointBrowserDialog Spawn(SelectFileCallback onFileSelected, CancelledCallback onCancel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnButtonCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnButtonTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void TestSelectedCheckpoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void InjectMissionDataIntoCheckpoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnButtonReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnResetConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnButtonRemoveDirty()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRemoveDirtyConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRemoveDirtyDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Show()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildCheckpointList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CValidateCheckpoints_003Ed__33))]
	private IEnumerator ValidateCheckpoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int MissionCompare(CheckpointEntry a, CheckpointEntry b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddMissionEntryWidget(CheckpointEntry entry, RectTransform listParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearMissionList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearSelection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnSelectionChanged(CheckpointEntry selectedEntry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnEntrySelected(CheckpointEntry entry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDeleteEntry(CheckpointEntry entry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeleteConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeleteDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DeleteEntry(CheckpointEntry entry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> ct)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Lock(string lockID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Unlock(string lockID)
	{
		throw null;
	}
}
