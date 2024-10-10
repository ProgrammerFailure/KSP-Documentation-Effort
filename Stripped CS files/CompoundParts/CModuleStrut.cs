using ns9;
using UnityEngine;

namespace CompoundParts;

public class CModuleStrut : CompoundPartModule, IModuleInfo
{
	[KSPField]
	public float linearStrength = 22f;

	[KSPField]
	public float angularStrength = 22f;

	public PartJoint strutJoint;

	public Part jointRoot;

	public Part jointTarget;

	public AttachNode targetNode;

	public static string cacheAutoLOC_217004;

	public override void OnTargetSet(Part target)
	{
		if (!(base.part.Rigidbody == null) && HighLogic.LoadedSceneIsFlight)
		{
			jointTarget = target.Rigidbody.GetComponent<Part>();
			jointRoot = base.part.Rigidbody.GetComponent<Part>();
			if (jointTarget != jointRoot)
			{
				targetNode = new AttachNode();
				targetNode.id = "Strut";
				targetNode.attachedPart = jointTarget;
				targetNode.srfAttachMeshName = base.compoundPart.targetMeshColName;
				targetNode.nodeType = AttachNode.NodeType.Surface;
				targetNode.attachMethod = AttachNodeMethod.FIXED_JOINT;
				targetNode.breakingForce = linearStrength;
				targetNode.breakingTorque = angularStrength;
				Vector3 vector = base.compoundPart.transform.TransformPoint(base.compoundPart.targetPosition);
				targetNode.position = jointTarget.partTransform.InverseTransformPoint(vector);
				targetNode.orientation = jointTarget.partTransform.InverseTransformDirection((base.transform.position - vector).normalized);
				targetNode.size = 1;
				targetNode.ResourceXFeed = false;
				targetNode.owner = jointRoot;
				strutJoint = PartJoint.Create(jointRoot, jointTarget, targetNode, null, AttachModes.SRF_ATTACH);
			}
			else
			{
				Debug.LogWarning("[StrutConnector]: Both sides of the strut share the same parent rigidbody. No actual joint was created.", base.gameObject);
				strutJoint = null;
				targetNode = null;
			}
		}
	}

	public override void OnTargetLost()
	{
		if ((bool)strutJoint)
		{
			strutJoint.DestroyJoint();
			if (targetNode != null)
			{
				targetNode.owner = null;
			}
		}
	}

	public override void OnStartFinished(StartState state)
	{
		base.OnStartFinished(state);
		if (HighLogic.LoadedSceneIsFlight && EVAConstructionModeController.Instance.IsOpen)
		{
			base.compoundPart.attachState = CompoundPart.AttachState.Attaching;
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_216749", base.compoundPart.maxLength.ToString("0.0")) + Localizer.Format("#autoLOC_216999", linearStrength.ToString(), angularStrength.ToString());
	}

	public string GetModuleTitle()
	{
		return "Strut Connector";
	}

	public Callback<Rect> GetDrawModulePanelCallback()
	{
		return null;
	}

	public string GetPrimaryField()
	{
		return "";
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_217004;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_217004 = Localizer.Format("#autoLOC_217004");
	}
}
