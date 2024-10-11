using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManeuverNodeEditorTabVectorInput : ManeuverNodeEditorTab
{
	[SerializeField]
	public TMP_InputField proRetrogradeField;

	private InputFieldHandler proRetrogradeFieldHandler;

	[SerializeField]
	public TMP_InputField normalField;

	private InputFieldHandler normalFieldHandler;

	[SerializeField]
	public TMP_InputField radialField;

	private InputFieldHandler radialFieldHandler;

	[SerializeField]
	public TMP_InputField timeField;

	private TimeInputFieldHandler timeFieldHander;

	[SerializeField]
	public Toggle utInputMode;

	[SerializeField]
	public TextMeshProUGUI timeUnits;

	[SerializeField]
	public TooltipController_Text utTooltip;

	private Vector3d newDeltaV;

	private ManeuverNodeEditorTabButton editorTab;

	public bool isEditing;

	public bool UTMode;

	private static string cacheAutoLOC_6002317;

	private static string cacheAutoLOC_6001498;

	private static string cacheAutoLOC_6005047;

	private static string cacheAutoLOC_6005048;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverNodeEditorTabVectorInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateUIElements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFieldSelected(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFieldDeselected(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetInitialValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTabToggleClicked(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ChangeInputMode(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CacheLocalStrings()
	{
		throw null;
	}
}
