using UnityEngine;

public class PartPointer : MonoBehaviour
{
	[SerializeField]
	public Part part;

	public Part PartReference => part;

	public void SetPart(Part part)
	{
		this.part = part;
	}

	public Part GetPart()
	{
		return part;
	}
}
