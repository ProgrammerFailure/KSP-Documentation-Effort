using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class TweeningController : MonoBehaviour
{
	public enum TweeningFunction
	{
		LINEAR,
		EASEINBACK
	}

	public static TweeningController Instance;

	public RectTransform tweeningPlane;

	private int activeCount;

	public bool isTweening
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TweeningController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResizingLayoutElement SpawnResizingLayoutElement(RectTransform target, int index, int siblingIndex, float startHeight, float endHeight, float duration, bool destroyOnCompletion, bool addElementOnTopOfList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TweenIntoList(RectTransform tweeningElement, RectTransform target, int inverseStageIndex, int siblingIndex, float duration, Callback onComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Tween(RectTransform tweeningElement, Vector2 startPos, Vector2 endPos, float duration, TweeningFunction tweeningFunction, Callback onComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTweeningComplete()
	{
		throw null;
	}
}
