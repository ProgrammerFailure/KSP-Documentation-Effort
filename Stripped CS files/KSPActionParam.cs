using UnityEngine;

public class KSPActionParam
{
	public float _cooldown;

	public KSPActionGroup group { get; set; }

	public KSPActionType type { get; set; }

	public float Cooldown
	{
		get
		{
			return _cooldown;
		}
		set
		{
			_cooldown = Mathf.Max(_cooldown, value);
		}
	}

	public KSPActionParam(KSPActionGroup actionGroup, KSPActionType actionType)
	{
		group = actionGroup;
		type = actionType;
		_cooldown = 0f;
	}
}
