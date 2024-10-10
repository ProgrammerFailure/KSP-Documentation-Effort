public class PQSSurfaceObject : PQSMod
{
	public virtual string SurfaceObjectName => string.Empty;

	public virtual string DisplaySurfaceObjectName => string.Empty;

	public virtual Vector3d PlanetRelativePosition => base.transform.localPosition;
}
