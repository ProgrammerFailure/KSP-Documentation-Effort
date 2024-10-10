using System.Reflection;

namespace ns21;

public class FieldAccessor : AccessorBase
{
	public FieldInfo field => member as FieldInfo;

	public override object Value
	{
		get
		{
			return ((FieldInfo)member).GetValue(obj);
		}
		set
		{
			((FieldInfo)member).SetValue(obj, value);
		}
	}

	public FieldAccessor(object obj, MemberInfo member)
		: base(obj, member)
	{
	}
}
