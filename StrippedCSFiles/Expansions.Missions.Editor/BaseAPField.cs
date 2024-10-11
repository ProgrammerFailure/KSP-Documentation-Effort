using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class BaseAPField : BaseField<MEGUI_Control>
{
	[SerializeField]
	private bool _gapDisplay;

	[SerializeField]
	private string _group;

	[SerializeField]
	private string _groupDisplayName;

	private MethodInfo _onValueChange;

	private MethodInfo _onControlCreated;

	private MethodInfo _onControlSetupComplete;

	private MethodInfo _compareValuesForCheckpoint;

	private PropertyInfo _propertyInfo;

	private bool _hideWhenSiblingsExist;

	private bool _hideWhenStartNode;

	private bool _hideWhenDocked;

	private bool _hideWhenInputConnected;

	private bool _hideWhenOutputConnected;

	private bool _hideWhenNoTestModules;

	private bool _hideWhenNoActionModules;

	private bool _hideOnSetup;

	[SerializeField]
	private int _order;

	private bool _tabStop;

	[SerializeField]
	private bool _groupStartCollapsed;

	public bool gapDisplay
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

	public MethodInfo OnValueChange
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MethodInfo OnControlCreated
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MethodInfo OnControlSetupComplete
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MethodInfo CompareValuesForCheckpoint
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PropertyInfo PropertyInfo
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

	public string FieldID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Type FieldType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Group
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

	public string GroupDisplayName
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

	public bool HideWhenSiblingsExist
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

	public bool HideWhenStartNode
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

	public bool HideWhenDocked
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

	public bool HideWhenInputConnected
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

	public bool HideWhenOutputConnected
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

	public bool HideWhenNoTestModules
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

	public bool HideWhenNoActionModules
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

	public bool HideOnSetup
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

	public int Order
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

	public bool TabStop
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

	public bool GroupStartCollapsed
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
	public BaseAPField(MEGUI_Control fieldAttrib, MemberInfo fieldInfo, object host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(object newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object GetValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T GetValue<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetOriginalValue()
	{
		throw null;
	}
}
