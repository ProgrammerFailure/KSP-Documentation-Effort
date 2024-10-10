using UnityEngine;

namespace ns2;

public class AnchoredDialogHost : MonoBehaviour
{
	public AnchoredDialog host;

	public Callback OnHostLateUpdate;

	public void LateUpdate()
	{
		if (host != null)
		{
			OnHostLateUpdate();
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
