using UnityEngine;

public class LoadingSystem : MonoBehaviour
{
	public virtual bool IsReady()
	{
		return false;
	}

	public virtual string ProgressTitle()
	{
		return "None";
	}

	public virtual float ProgressFraction()
	{
		return 0f;
	}

	public virtual void StartLoad()
	{
	}

	public virtual float LoadWeight()
	{
		return 1f;
	}
}
