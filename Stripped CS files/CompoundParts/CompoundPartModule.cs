using UnityEngine;

namespace CompoundParts;

public abstract class CompoundPartModule : PartModule
{
	public CompoundPart _compoundPart;

	public CompoundPart compoundPart
	{
		get
		{
			return _compoundPart;
		}
		set
		{
			_compoundPart = value;
		}
	}

	public Part target => _compoundPart.target;

	public CompoundPartModule()
	{
	}

	public sealed override void OnAwake()
	{
		_compoundPart = base.part as CompoundPart;
		if (_compoundPart == null)
		{
			Debug.LogError("[CompoundPartModule]: CompoundPartModule requires a CompoundPart component!", base.gameObject);
		}
		OnModuleAwake();
	}

	public virtual void OnModuleAwake()
	{
	}

	public abstract void OnTargetSet(Part target);

	public abstract void OnTargetLost();

	public virtual void OnPreviewAttachment(Vector3 rDir, Vector3 rPos, Quaternion rRot)
	{
	}

	public virtual void OnPreviewEnd()
	{
	}

	public virtual void OnTargetUpdate()
	{
	}
}
