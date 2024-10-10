using TMPro;
using UnityEngine;

namespace ns21;

public class SettingsControlBase : MonoBehaviour
{
	public TextMeshProUGUI titleText;

	public bool ignoreEmptySetting;

	public bool IgnoreEmptySetting
	{
		get
		{
			return ignoreEmptySetting;
		}
		set
		{
			ignoreEmptySetting = value;
		}
	}

	public virtual void OnApply()
	{
	}

	public virtual void OnRevert()
	{
	}

	public void SetTitleText(string title)
	{
		if (titleText != null && !string.IsNullOrEmpty(title))
		{
			titleText.text = title;
		}
	}

	public virtual bool IsValid()
	{
		return true;
	}
}
