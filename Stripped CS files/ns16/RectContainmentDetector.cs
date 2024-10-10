using ns2;
using UnityEngine;

namespace ns16;

public class RectContainmentDetector : MonoBehaviour
{
	public RectTransform container;

	public RectTransform refRect;

	public bool twoWayTest;

	public RectUtil.ContainmentLevel level;

	public RectUtil.ContainmentLevel levelLast;

	public bool initialUpdate;

	[SerializeField]
	public Camera refCamera;

	public RectUtil.ContainmentLevel Level => level;

	public event Callback<RectUtil.ContainmentLevel> OnContainmentChanged = delegate
	{
	};

	public void Start()
	{
		refCamera = UIMainCamera.Camera;
		if (refRect == null)
		{
			refRect = GetComponent<RectTransform>();
		}
		level = RectUtil.ContainmentLevel.Full;
		levelLast = RectUtil.ContainmentLevel.None;
		initialUpdate = false;
	}

	public void Update()
	{
		level = RectUtil.GetRectContainment(refRect, container, refCamera, twoWayTest);
		if (level != levelLast || !initialUpdate)
		{
			this.OnContainmentChanged(level);
			levelLast = level;
			initialUpdate = true;
		}
	}
}
