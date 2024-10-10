using UnityEngine;

public class InternalCameraSwitch : InternalModule
{
	[KSPField]
	public string colliderTransformName = "colliderName";

	[KSPField]
	public string cameraTransformName = "cameraName";

	public Transform colliderTransform;

	public Transform cameraTransform;

	public Transform oldParent;

	public override void OnAwake()
	{
		if (cameraTransform == null)
		{
			cameraTransform = internalProp.FindModelTransform(cameraTransformName);
			if (cameraTransform == null)
			{
				Debug.LogError("InternalCameraSwitch: cameraTransform '" + cameraTransformName + "' is null");
			}
		}
		if (colliderTransform == null)
		{
			colliderTransform = internalProp.FindModelTransform(colliderTransformName);
			if (colliderTransform == null)
			{
				Debug.LogError("InternalCameraSwitch: colliderTransform '" + colliderTransformName + "' is null");
			}
		}
		if (colliderTransform != null && cameraTransform != null)
		{
			InternalButton.Create(colliderTransform.gameObject).OnDoubleTap(Button_OnDoubleTap);
		}
	}

	public void Button_OnDoubleTap()
	{
		if (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal)
		{
			if (InternalCamera.Instance.transform.parent != cameraTransform)
			{
				CameraManager.Instance.SetCameraInternal(base.internalModel, cameraTransform);
			}
			else
			{
				CameraManager.Instance.SetCameraIVA();
			}
		}
		else
		{
			CameraManager.Instance.SetCameraInternal(base.internalModel, cameraTransform);
		}
	}
}
