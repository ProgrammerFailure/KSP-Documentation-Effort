using System.Collections.Generic;

public abstract class PartModuleFXSetter : PartModule
{
	[KSPField]
	public string fxModuleNames = string.Empty;

	public List<IScalarModule> fxModules;

	public int modulesCount;

	public PartModuleFXSetter()
	{
	}

	public override void OnAwake()
	{
		base.OnAwake();
		if (fxModules == null)
		{
			fxModules = new List<IScalarModule>();
		}
	}

	public override void OnStart(StartState state)
	{
		if (string.IsNullOrEmpty(fxModuleNames))
		{
			return;
		}
		fxModules.Clear();
		string[] array = fxModuleNames.Split(',');
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			IScalarModule scalarModule = base.part.Modules.GetScalarModule(array[i].Trim());
			if (scalarModule != null)
			{
				fxModules.Add(scalarModule);
			}
		}
		modulesCount = fxModules.Count;
	}

	public virtual void SetFXModules(float scalar)
	{
		for (int i = 0; i < modulesCount; i++)
		{
			fxModules[i].SetScalar(scalar);
		}
	}
}
