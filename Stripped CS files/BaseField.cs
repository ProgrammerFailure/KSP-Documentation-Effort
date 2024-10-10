using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ns9;
using UnityEngine;

[Serializable]
public class BaseField<T> where T : FieldAttribute
{
	[SerializeField]
	public string _name;

	[SerializeField]
	public string _guiName;

	public object _host;

	public object _originalValue;

	public FieldInfo _fieldInfo;

	public MemberInfo _memberInfo;

	public T _attribute;

	public bool _hasInterface;

	public static Dictionary<Type, bool> hasInterfaceDictionary = new Dictionary<Type, bool>();

	public string name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public string guiName
	{
		get
		{
			return _guiName;
		}
		set
		{
			_guiName = Localizer.Format(value);
		}
	}

	public object host
	{
		get
		{
			return _host;
		}
		set
		{
			_host = value;
		}
	}

	public object originalValue
	{
		get
		{
			return _originalValue;
		}
		set
		{
			_originalValue = value;
		}
	}

	public FieldInfo FieldInfo
	{
		get
		{
			return _fieldInfo;
		}
		set
		{
			_fieldInfo = value;
		}
	}

	public MemberInfo MemberInfo
	{
		get
		{
			return _memberInfo;
		}
		set
		{
			_memberInfo = value;
		}
	}

	public T Attribute => _attribute;

	public bool hasInterface
	{
		get
		{
			return _hasInterface;
		}
		set
		{
			_hasInterface = value;
		}
	}

	public event Callback<object> OnValueModified = delegate
	{
	};

	public BaseField()
	{
	}

	public BaseField(T fieldAttrib, MemberInfo memberInfo, object host)
	{
		_host = host;
		_attribute = fieldAttrib;
		_name = memberInfo.Name;
		_memberInfo = memberInfo;
		_fieldInfo = memberInfo as FieldInfo;
		if (_fieldInfo != null && !hasInterfaceDictionary.TryGetValue(_fieldInfo.FieldType, out _hasInterface))
		{
			_hasInterface = _fieldInfo.FieldType.GetInterface("IConfigNode") != null;
			hasInterfaceDictionary.Add(_fieldInfo.FieldType, _hasInterface);
		}
		guiName = (string.IsNullOrEmpty(fieldAttrib.guiName) ? memberInfo.Name : fieldAttrib.guiName);
	}

	public bool SetValue(object newValue, object host)
	{
		try
		{
			_fieldInfo.SetValue(host, newValue);
			this.OnValueModified(newValue);
			return true;
		}
		catch (Exception ex)
		{
			PDebug.Error(string.Concat("Value '", newValue, "' could not be set to field '", _name, "'"));
			PDebug.Error(ex.Message + "\n" + ex.StackTrace + "\n" + ex.Data);
			return false;
		}
	}

	public object GetValue(object host)
	{
		try
		{
			return _fieldInfo.GetValue(host);
		}
		catch
		{
			PDebug.Error("Value could not be retrieved from field '" + _name + "'");
			return null;
		}
	}

	public U GetValue<U>(object host)
	{
		return (U)GetValue(host);
	}

	public virtual void SetOriginalValue()
	{
		_originalValue = _fieldInfo.GetValue(_host);
	}
}
[Serializable]
public class BaseField : BaseField<KSPField>
{
	[SerializeField]
	public bool _isPersistant;

	[SerializeField]
	public bool _guiActive;

	[SerializeField]
	public bool _guiInteractable;

	[SerializeField]
	public bool _guiActiveEditor;

	[SerializeField]
	public bool _guiActiveUnfocused;

	[SerializeField]
	public float _guiUnfocusedRange;

	[SerializeField]
	public bool _guiRemovedIfPinned;

	[SerializeField]
	public string _guiUnits;

	[SerializeField]
	public string _guiFormat;

	[SerializeField]
	public string _category;

	[SerializeField]
	public bool _advancedTweakable;

	[SerializeField]
	public UI_Control _uiControlFlight;

