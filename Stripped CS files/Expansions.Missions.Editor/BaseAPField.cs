using System;
using System.Reflection;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class BaseAPField : BaseField<MEGUI_Control>
{
	[SerializeField]
	public bool _gapDisplay;

	[SerializeField]
	public string _group;

	[SerializeField]
	public string _groupDisplayName;

	public MethodInfo _onValueChange;

	public MethodInfo _onControlCreated;

	public MethodInfo _onControlSetupComplete;

	public MethodInfo _compareValuesForCheckpoint;

	public PropertyInfo _propertyInfo;

	public bool _hideWhenSiblingsExist;

	public bool _hideWhenStartNode;

	public bool _hideWhenDocked;

	public bool _hideWhenInputConnected;

	public bool _hideWhenOutputConnected;

	public bool _hideWhenNoTestModules;

	public bool _hideWhenNoActionModules;

	public bool _hideOnSetup;

	[SerializeField]
	public int _order;

	public bool _tabStop;

	[SerializeField]
	public bool _groupStartCollapsed;

	public bool gapDisplay
	{
		get
		{
			return _gapDisplay;
		}
		set
		{
			_gapDisplay = value;
		}
	}

	public MethodInfo OnValueChange => _onValueChange;

	public MethodInfo OnControlCreated => _onControlCreated;

	public MethodInfo OnControlSetupComplete => _onControlSetupComplete;

	public MethodInfo CompareValuesForCheckpoint => _compareValuesForCheckpoint;

	public PropertyInfo PropertyInfo
	{
		get
		{
			return _propertyInfo;
		}
		set
		{
			_propertyInfo = value;
		}
	}

	public string FieldID => string.Concat(base.MemberInfo.DeclaringType, ".", base.name);

	public Type FieldType
	{
		get
		{
			if (_propertyInfo != null)
			{
				return _propertyInfo.PropertyType;
			}
			if (base.FieldInfo != null)
			{
				return base.FieldInfo.FieldType;
			}
			return null;
		}
	}

	public string Group
	{
		get
		{
			return _group;
		}
		set
		{
			_group = value;
		}
	}

	public string GroupDisplayName
	{
		get
		{
			return _groupDisplayName;
		}
		set
		{
			_groupDisplayName = value;
		}
	}

	public bool HideWhenSiblingsExist
	{
		get
		{
			return _hideWhenSiblingsExist;
		}
		set
		{
			_hideWhenSiblingsExist = value;
		}
	}

	public bool HideWhenStartNode
	{
		get
		{
			return _hideWhenStartNode;
		}
		set
		{
			_hideWhenStartNode = value;
		}
	}

	public bool HideWhenDocked
	{
		get
		{
			return _hideWhenDocked;
		}
		set
		{
			_hideWhenDocked = value;
		}
	}

	public bool HideWhenInputConnected
	{
		get
		{
			return _hideWhenInputConnected;
		}
		set
		{
			_hideWhenInputConnected = value;
		}
	}

	public bool HideWhenOutputConnected
	{
		get
		{
			return _hideWhenOutputConnected;
		}
		set
		{
			_hideWhenOutputConnected = value;
		}
	}

	public bool HideWhenNoTestModules
	{
		get
		{
			return _hideWhenNoTestModules;
		}
		set
		{
			_hideWhenNoTestModules = value;
		}
	}

	public bool HideWhenNoActionModules
	{
		get
		{
			return _hideWhenNoActionModules;
		}
		set
		{
			_hideWhenNoActionModules = value;
		}
	}

	public bool HideOnSetup
	{
		get
		{
			return _hideOnSetup;
		}
		set
		{
			_hideOnSetup = value;
		}
	}

	public int Order
	{
		get
		{
			return _order;
		}
		set
		{
			_order = value;
		}
	}

	public bool TabStop
	{
		get
		{
			return _tabStop;
		}
		set
		{
			_tabStop = value;
		}
	}

	public bool GroupStartCollapsed
	{
		get
		{
			return _groupStartCollapsed;
		}
		set
		{
			_groupStartCollapsed = value;
		}
	}

	public BaseAPField(MEGUI_Control fieldAttrib, MemberInfo fieldInfo, object host)
		: base(fieldAttrib, fieldInfo, host)
	{
		_gapDisplay = fieldAttrib.gapDisplay;
		_group = fieldAttrib.group;
		_groupDisplayName = fieldAttrib.groupDisplayName;
		_propertyInfo = fieldInfo as PropertyInfo;
		if (!string.IsNullOrEmpty(fieldAttrib.onValueChange))
		{
			_onValueChange = host.GetType().GetMethod(fieldAttrib.onValueChange, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { FieldType }, null);
		}
		if (!string.IsNullOrEmpty(fieldAttrib.onControlCreated))
		{
			_onControlCreated = host.GetType().GetMethod(fieldAttrib.onControlCreated, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}
		if (!string.IsNullOrEmpty(fieldAttrib.onControlSetupComplete))
		{
			_onControlSetupComplete = host.GetType().GetMethod(fieldAttrib.onControlSetupComplete, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}
		if (!string.IsNullOrEmpty(fieldAttrib.compareValuesForCheckpoint))
		{
			_compareValuesForCheckpoint = host.GetType().GetMethod(fieldAttrib.compareValuesForCheckpoint, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { FieldType }, null);
			if (_compareValuesForCheckpoint != null && _compareValuesForCheckpoint.ReturnType != typeof(bool))
			{
				_compareValuesForCheckpoint = null;
				Debug.Log("[Checkpoint validation]: The method " + fieldAttrib.compareValuesForCheckpoint + " has an invalid return type.");
			}
		}
		_hideWhenSiblingsExist = fieldAttrib.hideWhenSiblingsExist;
		_hideWhenStartNode = fieldAttrib.hideWhenStartNode;
		_hideWhenDocked = fieldAttrib.hideWhenDocked;
		_hideWhenInputConnected = fieldAttrib.hideWhenInputConnected;
		_hideWhenOutputConnected = fieldAttrib.hideWhenOutputConnected;
		_hideWhenNoTestModules = fieldAttrib.hideWhenNoTestModules;
		_hideWhenNoActionModules = fieldAttrib.hideWhenNoActionModules;
		_hideOnSetup = fieldAttrib.hideOnSetup;
		_order = fieldAttrib.order;
		_tabStop = fieldAttrib.tabStop;
		_groupStartCollapsed = fieldAttrib.groupStartCollapsed;
	}

	public bool SetValue(object newValue)
	{
		if (base.MemberInfo.MemberType == MemberTypes.Field)
		{
			if (SetValue(newValue, base.host))
			{
				if (_onValueChange != null)
				{
					_onValueChange.Invoke(base.host, new object[1] { newValue });
				}
				MissionEditorValidator.RunValidationOnParamChange();
				return true;
			}
			return false;
		}
		try
		{
			_propertyInfo.SetValue(base.host, newValue, null);
			if (_onValueChange != null)
			{
				_onValueChange.Invoke(base.host, new object[1] { newValue });
			}
			MissionEditorValidator.RunValidationOnParamChange();
			return true;
		}
		catch (Exception ex)
		{
			PDebug.Error(string.Concat("Value '", newValue, "' could not be set to field '", base.name, "'"));
			PDebug.Error(ex.Message + "\n" + ex.StackTrace + "\n" + ex.Data);
			return false;
		}
	}

	public object GetValue()
	{
		if (base.MemberInfo.MemberType == MemberTypes.Field)
		{
			return GetValue(base.host);
		}
		try
		{
			return _propertyInfo.GetValue(base.host, null);
		}
		catch
		{
			PDebug.Error("Value could not be retrieved from field '" + base.name + "'");
			return null;
		}
	}

	public T GetValue<T>()
	{
		return (T)GetValue();
	}

	public override void SetOriginalValue()
	{
		if (base.MemberInfo.MemberType == MemberTypes.Field)
		{
			base.SetOriginalValue();
		}
		else
		{
			base.originalValue = _propertyInfo.GetValue(base.host, null);
		}
	}
}
