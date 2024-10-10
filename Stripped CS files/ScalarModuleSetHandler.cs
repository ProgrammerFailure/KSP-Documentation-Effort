using System.Collections.Generic;

public class ScalarModuleSetHandler : IConfigNode
{
	public string nodeName = "scalarModules";

	public string[] fxModuleNames;

	public List<IScalarModule> fxModules = new List<IScalarModule>();

	public ScalarModuleSetHandler()
	{
	}

	public ScalarModuleSetHandler(string configValueName)
	{
		nodeName = configValueName;
	}

	public virtual void Load(ConfigNode node)
	{
		string[] values = node.GetValues(nodeName);
		if (values.Length != 0)
		{
			fxModuleNames = values;
		}
	}

	public virtual void Save(ConfigNode node)
	{
		int i = 0;
		for (int num = fxModuleNames.Length; i < num; i++)
		{
			node.AddValue("moduleID", fxModuleNames[i]);
		}
	}

	public virtual void Initialize(Part part)
	{
		fxModules.Clear();
		int i = 0;
		for (int num = fxModuleNames.Length; i < num; i++)
		{
			fxModules.AddRange(part.Modules.GetScalarModules(fxModuleNames[i].Trim()));
		}
	}

	public virtual void SetFXModules(float scalar)
	{
		for (int i = 0; i < fxModules.Count; i++)
		{
			fxModules[i].SetScalar(scalar);
		}
	}
}
