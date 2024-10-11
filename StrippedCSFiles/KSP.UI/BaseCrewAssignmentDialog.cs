using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class BaseCrewAssignmentDialog : MonoBehaviour
{
	public UIList scrollListCrew;

	public UIList scrollListAvail;

	[SerializeField]
	protected CrewListItem widgetCrew;

	[SerializeField]
	protected CrewListItem widgetCrewEmpty;

	[SerializeField]
	protected UIListItem widgetBorder;

	[SerializeField]
	protected Sprite disabledCrewListSprite;

	protected VesselCrewManifest defaultManifest;

	protected VesselCrewManifest listManifest;

	public static EventData<VesselCrewManifest> onCrewDialogChange;

	protected bool listIsValid;

	protected static Color disabledColor;

	public VesselCrewManifest CurrentManifestUnsafe
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public KerbalRoster CurrentCrewRoster
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseCrewAssignmentDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static BaseCrewAssignmentDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetCurrentCrewRoster(KerbalRoster newRoster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual KerbalRoster GetCurrentCrewRoster()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DropOnCrewList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DropOnAvailList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshCrewLists(VesselCrewManifest crewManifest, bool setAsDefault, bool updateUI, Func<PartCrewManifest, bool> displayFilter = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Refresh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDefaultManifest(VesselCrewManifest manifest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateCrewList(VesselCrewManifest manifest, Func<PartCrewManifest, bool> displayFilter = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateAvailList(VesselCrewManifest manifest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddCrewListBorder(string text, Color textColor, bool expandHeight = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddCrewItemEmpty(uint pUid, int index = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual CrewListItem AddItem(uint pUid, UIList list, ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AddAvailItem(ProtoCrewMember crew, UIList list = null, CrewListItem.ButtonTypes type = CrewListItem.ButtonTypes.V)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AddAvailItem(ProtoCrewMember crew, out CrewListItem item, UIList list = null, CrewListItem.ButtonTypes type = CrewListItem.ButtonTypes.V)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddAvailItem(string name, string trait, float xp, int level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddCrewItem(ProtoCrewMember crew, uint pUid)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ListItemButtonClick(CrewListItem.ButtonTypes type, CrewListItem clickItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void MoveCrewToEmptySeat(UIList fromlist, UIList tolist, UIListItem itemToMove, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void MoveCrewToAvail(UIList fromlist, UIList tolist, UIListItem itemToMove)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ButtonFill()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ButtonClear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ButtonReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselCrewManifest GetManifest(bool createClone = true)
	{
		throw null;
	}
}
