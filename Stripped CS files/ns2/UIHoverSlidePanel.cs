using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns2;

public class UIHoverSlidePanel : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public RectTransform panel;

	public Vector2 positionNormal;

	public Vector2 positionHovered;

	public float sharpness = 1f;

	public bool locked;

	public bool pointOver;

	public Coroutine coroutine;

	public Callback<Vector2> OnUpdatePosition = delegate
	{
	};

	public List<GameObject> childrenForDeactivate = new List<GameObject>();

	public void Reset()
	{
		panel = GetComponent<RectTransform>();
	}

	public void Start()
	{
		pointOver = false;
		panel.anchoredPosition = positionNormal;
		OnUpdatePosition(panel.anchoredPosition);
		for (int num = childrenForDeactivate.Count - 1; num >= 0; num--)
		{
			if (childrenForDeactivate[num] == null)
			{
				childrenForDeactivate.RemoveAt(num);
			}
		}
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		pointOver = true;
		if (!locked && coroutine == null)
		{
			coroutine = StartCoroutine(MoveToState(0f, newState: true));
		}
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		pointOver = false;
		if (!locked && coroutine == null)
		{
			coroutine = StartCoroutine(MoveToState(0.1f, newState: false));
		}
	}

	public IEnumerator MoveToState(float delay, bool newState)
	{
		bool state = newState;
		Vector2 vTgt = ((!newState) ? positionNormal : positionHovered);
		yield return new WaitForSeconds(delay);
		if (!locked && state != pointOver)
		{
			coroutine = null;
			yield break;
		}
		if (newState)
		{
			for (int i = 0; i < childrenForDeactivate.Count; i++)
			{
				if (childrenForDeactivate[i] != null && !childrenForDeactivate[i].activeSelf)
				{
					childrenForDeactivate[i].SetActive(value: true);
				}
			}
		}
		while ((panel.anchoredPosition - vTgt).sqrMagnitude > 0.0001f)
		{
			panel.anchoredPosition = Vector2.Lerp(panel.anchoredPosition, vTgt, sharpness * Time.deltaTime);
			OnUpdatePosition(panel.anchoredPosition);
			yield return null;
		}
		if (!newState)
		{
			for (int j = 0; j < childrenForDeactivate.Count; j++)
			{
				if (childrenForDeactivate[j] != null && childrenForDeactivate[j].activeSelf)
				{
					childrenForDeactivate[j].SetActive(value: false);
				}
			}
		}
		panel.anchoredPosition = vTgt;
		OnUpdatePosition(panel.anchoredPosition);
		if (!locked && state != pointOver)
		{
			coroutine = StartCoroutine(MoveToState(0.5f, pointOver));
		}
		else
		{
			coroutine = null;
		}
	}
}
