using System;
using UnityEngine;

public class RDPartCollisionHandler : MonoBehaviour
{
	public KerbalEVA eva;

	public void Start()
	{
		if (!eva)
		{
			throw new Exception("[Ragdoll Error]: Ragdoll Part Collision Handler's EVA reference is null");
		}
	}

	public void OnCollisionEnter(Collision c)
	{
		if ((bool)eva)
		{
			eva.OnCollisionEnter(c);
		}
	}

	public void OnCollisionStay(Collision c)
	{
		if ((bool)eva)
		{
			eva.OnCollisionStay(c);
		}
	}

	public void OnCollisionExit(Collision c)
	{
		if ((bool)eva)
		{
			eva.OnCollisionExit(c);
		}
	}
}
