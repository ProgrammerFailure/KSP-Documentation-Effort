using UnityEngine;

public class InternalModule : MonoBehaviour
{
	[SerializeField]
	[HideInInspector]
	public string className;

	[HideInInspector]
	[SerializeField]
	public int classID;

	[SerializeField]
	[HideInInspector]
	public BaseEventList events;

	[SerializeField]
	[HideInInspector]
	public BaseFieldList fields;

	public int moduleID;

	public InternalProp internalProp;

	public string ClassName => className;

	public int ClassID => classID;

	public BaseEventList Events => events;

	public BaseFieldList Fields => fields;

	public InternalModel internalModel => internalProp.internalModel;

	public Part part => internalModel.part;

	public Vessel vessel => part.vessel;

	public void ModularSetup()
	{
		className = GetType().Name;
		classID = className.GetHashCode();
		events = new BaseEventList(this);
		fields = new BaseFieldList(this);
	}

	public void Awake()
	{
		if (Fields == null)
		{
			ModularSetup();
		}
	}

	public void Load(ConfigNode node)
	{
		if (Fields == null)
		{
			ModularSetup();
		}
		Fields.Load(node);
		OnLoad(node);
		OnAwake();
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", ClassName);
		Fields.Save(node);
		OnSave(node);
	}

	public virtual void OnAwake()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual void OnFixedUpdate()
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}
}