	[SerializeField]
	public UI_Control _uiControlEditor;

	[SerializeField]
	public bool _uiControlOnly;

	public BasePAWGroup group;

	public static KSPUtil.ObjectActivator<UI_Label> objectActivatorUILabelLambdaExpression = null;

	public static Dictionary<Type, FieldInfo[]> typeToFieldInfoDictionary = new Dictionary<Type, FieldInfo[]>();

	public static Dictionary<int, UI_Control[]> fieldInfoToUIControlDictionary = new Dictionary<int, UI_Control[]>();

	[SerializeField]
	public bool _wasActiveBeforePartWasAdjusted;

	public static string cacheAutoLOC_8004438;

	public static string cacheAutoLOC_8004439;

	public bool isPersistant
	{
		get
		{
			return _isPersistant;
		}
		set
		{
			_isPersistant = value;
		}
	}

	public bool guiActive
	{
		get
		{
			return _guiActive;
		}
		set
		{
			_guiActive = value;
		}
	}

	public bool guiInteractable
	{
		get
		{
			return _guiInteractable;
		}
		set
		{
			_guiInteractable = value;
		}
	}

	public bool guiActiveEditor
	{
		get
		{
			return _guiActiveEditor;
		}
		set
		{
			_guiActiveEditor = value;
		}
	}

	public bool guiActiveUnfocused
	{
		get
		{
			return _guiActiveUnfocused;
		}
		set
		{
			_guiActiveUnfocused = value;
		}
	}

	public float guiUnfocusedRange
	{
		get
		{
			return _guiUnfocusedRange;
		}
		set
		{
			_guiUnfocusedRange = value;
		}
	}

	public bool guiRemovedIfPinned
	{
		get
		{
			return _guiRemovedIfPinned;
		}
		set
		{
			_guiRemovedIfPinned = value;
		}
	}

	public string guiUnits
	{
		get
		{
			return _guiUnits;
		}
		set
		{
			_guiUnits = Localizer.Format(value);
		}
	}

	public string guiFormat
	{
		get
		{
			return _guiFormat;
		}
		set
		{
			_guiFormat = value;
		}
	}

	public string category
	{
		get
		{
			return _category;
		}
		set
		{
			_category = value;
		}
	}

	public bool advancedTweakable
	{
		get
		{
			return _advancedTweakable;
		}
		set
		{
			_advancedTweakable = value;
		}
	}

	public UI_Control uiControlFlight
	{
		get
		{
			return _uiControlFlight;
		}
		set
		{
			_uiControlFlight = value;
		}
	}

	public UI_Control uiControlEditor
	{
		get
		{
			return _uiControlEditor;
		}
		set
		{
			_uiControlEditor = value;
		}
	}

	public bool uiControlOnly => _uiControlOnly;

	public bool WasActiveBeforePartWasAdjusted
	{
		get
		{
			return _wasActiveBeforePartWasAdjusted;
		}
		set
		{
			_wasActiveBeforePartWasAdjusted = value;
		}
	}

	public BaseField(KSPField fieldAttrib, FieldInfo fieldInfo, object host)
		: base(fieldAttrib, (MemberInfo)fieldInfo, host)
	{
		_uiControlOnly = false;
		_guiInteractable = true;
		_isPersistant = fieldAttrib.isPersistant;
		base.FieldInfo = fieldInfo;
		bool value = false;
		if (!BaseField<KSPField>.hasInterfaceDictionary.TryGetValue(fieldInfo.FieldType, out value))
		{
			value = fieldInfo.FieldType.GetInterface("IConfigNode") != null;
			BaseField<KSPField>.hasInterfaceDictionary.Add(fieldInfo.FieldType, value);
		}
		base.hasInterface = value;
		_guiActive = fieldAttrib.guiActive;
		_guiActiveEditor = fieldAttrib.guiActiveEditor;
		_guiActiveUnfocused = fieldAttrib.guiActiveUnfocused;
		_guiUnfocusedRange = fieldAttrib.unfocusedRange;
		base.guiName = fieldAttrib.guiName;
		guiUnits = fieldAttrib.guiUnits;
		_guiFormat = fieldAttrib.guiFormat;
		_category = fieldAttrib.category;
		_advancedTweakable = fieldAttrib.advancedTweakable;
		group = new BasePAWGroup(fieldAttrib.groupName, fieldAttrib.groupDisplayName, fieldAttrib.groupStartCollapsed);
		SetupUIControls();
	}

