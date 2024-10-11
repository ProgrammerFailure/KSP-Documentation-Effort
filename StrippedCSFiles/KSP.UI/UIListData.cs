using System.Runtime.CompilerServices;

namespace KSP.UI;

public class UIListData<T>
{
	public T data;

	public UIListItem listItem;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListData(T data, UIListItem listItem)
	{
		throw null;
	}
}
