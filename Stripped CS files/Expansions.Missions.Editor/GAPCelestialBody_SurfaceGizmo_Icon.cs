using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBody_SurfaceGizmo_Icon : GAPCelestialBody_SurfaceGizmo
{
	[SerializeField]
	public Projector projectorArea;

	public double Radius
	{
		set
		{
			projectorArea.orthographicSize = (float)value;
		}
	}

	public override void Initialize(GAPCelestialBody newGapRef)
	{
		base.Initialize(newGapRef);
		projectorArea.farClipPlane = (float)newGapRef.CelestialBody.Radius / 2f;
	}
}