	public BaseField(UI_Control fieldAttrib, FieldInfo fieldInfo, object host)
	{
		base.host = host;
		base.name = fieldInfo.Name;
		_isPersistant = false;
		_guiInteractable = true;
		base.FieldInfo = fieldInfo;
		bool value = false;
		if (!BaseField<KSPField>.hasInterfaceDictionary.TryGetValue(fieldInfo.FieldType, out value))
		{
			value = fieldInfo.FieldType.GetInterface("IConfigNode") != null;
			BaseField<KSPField>.hasInterfaceDictionary.Add(fieldInfo.FieldType, value);
		}
		base.hasInterface = fieldInfo.FieldType.GetInterface("IConfigNode") != null;
		_guiActive = true;
		base.guiName = fieldInfo.Name;
		guiUnits = "";
		_guiFormat = "";
		_category = "";
		_advancedTweakable = false;
		group = new BasePAWGroup();
		_uiControlOnly = false;
		SetupUIControls();
	}

	public virtual void CopyField(BaseField field)
	{
		if (isPersistant)
		{
			SetValue(field.GetValue(field.host), base.host);
		}
	}

	public void SetupUIControls()
	{
		UI_Control[] value = null;
		int hashCode = base.FieldInfo.GetHashCode();
		if (!fieldInfoToUIControlDictionary.TryGetValue(hashCode, out value))
		{
			value = (UI_Control[])base.FieldInfo.GetCustomAttributes(typeof(UI_Control), inherit: false);
			fieldInfoToUIControlDictionary.Add(hashCode, value);
		}
		if (value != null && value.Length != 0)
		{
			int i = 0;
			for (int num = value.Length; i < num; i++)
			{
				UI_Control uI_Control = value[i];
				if ((uI_Control.scene & UI_Scene.Flight) != 0)
				{
					if (_uiControlFlight == null)
					{
						_uiControlFlight = SetupUIControl(uI_Control);
					}
					else
					{
						Debug.LogError("BaseField " + base.FieldInfo.Name + " in " + base.host.GetType().Name + " has too many UICtrls marked for Flight. Max == 1. UICtrl scene set to " + (int)uI_Control.scene);
					}
				}
				if ((uI_Control.scene & UI_Scene.Editor) != 0)
				{
					if (_uiControlEditor == null)
					{
						_uiControlEditor = SetupUIControl(uI_Control);
						guiActiveEditor = true;
						continue;
					}
					Debug.LogError("BaseField " + base.FieldInfo.Name + " in " + base.host.GetType().Name + " has too many UICtrls marked for Editor. Max == 1. UICtrl scene set to " + (int)uI_Control.scene);
				}
			}
		}
		if (_uiControlEditor == null)
		{
			_uiControlEditor = SetupUIControlLabel();
		}
		if (_uiControlFlight == null)
		{
			_uiControlFlight = SetupUIControlLabel();
		}
	}

	public UI_Control SetupUIControl(UI_Control attr)
	{
		UI_Control uI_Control = (UI_Control)Activator.CreateInstance(attr.GetType());
		uI_Control.Setup(this);
		FieldInfo[] value = null;
		if (!typeToFieldInfoDictionary.TryGetValue(attr.GetType(), out value))
		{
			value = attr.GetType().GetFields();
			typeToFieldInfoDictionary.Add(attr.GetType(), value);
		}
		int i = 0;
		for (int num = value.Length; i < num; i++)
		{
			FieldInfo fieldInfo = value[i];
			fieldInfo.SetValue(uI_Control, fieldInfo.GetValue(attr));
		}
		return uI_Control;
	}

