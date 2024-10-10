using UnityEngine;

namespace Expansions.Missions.Editor;

public class MEGUI_PartPicker : MEGUI_Control
{
	public string _selectedPartsColorString;

	public Color _selectedPartsColor;

	public string getExcludedPartsFilter;

	public string updatePartnerExcludedPartsFilter;

	public string _dialogTitle;

	public string SelectedPartsColorString
	{
		get
		{
			return _selectedPartsColorString;
		}
		set
		{
			_selectedPartsColor = Color.white;
			ColorUtility.TryParseHtmlString(value, out _selectedPartsColor);
			_selectedPartsColorString = ColorUtility.ToHtmlStringRGB(_selectedPartsColor);
		}
	}

	public Color SelectedPartsColor
	{
		get
		{
			return _selectedPartsColor;
		}
		set
		{
			_selectedPartsColor = value;
			_selectedPartsColorString = ColorUtility.ToHtmlStringRGB(_selectedPartsColor);
		}
	}

	public string DialogTitle
	{
		get
		{
			return _dialogTitle;
		}
		set
		{
			_dialogTitle = value;
		}
	}

	public MEGUI_PartPicker()
	{
		SelectedPartsColor = Color.green;
		getExcludedPartsFilter = null;
	}
}
