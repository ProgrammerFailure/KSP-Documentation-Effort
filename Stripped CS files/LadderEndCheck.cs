using UnityEngine;

public class LadderEndCheck : MonoBehaviour
{
	public bool reached;

	[SerializeField]
	public KerbalEVA kerbalEVA;

	public bool Reached
	{
		get
		{
			return reached;
		}
		set
		{
			reached = value;
		}
	}

	public virtual void OnTriggerEnter(Collider o)
	{
		if (o.CompareTag("Ladder"))
		{
			reached = false;
		}
	}

	public virtual void OnTriggerStay(Collider o)
	{
		if (kerbalEVA.OnALadder && o.CompareTag("Ladder"))
		{
			reached = false;
		}
	}

	public virtual void OnTriggerExit(Collider o)
	{
		if (o.CompareTag("Ladder"))
		{
			reached = true;
		}
	}
}
