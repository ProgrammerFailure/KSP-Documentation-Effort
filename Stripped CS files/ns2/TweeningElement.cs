using System;
using System.Collections;
using UnityEngine;

namespace ns2;

public class TweeningElement : MonoBehaviour
{
	[HideInInspector]
	public object data;

	[HideInInspector]
	public int dataInt;

	public RectTransform tweeningElement;

	public Vector3 startPos;

	public Vector3 endPos;

	public RectTransform endPosByElement;

	public bool endAtElement;

	public float duration;

	public TweeningController.TweeningFunction tweeningFunction;

	public Callback onComplete = delegate
	{
	};

	public void Awake()
	{
	}

	public void StartTweening(RectTransform tweeningElement, Vector3 startPos, Vector3 endPos, float duration, TweeningController.TweeningFunction tweeningFunction, Callback onComplete)
	{
		tweeningElement.transform.SetParent(TweeningController.Instance.tweeningPlane);
		tweeningElement.transform.localPosition = new Vector3(tweeningElement.transform.localPosition.x, tweeningElement.transform.localPosition.y, 0f);
		this.tweeningElement = tweeningElement;
		this.startPos = new Vector3(startPos.x, startPos.y, TweeningController.Instance.tweeningPlane.transform.position.z);
		this.endPos = new Vector3(endPos.x, endPos.y, TweeningController.Instance.tweeningPlane.transform.position.z);
		tweeningElement.transform.position = startPos;
		this.duration = duration;
		this.onComplete = (Callback)Delegate.Combine(this.onComplete, onComplete);
		this.tweeningFunction = tweeningFunction;
		StartCoroutine(Tween());
	}

	public void StartTweening(RectTransform tweeningElement, Vector3 startPos, RectTransform endPosByElement, float duration, Callback onComplete)
	{
		tweeningElement.transform.SetParent(TweeningController.Instance.tweeningPlane);
		tweeningElement.transform.localPosition = new Vector3(tweeningElement.transform.localPosition.x, tweeningElement.transform.localPosition.y, 0f);
		this.tweeningElement = tweeningElement;
		this.startPos = startPos;
		this.endPosByElement = endPosByElement;
		endAtElement = true;
		this.duration = duration;
		this.onComplete = (Callback)Delegate.Combine(this.onComplete, onComplete);
		tweeningFunction = TweeningController.TweeningFunction.LINEAR;
		StartCoroutine(Tween());
	}

	public IEnumerator Tween()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float timeAtLastFrame = realtimeSinceStartup;
		float time = 0f;
		while (time < duration)
		{
			realtimeSinceStartup = Time.realtimeSinceStartup;
			float num = realtimeSinceStartup - timeAtLastFrame;
			timeAtLastFrame = realtimeSinceStartup;
			time += num;
			if (endAtElement)
			{
				tweeningElement.transform.position = Vector3.Lerp(startPos, endPosByElement.position + new Vector3(0f, tweeningElement.sizeDelta.y - endPosByElement.sizeDelta.y, 0f), time / duration);
			}
			else
			{
				switch (tweeningFunction)
				{
				case TweeningController.TweeningFunction.EASEINBACK:
					tweeningElement.transform.position = startPos + (endPos - startPos) * EaseInBack(time / duration);
					break;
				case TweeningController.TweeningFunction.LINEAR:
					tweeningElement.transform.position = Vector3.Lerp(startPos, endPos, time / duration);
					break;
				}
			}
			yield return null;
		}
		onComplete();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public float EaseInBack(float value)
	{
		return value * value * (2.70158f * value - 1.70158f);
	}
}
