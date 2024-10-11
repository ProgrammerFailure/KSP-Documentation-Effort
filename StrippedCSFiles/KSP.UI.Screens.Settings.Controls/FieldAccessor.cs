using System.Reflection;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings.Controls;

public class FieldAccessor : AccessorBase
{
	public FieldInfo field
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

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
	public FieldAccessor(object obj, MemberInfo member)
	{
		throw null;
	}
}
