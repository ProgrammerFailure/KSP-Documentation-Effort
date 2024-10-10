using System;

namespace ns15;

[Serializable]
public class DictionarySerializerHelper
{
	public object Key;

	public object Value;

	public DictionarySerializerHelper(object key, object value)
	{
		Key = key;
		Value = value;
	}

	public DictionarySerializerHelper()
	{
	}
}
