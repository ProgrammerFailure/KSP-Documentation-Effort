using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScenarioLoadDialog : MonoBehaviour
{
	public delegate void FinishedCallback(ScenarioSaveInfo save);

	public class ScenarioSaveInfo
	{
		public class GameStateInfo
		{
			public Game game;

			public string fullFilePath;

			public int vesselCount;

			public int UT;

			public int funds;

			public int science;

			public int reputationPercent;

			public int ongoingContracts;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public GameStateInfo()
			{
				throw null;
			}
		}

		public string name;

		public Texture2D banner;

		public GameStateInfo Initial;

		public GameStateInfo Current;

		public string savePath;

		public string resumeFolderFullPath;

		public GameStateInfo ToLoad
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public event Callback<Texture2D> OnBannerLoaded
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			add
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			remove
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ScenarioSaveInfo()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetBanner(Texture2D texture)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool CanRestart()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string LoadPath()
		{
			throw null;
		}
	}

	public enum ScenarioSet
	{
		[Description("#autoLOC_7001034")]
		Scenarios,
		[Description("#autoLOC_7001035")]
		Training
	}

	[CompilerGenerated]
	private sealed class _003CLoadBannerTexture_003Ed__39 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string folderPath;

		public ScenarioSaveInfo scn;

		private string _003CfilePathToLoad_003E5__2;

		private UnityWebRequest _003Cloader_003E5__3;

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
		public _003CLoadBannerTexture_003Ed__39(int _003C_003E1__state)
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
		private void _003C_003Em__Finally1()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public Texture2D scenarioIcon;

	public Texture2D tutorialIcon;

	public Material iconMaterial;

	public Color scnIconColor;

	public Color tutIconColor;

	public List<string> blacklistScenarios;

	[SerializeField]
	private Button btnLoad;

	[SerializeField]
	private TextMeshProUGUI btnLoadCaption;

	[SerializeField]
	private Button btnCancel;

	[SerializeField]
	private Button btnRestart;

	[SerializeField]
	private Button btnScnLink;

	[SerializeField]
	private TextMeshProUGUI btnScnLinkCaption;

	[SerializeField]
	private TextMeshProUGUI textScnDescription;

	[SerializeField]
	private TextMeshProUGUI textScnTitle;

	[SerializeField]
	private RawImage imgScnBanner;

	[SerializeField]
	private RectTransform scrollListContent;

	[SerializeField]
	private ToggleGroup listGroup;

	[SerializeField]
	private TextMeshProUGUI header;

	private List<DialogGUIToggleButton> items;

	private FinishedCallback OnFinishedCallback;

	private UISkinDef skin;

	private string directory;

	private bool confirmGameRestart;

	private ScenarioSet scnSet;

	private List<ScenarioSaveInfo> saves;

	private ScenarioSaveInfo selectedGame;

	private MenuNavigation menuNav;

	private static Dictionary<string, Texture2D> bannerDictionary;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScenarioLoadDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ScenarioLoadDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScenarioLoadDialog Create(FinishedCallback onDismiss, ScenarioSet scnSet, UISkinDef skin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetExplicitNavigation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateBlacklist()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GatherSaveFiles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ScenarioSaveInfo GatherScenarioInfo(string file, string pathRoot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadBannerTexture_003Ed__39))]
	private IEnumerator LoadBannerTexture(ScenarioSaveInfo scn, string folderPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateWidget(ScenarioSaveInfo save, DialogGUIVerticalLayout vLayout)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearListItems()
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
	private void OnButtonRestart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfirmRestart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DismissRestartGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnSelectionChanged(bool haveSelection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnBtnScnLink()
	{
		throw null;
	}
}
