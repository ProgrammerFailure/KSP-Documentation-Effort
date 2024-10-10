using UnityEngine;

public class FXObjectPhoneHome : MonoBehaviour
{
	public FXObject parent;

	public void OnDestroy()
	{
		FXMonger.RemoveFXOjbect(parent);
	}
}
