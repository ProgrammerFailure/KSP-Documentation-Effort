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

public class MissionsBrowserDialog : MonoBehaviour
{
	public delegate void SelectFileCallback(string fullPath);

	public delegate void CancelledCallback();

	[CompilerGenerated]
	private sealed class _003CBuildMissionListRoutine_003Ed__40 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MissionsBrowserDialog _003C_003E4__this;

		private List<MissionFileInfo> _003CmissionFiles_003E5__2;

		private int _003Cc_003E5__3;

		private int _003Cj_003E5__4;

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
		public _003CBuildMissionListRoutine_003Ed__40(int _003C_003E1__state)
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

	private DictionaryValueList<DialogGUIToggleButton, MissionFileInfo> items;

	protected string title;

	private Texture2D currentIcon;

	public Texture2D steamIcon;

	public SelectFileCallback OnFileSelected;

	public CancelledCallback OnBrowseCancelled;

	[SerializeField]
	private Toggle tabUserCreated;

	[SerializeField]
	private Toggle tabStock;

	[SerializeField]
	private Button btnCancel;

	[SerializeField]
	private Button btnLoad;

	[SerializeField]
	private Button btnDelete;

	[SerializeField]
	private ToggleGroup listGroup;

	[SerializeField]
	private RectTransform listContainer;

	[SerializeField]
	protected UISkinDefSO uiSkin;

	protected UISkinDef skin;

	private PopupDialog window;

	private MissionFileInfo selectedMission;

	private MissionTypes selectedType;

	public Texture2D missionNormalIcon;

	public Texture2D missionHardIcon;

	[SerializeField]
	private List<Texture2D> missionDifficultyButtons;

	[SerializeField]
	private CanvasGroup canvasGroup;

	private DictionaryValueList<string, MissionListGroup> packGroups;

	private List<MissionPack> missionPacks;

	private Coroutine buildListCoroutine;

	private static float maxFrameTime;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionsBrowserDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MissionsBrowserDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionsBrowserDialog Spawn(SelectFileCallback onFileSelected, CancelledCallback onCancel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onButtonLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfirmLoadGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onButtonCancel()
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
	private void onMissionImported()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildMissionList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CBuildMissionListRoutine_003Ed__40))]
	private IEnumerator BuildMissionListRoutine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisplayAddedMissions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildPacksList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateWidget(MissionFileInfo mission, DialogGUIVerticalLayout vLayout)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearListItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearSelection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnSelectionChanged(bool haveSelection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onButtonDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PromptDeleteFileConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeleteConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnTabUser(bool st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnTabStock(bool st)
	{
		throw null;
	}
}
