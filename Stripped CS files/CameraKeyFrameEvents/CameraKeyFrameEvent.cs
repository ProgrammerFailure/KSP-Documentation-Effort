using UnityEngine;

namespace CameraKeyFrameEvents;

public class CameraKeyFrameEvent : MonoBehaviour
{
	public float timeIntoFrame;

	public bool done;

	public virtual void RunEvent()
	{
	}
}
