using System;
using UnityEngine;

public class Timing5 : MonoBehaviour
{
	public TimingManager.UpdateAction onUpdate { get; set; }

	public TimingManager.UpdateAction onFixedUpdate { get; set; }

	public TimingManager.UpdateAction onLateUpdate { get; set; }

	public virtual void Update()
	{
		try
		{
			if (onUpdate != null)
			{
				onUpdate();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Timing5 threw during Update: " + ex);
		}
	}

	public virtual void FixedUpdate()
	{
		try
		{
			if (onFixedUpdate != null)
			{
				onFixedUpdate();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Timing5 threw during FixedUpdate: " + ex);
		}
	}

	public virtual void LateUpdate()
	{
		try
		{
			if (onLateUpdate != null)
			{
				onLateUpdate();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Timing5 threw during LateUpdate: " + ex);
		}
	}
}
