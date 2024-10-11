using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameDialog : MonoBehaviour
{
	public delegate void FinishedCallback(string path);

	internal class PlayerProfileInfo : IConfigNode
	{
		public string name;

		public int vesselCount;

		public double UT;

		public Game.Modes gameMode;

		public bool gameNull;

		public bool gameCompatible;

		public double funds;

		public int science;

		public int reputationPercent;

		public int ongoingContracts;

		public string kspVersion;

		public float missionCurrentScore;

		public bool missionIsStarted;

		public bool missionIsEnded;

		public Guid missionHistoryId;

		public string missionExpansionVersion;

		public List<string> testModules;

		public List<string> actionModules;

		public bool errorAccess;

		public string errorDetails;

		public string saveMD5;

		public long lastWriteTime;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PlayerProfileInfo()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PlayerProfileInfo(string filename, string saveFolder, Game game)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PlayerProfileInfo LoadDetailsFromGame(Game game)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Save(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void LoadFromMetaFile(string filename, string saveFolder)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SaveToMetaFile(string filename, string saveFolder)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string GetSFSMD5(string filename, string saveFolder)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static long GetLastWriteTime(string filename, string saveFolder)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal bool AreVesselsInFlightCompatible(string filename, string saveFolder, ref string errorString)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool IsCompatible()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CLoadMenuNav_003Ed__42 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LoadGameDialog _003C_003E4__this;

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
		public _003CLoadMenuNav_003Ed__42(int _003C_003E1__state)
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

	public Sprite careerIcon;

	public Sprite scienceSandboxIcon;

	public Sprite sandboxIcon;

	public Sprite scenarioIcon;

	public Sprite tutorialIcon;

	[SerializeField]
	private Button btnLoad;

	[SerializeField]
	private Button btnCancel;

	[SerializeField]
	private Button btnDelete;

	[SerializeField]
	private RectTransform scrollListContent;

	[SerializeField]
	private ToggleGroup listGroup;

	[SerializeField]
	private TextMeshProUGUI header;

	[SerializeField]
	private TMP_InputField searchInput;

	private List<DialogGUIToggleButton> items;

	private FinishedCallback OnFinishedCallback;

	private UISkinDef skin;

	private string directory;

	private bool persistent;

	private string selectedGame;

	private PlayerProfileInfo selectedSave;

	private PopupDialog popupdialog;

	private MenuNavigation menuNav;

	private List<PlayerProfileInfo> saves;

	private bool confirmGameDelete;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LoadGameDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBtnCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnSelectionChanged(bool haveSelection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfirmLoadGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CancelLoadGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfirmDeleteGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DismissDeleteGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LoadGameDialog Create(FinishedCallback onDismiss, string directory, bool persistent, UISkinDef skin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetHidden(bool hide)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetLocked(bool locked)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static PlayerProfileInfo GetSaveData(PlayerProfileInfo save, string fname, string folder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PersistentLoadGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateLoadList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadMenuNav_003Ed__42))]
	private IEnumerator LoadMenuNav()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearListItems()
	{
		throw null;
	}
}