	public UI_Control SetupUIControlLabel()
	{
		if (objectActivatorUILabelLambdaExpression == null)
		{
			objectActivatorUILabelLambdaExpression = KSPUtil.GetActivator<UI_Label>(typeof(UI_Label).GetConstructors()[0]);
		}
		UI_Label uI_Label = objectActivatorUILabelLambdaExpression();
		uI_Label.Setup(this);
		return uI_Label;
	}

	public void Read(string value, object host)
	{
		ReadPvt(base.FieldInfo, value, host);
	}

	public void Write(StreamWriter sw, string tabIndent, object host)
	{
		if (!_isPersistant)
		{
			return;
		}
		Type fieldType = base.FieldInfo.FieldType;
		object value = GetValue(host);
		sw.Write(tabIndent + base.name + " = ");
		if (value != null)
		{
			if (fieldType == typeof(Vector2))
			{
				Vector2 vector = (Vector2)value;
				sw.WriteLine("(" + vector.x.ToString("G9") + ", " + vector.y.ToString("G9") + ")");
			}
			else if (fieldType == typeof(Vector3))
			{
				Vector3 vector2 = (Vector3)value;
				sw.WriteLine("(" + vector2.x.ToString("G9") + ", " + vector2.y.ToString("G9") + ", " + vector2.z.ToString("G9") + ")");
			}
			else if (fieldType == typeof(Vector4))
			{
				Vector4 vector3 = (Vector4)value;
				sw.WriteLine("(" + vector3.x.ToString("G9") + ", " + vector3.y.ToString("G9") + ", " + vector3.z.ToString("G9") + ", " + vector3.w.ToString("G9") + ")");
			}
			else if (fieldType == typeof(Quaternion))
			{
				Quaternion quaternion = (Quaternion)value;
				sw.WriteLine("(" + quaternion.x.ToString("G9") + ", " + quaternion.y.ToString("G9") + ", " + quaternion.z.ToString("G9") + ", " + quaternion.w.ToString("G9") + ")");
			}
			else
			{
				sw.WriteLine(value);
			}
		}
	}

