using System.Collections;
using UnityEngine;

public abstract class CurrencyWidget : MonoBehaviour
{
	public int initDelay = 20;

	public CurrencyWidget()
	{
	}

	public IEnumerator Start()
	{
		if (OnAboutToStart())
		{
			yield return null;
			for (int i = 1; i < initDelay; i++)
			{
				yield return null;
			}
			DelayedStart();
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public abstract bool OnAboutToStart();

	public abstract void DelayedStart();
}
