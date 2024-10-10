using System.Collections.Generic;
using UnityEngine;

public class FXObject
{
	public GameObject effectObj;

	public List<ParticleSystem> systems = new List<ParticleSystem>();

	public AudioClip effectSound;

	public FXObject()
	{
	}

	public FXObject(GameObject obj)
	{
		effectObj = obj;
		obj.GetComponentsInChildren(systems);
	}
}
