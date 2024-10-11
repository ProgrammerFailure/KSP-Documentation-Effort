using System;
using System.Runtime.CompilerServices;

namespace KSP.UI.Language;

[Serializable]
public class DictionarySerializerHelper
{
	public object Key;

	public object Value;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DictionarySerializerHelper(object key, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DictionarySerializerHelper()
	{
		throw null;
	}
}
