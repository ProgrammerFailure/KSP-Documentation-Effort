using UnityEngine;

public class EditorMarker_CoM : EditorMarker
{
	public static Vector3 CraftCoM;

	public void Awake()
	{
		if (rootPart == null)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				rootPart = EditorLogic.RootPart;
			}
			else if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.rootPart != null)
			{
				rootPart = FlightGlobals.ActiveVessel.rootPart;
			}
		}
	}

	public override Vector3 UpdatePosition()
	{
		if (rootPart != null)
		{
			CraftCoM = findCenterOfMass(rootPart);
		}
		return CraftCoM;
	}

	public static Vector3 findCenterOfMass(Part root)
	{
		if (HighLogic.LoadedSceneIsEditor && EditorLogic.fetch == null)
		{
			return Vector3.zero;
		}
		Vector3 CoM = Vector3.zero;
		float m = 0f;
		recurseParts(root, ref CoM, ref m);
		if (HighLogic.LoadedSceneIsEditor && (bool)EditorLogic.SelectedPart && !EditorLogic.fetch.ship.Contains(EditorLogic.SelectedPart) && (bool)EditorLogic.SelectedPart.potentialParent)
		{
			recurseParts(EditorLogic.SelectedPart, ref CoM, ref m);
			for (int i = 0; i < EditorLogic.SelectedPart.symmetryCounterparts.Count; i++)
			{
				recurseParts(EditorLogic.SelectedPart.symmetryCounterparts[i], ref CoM, ref m);
			}
		}
		return CoM / m;
	}

	public static void recurseParts(Part part, ref Vector3 CoM, ref float m)
	{
		if (part.physicalSignificance == Part.PhysicalSignificance.FULL)
		{
			CoM += (part.transform.position + part.transform.rotation * part.CoMOffset) * (part.mass + part.GetResourceMass());
			m += part.mass + part.GetResourceMass();
		}
		else if (part.parent != null)
		{
			CoM += (part.parent.transform.position + part.parent.transform.rotation * part.parent.CoMOffset) * (part.mass + part.GetResourceMass());
			m += part.mass + part.GetResourceMass();
		}
		else if (part.potentialParent != null)
		{
			CoM += (part.potentialParent.transform.position + part.potentialParent.transform.rotation * part.potentialParent.CoMOffset) * (part.mass + part.GetResourceMass());
			m += part.mass + part.GetResourceMass();
		}
		for (int i = 0; i < part.children.Count; i++)
		{
			recurseParts(part.children[i], ref CoM, ref m);
		}
	}
}
