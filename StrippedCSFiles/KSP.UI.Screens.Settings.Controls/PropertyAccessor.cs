using System.Reflection;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings.Controls;

public class PropertyAccessor : AccessorBase
{
	protected object[] indexParam;

	public override object Value
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PropertyAccessor(object obj, MemberInfo member, object[] indexParam)
	{
		throw null;
	}
}