	public string GetStringValue(object host, bool gui)
	{
		Type fieldType = base.FieldInfo.FieldType;
		object value = GetValue(host);
		if (value == null)
		{
			return null;
		}
		if (fieldType.IsValueType)
		{
			if (fieldType == typeof(bool))
			{
				if (gui)
				{
					if ((bool)value)
					{
						return cacheAutoLOC_8004438;
					}
					return cacheAutoLOC_8004439;
				}
				return ((bool)value).ToString();
			}
			if (fieldType == typeof(int))
			{
				if (gui && _guiFormat != string.Empty)
				{
					return KSPUtil.LocalizeNumber((int)value, _guiFormat);
				}
				return ((int)value).ToString();
			}
			if (fieldType == typeof(float))
			{
				if (gui)
				{
					if (_guiFormat != string.Empty)
					{
						return KSPUtil.LocalizeNumber((float)value, _guiFormat);
					}
					return ((float)value).ToString();
				}
				return ((float)value).ToString("G9");
			}
			if (fieldType == typeof(double))
			{
				if (gui)
				{
					if (_guiFormat != string.Empty)
					{
						return KSPUtil.LocalizeNumber((double)value, _guiFormat);
					}
					return ((double)value).ToString();
				}
				return ((double)value).ToString("G17");
			}
			if (fieldType == typeof(Vector2))
			{
				if (gui)
				{
					if (_guiFormat != string.Empty)
					{
						return ((Vector2)value).ToString(_guiFormat);
					}
					Vector2 vector = (Vector2)value;
					return "(" + vector.x + ", " + vector.y + ")";
				}
				Vector2 vector2 = (Vector2)value;
				return "(" + vector2.x.ToString("G9") + ", " + vector2.y.ToString("G9") + ")";
			}
			if (fieldType == typeof(Vector3))
			{
				if (gui)
				{
					if (_guiFormat != string.Empty)
					{
						return ((Vector3)value).ToString(_guiFormat);
					}
					Vector3 vector3 = (Vector3)value;
					return "(" + vector3.x + ", " + vector3.y + ", " + vector3.z + ")";
				}
				Vector3 vector4 = (Vector3)value;
				return "(" + vector4.x.ToString("G9") + ", " + vector4.y.ToString("G9") + ", " + vector4.z.ToString("G9") + ")";
			}
			if (fieldType == typeof(Vector4))
			{
				if (gui)
				{
					if (_guiFormat != string.Empty)
					{
						return ((Vector4)value).ToString(_guiFormat);
					}
					Vector4 vector5 = (Vector4)value;
					return "(" + vector5.x + ", " + vector5.y + ", " + vector5.z + ", " + vector5.w + ")";
				}
				Vector4 vector6 = (Vector4)value;
				return "(" + vector6.x.ToString("G9") + ", " + vector6.y.ToString("G9") + ", " + vector6.z.ToString("G9") + ", " + vector6.w.ToString("G9") + ")";
			}
			if (fieldType == typeof(Quaternion))
			{
				if (gui)
				{
					if (_guiFormat != string.Empty)
					{
						return ((Quaternion)value).ToString(_guiFormat);
					}
					Quaternion quaternion = (Quaternion)value;
					return "(" + quaternion.x + ", " + quaternion.y + ", " + quaternion.z + ", " + quaternion.w + ")";
				}
				Quaternion quaternion2 = (Quaternion)value;
				return "(" + quaternion2.x.ToString("G9") + ", " + quaternion2.y.ToString("G9") + ", " + quaternion2.z.ToString("G9") + ", " + quaternion2.w.ToString("G9") + ")";
			}
			return value.ToString();
		}
		return (string)value;
	}

	public string GuiString(object host)
	{
		string text = "";
		text = ((!(base.guiName != string.Empty)) ? (text + base.name) : (text + base.guiName));
		return text + ": " + GetStringValue(host, gui: true) + guiUnits;
	}

