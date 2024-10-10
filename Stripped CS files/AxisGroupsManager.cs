public class AxisGroupsManager
{
	public const string nodeName = "AXISGROUPS";

	public static void BuildBaseAxisFields(PartModule pm)
	{
		int count = pm.Fields.Count;
		while (count-- > 0)
		{
			BaseField baseField = pm.Fields[count];
			if (baseField.Attribute is KSPAxisField)
			{
				BaseAxisField value = new BaseAxisField(baseField.Attribute as KSPAxisField, baseField.FieldInfo, pm);
				pm.Fields[count] = value;
			}
		}
	}

	public static void LoadAxisFieldNodes(PartModule pm, ConfigNode node)
	{
		ConfigNode node2 = node.GetNode("AXISGROUPS");
		if (node2 == null)
		{
			return;
		}
		for (int i = 0; i < node2.nodes.Count; i++)
		{
			ConfigNode configNode = node2.nodes[i];
			if (pm.Fields[configNode.name] is BaseAxisField baseAxisField)
			{
				baseAxisField.OnLoad(configNode);
			}
		}
	}

	public static void SaveAxisFieldNodes(PartModule pm, ConfigNode node)
	{
		bool flag = false;
		int count = pm.Fields.Count;
		while (count-- > 0)
		{
			if (pm.Fields[count] is BaseAxisField)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		ConfigNode configNode = node.AddNode("AXISGROUPS");
		for (int i = 0; i < pm.Fields.Count; i++)
		{
			if (pm.Fields[i] is BaseAxisField baseAxisField)
			{
				baseAxisField.OnSave(configNode.AddNode(baseAxisField.name));
			}
		}
	}
}
