using UnityEngine;

namespace SaveUpgradePipeline;

public abstract class PartOffset : UpgradeScript
{
	public string partName;

	public Vector3 positionOffset;

	public Quaternion rotationOffset;

	public Quaternion rotation0 = Quaternion.identity;

	public Quaternion rotation;

	public Vector3 position0;

	public Vector3 position;

	public PartOffset()
	{
	}

	public override void OnInit()
	{
		Setup(out partName, out positionOffset, out rotationOffset);
	}

	public abstract void Setup(out string pName, out Vector3 posOffset, out Quaternion rotOffset);

	public override TestResult OnTest(ConfigNode node, LoadContext loadContext, ref string nodeName)
	{
		nodeName = NodeUtil.GetPartNodeNameValue(node, loadContext);
		if (loadContext == LoadContext.Craft)
		{
			nodeName = nodeName.Split('_')[0];
		}
		if (nodeName == partName)
		{
			return TestResult.Upgradeable;
		}
		return TestResult.Pass;
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		string name;
		string name2;
		if (loadContext != LoadContext.flag_1 && loadContext == LoadContext.Craft)
		{
			name = "rot";
			name2 = "pos";
		}
		else
		{
			name = "rotation";
			name2 = "position";
		}
		if (node.HasValue(name))
		{
			rotation0 = KSPUtil.ParseQuaternion(node.GetValue(name));
			rotation = rotation0 * rotationOffset;
			node.SetValue(name, KSPUtil.WriteQuaternion(rotation));
		}
		if (node.HasValue(name2))
		{
			position0 = KSPUtil.ParseVector3(node.GetValue(name2));
			Vector3 vector = position0 + rotation0 * positionOffset;
			node.SetValue(name2, KSPUtil.WriteVector(vector));
		}
	}
}
