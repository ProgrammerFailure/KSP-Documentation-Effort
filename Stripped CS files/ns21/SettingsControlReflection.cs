using System;
using ns20;
using TMPro;
using UnityEngine;

namespace ns21;

public class SettingsControlReflection : SettingsControlBase
{
	public string settingName;

	public TextMeshProUGUI valueText;

	public AccessorBase accessor;

	public object value;

	public object Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
			ValueUpdated();
			SettingsScreen.Instance.BroadcastMessage("SettingChanged", settingName + ";" + value.ToString(), SendMessageOptions.DontRequireReceiver);
		}
	}

	public void Start()
	{
		OnStart();
		accessor = GetAccessor(settingName);
		OnRevert();
	}

	public virtual AccessorBase GetAccessor(string settingName)
	{
		try
		{
			AccessorBase accessorBase = AccessorBase.Create(typeof(GameSettings), null, settingName);
			if (accessorBase == null && !base.IgnoreEmptySetting)
			{
				Debug.LogError("SettingControl '" + titleText.text + "': Cannot find GameSetting field named '" + settingName + "'", base.gameObject);
				return null;
			}
			return accessorBase;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
		return null;
	}

	public virtual void OnStart()
	{
	}

	public override void OnApply()
	{
		PreApply();
		SetValue();
	}

	public virtual void PreApply()
	{
	}

	public override void OnRevert()
	{
		if (accessor != null)
		{
			GetValue();
			ValueInitialized();
			ValueUpdated();
		}
	}

	public virtual void ValueInitialized()
	{
	}

	public virtual void ValueUpdated()
	{
	}

	public virtual void GetValue()
	{
		object obj = accessor.Value;
		if (obj != null && obj is ICloneable)
		{
			value = (obj as ICloneable).Clone();
		}
		else
		{
			value = obj;
		}
	}

	public virtual void SetValue()
	{
		if (accessor != null)
		{
			accessor.Value = value;
		}
	}
}
