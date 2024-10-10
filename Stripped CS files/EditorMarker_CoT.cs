using UnityEngine;

public class EditorMarker_CoT : EditorMarker
{
	public static Ray CoT;

	public static float t;

	public static CenterOfThrustQuery tQry = new CenterOfThrustQuery();

	public static Vector3 Pos => CoT.origin;

	public static Vector3 Dir => CoT.direction;

	public void Awake()
	{
		if (rootPart == null && ((HighLogic.LoadedSceneIsEditor && (bool)EditorLogic.fetch) || (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)))
		{
			rootPart = (HighLogic.LoadedSceneIsEditor ? EditorLogic.RootPart : FlightGlobals.ActiveVessel.rootPart);
		}
	}

	public override Vector3 UpdatePosition()
	{
		if ((HighLogic.LoadedSceneIsEditor && EditorLogic.fetch == null) || (HighLogic.LoadedSceneIsFlight && (rootPart == null || rootPart.vessel == null)))
		{
			return Vector3.zero;
		}
		CoT = FindCoT(rootPart);
		if ((bool)dirMarkerObject && CoT.direction != Vector3.zero)
		{
			dirMarkerObject.transform.forward = CoT.direction;
		}
		return CoT.origin;
	}

	public static Ray FindCoT(Part rootPart)
	{
		t = 0f;
		Vector3 origin = Vector3.zero;
		Vector3 direction = Vector3.zero;
		if (rootPart != null)
		{
			recurseParts(rootPart, ref origin, ref direction, ref t);
		}
		if (HighLogic.LoadedSceneIsEditor && (bool)EditorLogic.SelectedPart && !EditorLogic.fetch.ship.Contains(EditorLogic.SelectedPart) && (bool)EditorLogic.SelectedPart.potentialParent)
		{
			recurseParts(EditorLogic.SelectedPart, ref origin, ref direction, ref t);
			for (int i = 0; i < EditorLogic.SelectedPart.symmetryCounterparts.Count; i++)
			{
				recurseParts(EditorLogic.SelectedPart.symmetryCounterparts[i], ref origin, ref direction, ref t);
			}
		}
		if (t != 0f)
		{
			float num = 1f / t;
			origin *= num;
			direction *= num;
			return new Ray(origin, direction);
		}
		return new Ray(Vector3.zero, Vector3.zero);
	}

	public static Ray FindCoT()
	{
		Part part = null;
		if ((HighLogic.LoadedSceneIsEditor && (bool)EditorLogic.fetch) || (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null))
		{
			part = (HighLogic.LoadedSceneIsEditor ? EditorLogic.RootPart : FlightGlobals.ActiveVessel.rootPart);
		}
		return FindCoT(part);
	}

	public static void recurseParts(Part part, ref Vector3 origin, ref Vector3 direction, ref float t)
	{
		int count = part.Modules.Count;
		while (count-- > 0)
		{
			if (part.Modules[count] is IThrustProvider thrustProvider)
			{
				tQry.Reset();
				thrustProvider.OnCenterOfThrustQuery(tQry);
				origin += tQry.pos * tQry.thrust;
				direction += tQry.dir * tQry.thrust;
				t += tQry.thrust;
			}
		}
		for (int i = 0; i < part.children.Count; i++)
		{
			recurseParts(part.children[i], ref origin, ref direction, ref t);
		}
	}
}
