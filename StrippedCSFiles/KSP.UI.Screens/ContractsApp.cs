using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Contracts;
using UnityEngine;

namespace KSP.UI.Screens;

public class ContractsApp : UIApp
{
	[CompilerGenerated]
	private sealed class _003CUpdateDaemon_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ContractsApp _003C_003E4__this;

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
		public _003CUpdateDaemon_003Ed__31(int _003C_003E1__state)
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

	public static ContractsApp Instance;

	private Dictionary<Guid, UICascadingList.CascadingListItem> contractList;

	private string[] colors_levels;

	private string[] colors_notes;

	public float refreshTimeToWait;

	private bool refreshRequested;

	private List<Contract> refreshContracts;

	private bool refreshContractPos;

	private bool gamePaused;

	[SerializeField]
	private GenericAppFrame appFramePrefab;

	private GenericAppFrame appFrame;

	[SerializeField]
	private GenericCascadingList cascadingListPrefab;

	private GenericCascadingList cascadingList;

	[SerializeField]
	private UIListItem_spacer listItem_BodyContractState_prefab;

	[SerializeField]
	private UIListItem_spacer listItem_BodyContractNote_prefab;

	private bool updateDaemonRunning;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ContractsApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnAppAboutToStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DisplayApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void HideApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override ApplicationLauncher.AppScenes GetAppScenes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnContractsLoaded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveContract(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddContract(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshContractParameter(Contract contract, ContractParameter parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshContractRequest(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshContractRequested(Contract contract, bool changePositions = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshContract(Contract contract, bool changePositions = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateDaemon_003Ed__31))]
	private IEnumerator UpdateDaemon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateContractsList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UICascadingList.CascadingListItem CreateItem(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<UIListItem> CreateParameterList(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddSubParameter(List<UIListItem> tasks, ContractParameter p, int level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateLauncherButtonPlayAnim()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateLauncherButtonStopAnim()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGamePause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGameUnPause()
	{
		throw null;
	}
}
