namespace DDSHeaders;

public static class DDSValues
{
	public static uint uintMagic = StringToValue("DDS ");

	public static uint uintDXT1 = StringToValue("DXT1");

	public static uint uintDXT2 = StringToValue("DXT2");

	public static uint uintDXT3 = StringToValue("DXT3");

	public static uint uintDXT4 = StringToValue("DXT4");

	public static uint uintDXT5 = StringToValue("DXT5");

	public static uint uintDX10 = StringToValue("DX10");

	public static uint StringToValue(string str)
	{
		char[] array = str.ToCharArray(0, 4);
		uint num = 0u;
		for (int i = 0; i < 4; i++)
		{
			num += (uint)array[i] << i * 8;
		}
		return num;
	}
}
