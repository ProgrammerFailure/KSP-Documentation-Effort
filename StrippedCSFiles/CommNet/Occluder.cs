using System.Runtime.CompilerServices;

namespace CommNet;

public abstract class Occluder
{
	public double radius;

	public Vector3d position;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Occluder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool InRange(Vector3d source, double distance)
	{
		throw null;
	}

	public abstract bool Raycast(Vector3d source, Vector3d dest);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update()
	{
		throw null;
	}
}
