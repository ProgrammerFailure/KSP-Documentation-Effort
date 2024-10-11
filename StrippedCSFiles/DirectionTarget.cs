using System.Runtime.CompilerServices;

public class DirectionTarget : PositionTarget
{
	public Vector3d direction;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DirectionTarget(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DirectionTarget(string name, string displayName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new void Update(Vector3d direction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update(Vessel v, double pitch, double heading, bool degrees = false)
	{
		throw null;
	}
}
