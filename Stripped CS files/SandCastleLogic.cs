using System;
using UnityEngine;

public class SandCastleLogic : MonoBehaviour
{
	public void Start()
	{
		UnityEngine.Random.InitState(Environment.TickCount ^ Guid.NewGuid().GetHashCode());
		if (UnityEngine.Random.Range(0, 40) != 0)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