	public static void ReadPvt(FieldInfo field, string value, object host)
	{
		Type fieldType = field.FieldType;
		if (fieldType.IsValueType)
		{
			if (fieldType == typeof(bool))
			{
				if (bool.TryParse(value, out var result))
				{
					SetPvt(field, result, host);
					return;
				}
				PDebug.Error("Invalid boolean value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(byte))
			{
				if (byte.TryParse(value, out var result2))
				{
					SetPvt(field, result2, host);
					return;
				}
				PDebug.Error("Invalid byte value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(ushort))
			{
				if (ushort.TryParse(value, out var result3))
				{
					SetPvt(field, result3, host);
					return;
				}
				PDebug.Error("Invalid ushort value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(char))
			{
				if (char.TryParse(value, out var result4))
				{
					SetPvt(field, result4, host);
					return;
				}
				PDebug.Error("Invalid char value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(ulong))
			{
				if (ulong.TryParse(value, out var result5))
				{
					SetPvt(field, result5, host);
					return;
				}
				PDebug.Error("Invalid ulong value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(int))
			{
				if (int.TryParse(value, out var result6))
				{
					SetPvt(field, result6, host);
					return;
				}
				PDebug.Error("Invalid integer value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(long))
			{
				if (long.TryParse(value, out var result7))
				{
					SetPvt(field, result7, host);
					return;
				}
				PDebug.Error("Invalid long value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(float))
			{
				if (float.TryParse(value, out var result8))
				{
					SetPvt(field, result8, host);
					return;
				}
				PDebug.Error("Invalid float value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(uint))
			{
				if (uint.TryParse(value, out var result9))
				{
					SetPvt(field, result9, host);
					return;
				}
				PDebug.Error("Invalid uint value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(short))
			{
				if (short.TryParse(value, out var result10))
				{
					SetPvt(field, result10, host);
					return;
				}
				PDebug.Error("Invalid short value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(double))
			{
				if (double.TryParse(value, out var result11))
				{
					SetPvt(field, result11, host);
					return;
				}
				PDebug.Error("Invalid double value! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(Vector2))
			{
				string[] array = value.Split(new char[4] { '(', ',', ' ', ')' }, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 2)
				{
					try
					{
						Vector2 vector = new Vector2(float.Parse(array[0]), float.Parse(array[1]));
						SetPvt(field, vector, host);
						return;
					}
					catch (Exception ex)
					{
						PDebug.Error(string.Concat("Cannot parse Vector2 value. Field ", field.Name, ", value ", value, " on object of type ", host.GetType(), " ", ex));
						return;
					}
				}
				PDebug.Error("Invalid value length for Vector2! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(Vector3))
			{
				string[] array2 = value.Split(new char[4] { '(', ',', ' ', ')' }, StringSplitOptions.RemoveEmptyEntries);
				if (array2.Length == 3)
				{
					try
					{
						Vector3 vector2 = new Vector3(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2]));
						SetPvt(field, vector2, host);
						return;
					}
					catch (Exception ex2)
					{
						PDebug.Error(string.Concat("Cannot parse Vector3 value. Field ", field.Name, ", value ", value, " on object of type ", host.GetType(), " ", ex2));
						return;
					}
				}
				PDebug.Error("Invalid value length for Vector3! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(Vector4))
			{
				string[] array3 = value.Split(new char[4] { '(', ',', ' ', ')' }, StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length == 4)
				{
					try
					{
						Vector4 vector3 = new Vector4(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]), float.Parse(array3[3]));
						SetPvt(field, vector3, host);
						return;
					}
					catch (Exception ex3)
					{
						PDebug.Error(string.Concat("Cannot parse Vector4 value. Field ", field.Name, ", value ", value, " on object of type ", host.GetType(), ": ", ex3));
						return;
					}
				}
				PDebug.Error("Invalid value length for Vector4! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType == typeof(Quaternion))
			{
				string[] array4 = value.Split(new char[4] { '(', ',', ' ', ')' }, StringSplitOptions.RemoveEmptyEntries);
				if (array4.Length == 4)
				{
					try
					{
						Quaternion quaternion = new Quaternion(float.Parse(array4[0]), float.Parse(array4[1]), float.Parse(array4[2]), float.Parse(array4[3]));
						SetPvt(field, quaternion, host);
						return;
					}
					catch (Exception ex4)
					{
						PDebug.Error(string.Concat("Cannot parse Quaternion value. Field ", field.Name, ", value ", value, " on object of type ", host.GetType(), ": ", ex4));
						return;
					}
				}
				PDebug.Error("Invalid value length for Quaternion! Field " + field.Name + ", value " + value + " on object of type " + host.GetType());
			}
			else if (fieldType.IsEnum)
			{
				try
				{
					object newValue = Enum.Parse(fieldType, value);
					SetPvt(field, newValue, host);
				}
				catch (Exception ex5)
				{
					PDebug.Error(string.Concat("Cannot parse Enum value. Field ", field.Name, ", value ", value, " on object of type ", host.GetType(), ": ", ex5));
				}
			}
		}
		else if (fieldType == typeof(string))
		{
			SetPvt(field, value, host);
		}
	}

	public static bool SetPvt(FieldInfo field, object newValue, object host)
	{
		try
		{
			field.SetValue(host, newValue);
			return true;
		}
		catch (Exception ex)
		{
			PDebug.Error(string.Concat("Value '", newValue, "' could not be set to field '", field.Name, "'"));
			PDebug.Error(ex.Message + "\n" + ex.StackTrace + "\n" + ex.Data);
			return false;
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_8004438 = Localizer.Format("#autoLOC_8004438");
		cacheAutoLOC_8004439 = Localizer.Format("#autoLOC_8004439");
	}
}
