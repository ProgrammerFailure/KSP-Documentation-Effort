using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class EditorActionOverrideGroup : UISelectableGridLayoutGroupItem, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
{
	public TextMeshProUGUI text;

	public Button editButton;

	public UIButtonToggle closeButton;

	public TMP_InputField inputField;

	public MenuNavigation menuNavigation;

	private string[] groupNames;

	private string groupName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	private bool canEdit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int groupOverride
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
	public EditorActionOverrideGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetName(string groupName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Select()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Deselect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(int groupOverride, string[] groupNames, bool isOpen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OpenGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CloseGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Collapse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EditButtonClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputFieldDone(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSubmit(BaseEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
