using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ns8;

[Obsolete("Methods moved to IOUtils")]
public class IOTools
{
	public static byte[] SerializeToBinary(object something)
	{
		using System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
		new BinaryFormatter().Serialize(memoryStream, something);
		return memoryStream.ToArray();
	}

	public static object DeserializeFromBinary(byte[] input)
	{
		return new BinaryFormatter().Deserialize(new System.IO.MemoryStream(input));
	}
}
