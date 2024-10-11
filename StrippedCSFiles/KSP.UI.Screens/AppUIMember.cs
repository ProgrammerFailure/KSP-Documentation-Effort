using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public abstract class AppUIMember : MonoBehaviour
{
	protected AppUI_Data _host;

	protected AppUI_Control _attribs;

	protected FieldInfo _field;

	protected PropertyInfo _property;

	protected MemberInfo _member;

	protected bool isEnum;

	protected Type valueType;

	public TextMeshProUGUI guiNameLabel;

	protected RectTransform guiNameLabelRectTransform;

	[SerializeField]
	protected RectTransform valueHolderRectTransform;

	protected Vector2 valueRectOffsetMin;

	protected AppUIInputPanel parentPanel;

	protected VerticalLayoutGroup parentLayoutGroup;

	[SerializeField]
	protected List<UIHoverText> hoverTextList;

	[SerializeField]
	private string _hoverText;

	private AppUIMemberLabelList labelList;

	public bool showGuiName;

	[SerializeField]
	private string _guiName;

	public AppUI_Control.HorizontalAlignment guiNameHorizAlignment;

	public AppUI_Control.VerticalAlignment guiNameVertAlignment;

	public int order;

	public bool hideOnError;

	public string HoverText
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

	public string guiName
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

	public bool IsInitialized
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

	public string Name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected AppUIMember()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnInitialized()
	{
		throw null;
	}

	protected abstract void OnRefreshUI();

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal virtual bool AnyTextFieldHasFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(AppUI_Data host, MemberInfo member, AppUI_Control attribs, AppUIInputPanel parentPanel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHoverTextTarget(TextMeshProUGUI textTargetForHover)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool SetValue(object newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected T GetValue<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected object GetValue()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int SortByOrder(AppUIMember m1, AppUIMember m2)
	{
		throw null;
	}
}
