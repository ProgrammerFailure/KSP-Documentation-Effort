using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class CraftEntry : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CLoadSteamItemPreviewURL_003Ed__48 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CraftEntry _003C_003E4__this;

		private UnityWebRequest _003Cwww_003E5__2;

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
		public _003CLoadSteamItemPreviewURL_003Ed__48(int _003C_003E1__state)
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

	public string craftName;

	public string fullFilePath;

	private Callback<CraftEntry> OnSelected;

	private const string squadExpansionFolder = "SquadExpansion";

	private const string shipsFolder = "Ships";

	private const string thumbnailFolder = "thumbs";

	private const string thumbnailFolderPrefix = "@";

	private const string folderSeparator = "/";

	private float defaultLayoutHeight;

	private static ScrollRect scrollView;

	public int partCount;

	public int stageCount;

	public bool isStock;

	public bool isValid;

	public bool steamItem;

	private ShipTemplate _template;

	public VersionCompareResult compatibility;

	public CraftProfileInfo craftProfileInfo;

	public Texture2D thumbnail;

	public string thumbURL;

	[SerializeField]
	private Toggle tgtCtrl;

	[SerializeField]
	private TextMeshProUGUI header1;

	[SerializeField]
	private TextMeshProUGUI header2;

	[SerializeField]
	private TextMeshProUGUI fieldStats;

	[SerializeField]
	private TextMeshProUGUI fieldCost;

	[SerializeField]
	private TextMeshProUGUI fieldPath;

	[SerializeField]
	private TextMeshProUGUI fieldMsg;

	[SerializeField]
	private RawImage craftThumbImg;

	[SerializeField]
	private GameObject imageSteam;

	private DirectoryController directoryController;

	private ConfigNode _configNode;

	public SteamCraftInfo steamCraftInfo;

	public ShipTemplate template
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Toggle Toggle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ConfigNode configNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CraftEntry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CraftEntry Create(FileInfo fInfo, bool stock, Callback<CraftEntry> OnSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CraftEntry Create(FileInfo fInfo, bool stock, Callback<CraftEntry> OnSelected, bool steamItem, SteamCraftInfo steamCraftInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Init(FileInfo fInfo, bool stock)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Init(FileInfo fInfo, bool stock, bool steamItem, SteamCraftInfo steamCraftInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDirectoryController(DirectoryController directoryController)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Texture2D GetThumbnail(string thumbURL, FileInfo fileInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetThumbURL(FileInfo fileInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsExpansion(FileInfo fileInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetStockThumbnailPath(FileInfo fileInfo, string editorFolder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadSteamItemPreviewURL_003Ed__48))]
	private IEnumerator LoadSteamItemPreviewURL()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UIUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UIUpdate(SteamCraftInfo steamCraftInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UIUpdateSteamField(SteamCraftInfo steamCraftInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetCostTextColorTag(float cost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onValueChanged(bool st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ulong GetSteamFileId()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowPath(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowPath()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetDisplayPathFrom(string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HidePath()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateWidgetSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AttachListener(GameObject target, EventTriggerType triggerType, Func<PointerEventData, bool> callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OnPointerEnter(BaseEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OnPointerExit(BaseEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OnScroll(PointerEventData data)
	{
		throw null;
	}
}
