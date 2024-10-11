using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Serenity;

public class CurvePanelPoint : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public enum TangentTypes
	{
		In,
		Out
	}

	[SerializeField]
	private Button DeleteButton;

	[SerializeField]
	private Transform selectedStuff;

	[SerializeField]
	private Image pointImage;

	[SerializeField]
	private Color normalColor;

	[SerializeField]
	private Color selectedColor;

	[SerializeField]
	private bool noTangents;

	[SerializeField]
	internal bool noValue;

	internal bool unselectable;

	[SerializeField]
	private Transform leftHandleRotater;

	[SerializeField]
	private Transform rightHandleRotater;

	[SerializeField]
	private CanvasGroup leftHandleCanvasGroup;

	[SerializeField]
	private CanvasGroup rightHandleCanvasGroup;

	private Vector3 leftHandleRotation;

	private Vector3 rightHandleRotation;

	private int pointIndex;

	private int pointsCount;

	private Keyframe editingKeyframe;

	public Keyframe Keyframe
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public CurvePanel Panel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool Selected
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CurvePanelPoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(CurvePanel panel, Keyframe keyframe)
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
	private void OnDeleteClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Select(bool hideDelete = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Deselect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void HideDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ShowDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetTangentHandles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetKeyFrame(float time, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetKeyFrameTangents(float inTangent, float outTangent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetKeyFrameTangent(TangentTypes tangentType, float tangentValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Delete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEndDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnTangentDrag(TangentTypes tangentType, float tanValue)
	{
		throw null;
	}
}
