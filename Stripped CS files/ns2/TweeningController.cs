using UnityEngine;

namespace ns2;

public class TweeningController : MonoBehaviour
{
	public enum TweeningFunction
	{
		LINEAR,
		EASEINBACK
	}

	public static TweeningController Instance;

	public RectTransform tweeningPlane;

	public int activeCount;

	public bool isTweening => activeCount > 0;

	public void Awake()
	{
		if (Instance != null)
		{
			base.gameObject.DestroyGameObject();
			return;
		}
		Instance = this;
		tweeningPlane.gameObject.SetActive(value: true);
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnGameSceneLoadRequested(GameScenes scene)
	{
		int childCount = tweeningPlane.transform.childCount;
		while (childCount-- > 0)
		{
			Object.Destroy(tweeningPlane.transform.GetChild(childCount).gameObject);
		}
	}

	public ResizingLayoutElement SpawnResizingLayoutElement(RectTransform target, int index, int siblingIndex, float startHeight, float endHeight, float duration, bool destroyOnCompletion, bool addElementOnTopOfList)
	{
		if (index == 0 && !addElementOnTopOfList)
		{
			return null;
		}
		ResizingLayoutElement component = new GameObject("ResizingLayoutElement", typeof(ResizingLayoutElement)).GetComponent<ResizingLayoutElement>();
		component.transform.SetParent(target, worldPositionStays: false);
		if (siblingIndex != -1)
		{
			component.transform.SetSiblingIndex(siblingIndex);
		}
		else
		{
			component.transform.SetSiblingIndex(index);
		}
		component.StartResizing(startHeight, endHeight, duration, destroyOnCompletion, OnTweeningComplete);
		activeCount++;
		return component;
	}

	public bool TweenIntoList(RectTransform tweeningElement, RectTransform target, int inverseStageIndex, int siblingIndex, float duration, Callback onComplete)
	{
		ResizingLayoutElement resizingLayoutElement = SpawnResizingLayoutElement(target, inverseStageIndex, siblingIndex, 0f, tweeningElement.sizeDelta.y, duration, destroyOnCompletion: true, addElementOnTopOfList: false);
		if (resizingLayoutElement == null)
		{
			return false;
		}
		new GameObject("TweenignElement", typeof(TweeningElement)).GetComponent<TweeningElement>().StartTweening(tweeningElement, tweeningElement.position, resizingLayoutElement.GetComponent<RectTransform>(), duration, delegate
		{
			OnTweeningComplete();
			onComplete();
		});
		return true;
	}

	public void Tween(RectTransform tweeningElement, Vector2 startPos, Vector2 endPos, float duration, TweeningFunction tweeningFunction, Callback onComplete)
	{
		new GameObject("TweenignElement", typeof(TweeningElement)).GetComponent<TweeningElement>().StartTweening(tweeningElement, startPos, endPos, duration, tweeningFunction, delegate
		{
			OnTweeningComplete();
			onComplete();
		});
	}

	public void OnTweeningComplete()
	{
		activeCount--;
	}
}
