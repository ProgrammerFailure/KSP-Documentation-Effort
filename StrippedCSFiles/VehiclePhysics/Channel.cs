using System.Runtime.InteropServices;

namespace VehiclePhysics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Channel
{
	public const int Input = 0;

	public const int Vehicle = 1;

	public const int Settings = 2;
}
