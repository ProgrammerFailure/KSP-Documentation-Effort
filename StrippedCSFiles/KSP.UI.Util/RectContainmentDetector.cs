using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Util;

public class RectContainmentDetector : MonoBehaviour
{
	public RectTransform container;

	public RectTransform refRect;

	public bool twoWayTest;

	private RectUtil.ContainmentLevel level;

	private RectUtil.ContainmentLevel levelLast;

	private bool initialUpdate;

	[SerializeField]
	private Camera refCamera;

	public RectUtil.ContainmentLevel Level
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public event Callback<RectUtil.ContainmentLevel> OnContainmentChanged
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		add
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		remove
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RectContainmentDetector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Update()
	{
		throw null;
	}
}
