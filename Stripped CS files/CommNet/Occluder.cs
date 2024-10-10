namespace CommNet;

public abstract class Occluder
{
	public double radius;

	public Vector3d position;

	public Occluder()
	{
	}

	public virtual bool InRange(Vector3d source, double distance)
	{
		double num = distance + radius;
		num *= num;
		return (source - position).sqrMagnitude <= num;
	}

	public abstract bool Raycast(Vector3d source, Vector3d dest);

	public virtual void Update()
	{
	}
}
