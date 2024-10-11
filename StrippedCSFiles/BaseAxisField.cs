using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Expansions.Serenity;

public class BaseAxisField : BaseField
{
	public static KSPAxisGroup MaxAxisGroup;

	public float minValue;

	public float maxValue;

	public float incrementalSpeed;

	public float incrementalSpeedMultiplier;

	public bool ignoreClampWhenIncremental;

	public bool ignoreIncrementByZero;

	public bool active;

	public KSPAxisGroup defaultAxisGroup;

	public KSPAxisGroup axisGroup;

	public KSPAxisGroup axisIncremental;

	public KSPAxisGroup axisInverted;

	public KSPAxisGroup[] overrideGroups;

	public KSPAxisGroup[] overrideIncremental;

	public KSPAxisGroup[] overrideInverted;

	private static int axisGroupsLength;

	public PartModule module
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int AxisGroupsLength
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public event Callback OnAxisSpeedChanged
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		add
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		remove
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseAxisField(KSPAxisField fieldAttrib, FieldInfo fieldInfo, object host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static BaseAxisField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAxisGroup GetAxisGroup(int groupOverride)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void CopyField(BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsInGroup(KSPAxisGroup group, int overrideGroup, bool overrideDefault, bool include)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsNonDefaultAxes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private KSPAxisGroup ParseGroup(string groupName, KSPAxisGroup defaultGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAxis(float axisValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void IncrementAxis(float axisRate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static BaseAxisFieldList CreateAxisList(List<Part> parts, KSPAxisGroup group, int overrideGroup, bool overrideDefault, bool include)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddAxis(BaseAxisFieldList axisFieldList, KSPAxisGroup group, int overrideGroup, bool overrideDefault, bool include, BaseAxisField axisField)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static BaseAxisFieldList CreateAxisList(Part part, KSPAxisGroup group, int overrideGroup, bool overrideDefault, bool include)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetAxisGroupsLength(float facilityLevel, bool isVAB = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string[] GetAxisGroups(int maxlevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<bool> CreateGroupList(List<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<bool> CreateGroupList(List<Part> parts, int groupOverride)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ContainsNonDefaultAxes(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateAxis(KSPAxisGroup group, int groupOverride, float axisValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIncrementalSpeedMultiplier(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static BaseAxisFieldList CreateAxisList(List<Part> parts, ModuleRoboticController controller, bool include)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static BaseAxisFieldList CreateAxisList(Part part, ModuleRoboticController controller, bool include)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddAxis(BaseAxisFieldList axisFieldList, ModuleRoboticController controller, Part part, bool include, BaseAxisField axisField)
	{
		throw null;
	}
}
