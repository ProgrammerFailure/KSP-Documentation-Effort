using System.Runtime.CompilerServices;
using Contracts;
using Contracts.Agents;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class MissionControl : MonoBehaviour
{
	public enum DisplayMode
	{
		Available,
		Active,
		Archive
	}

	public enum ArchiveMode
	{
		All,
		Completed,
		Failed,
		Cancelled
	}

	public class MissionSelection
	{
		public bool isAvailable;

		public Contract contract;

		public UIListItem listItem;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MissionSelection(bool isAvailable, Contract contract, UIListItem listItem)
		{
			throw null;
		}
	}

	public RectTransform panelView;

	public MCListItem PrfbMissionListItem;

	public UIList scrollListContracts;

	public TextMeshProUGUI contractText;

	public ScrollRect contractTextRect;

	public RawImage logoRenderer;

	public TextMeshProUGUI textContractInfo;

	public TextMeshProUGUI textDateInfo;

	public TextMeshProUGUI textMCStats;

	public Button btnAccept;

	public Button btnDecline;

	public Button btnCancel;

	public Button btnAgentInfo;

	public Button btnAgentBack;

	public Toggle toggleDisplayModeAvailable;

	public Toggle toggleDisplayModeActive;

	public Toggle toggleDisplayModeArchive;

	public RectTransform toggleArchiveGroup;

	public Toggle toggleArchiveAll;

	public Toggle toggleArchiveCompleted;

	public Toggle toggleArchiveFailed;

	public Toggle toggleArchiveCancelled;

	public TextMeshProUGUI instructorText;

	public RawImage instructorRawImage;

	public KerbalInstructor instructorPrefab;

	public RenderTexture instructorTexture;

	public int instructorPortraitSize;

	private KerbalInstructor instructor;

	public MCAvatarController avatarController;

	private float lastOnSelectSoundTrigger;

	public MissionSelection selectedMission;

	private bool isShowingAgentInformation;

	private int maxActiveContracts;

	private int activeContractCount;

	public DisplayMode displayMode;

	public ArchiveMode archiveMode;

	public static MissionControl Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDisplayModeAvailable(bool force = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDisplayModeActive(bool force = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDisplayModeArchive(bool force = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetArchiveMode(ArchiveMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnClickAvailable(bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnClickActive(bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnClickArchive(bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnClickArchiveAll(bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnClickArchiveCompleted(bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnClickArchiveFailed(bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnClickArchiveCancelled(bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickAccept()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickDecline()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickAgentInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSelectContract(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeselectContract(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddItem(Contract contract, bool isAvailable, string label = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateInfoPanelContract(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateInfoPanelAgent(Agent agent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearInfoPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetInstructorText(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupInstructor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyInstructor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateInstructor(MCAvatarController.AvatarState state, MCAvatarController.AvatarAnimation anim)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshContracts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshUIControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RebuildContractList()
	{
		throw null;
	}
}
