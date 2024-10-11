using UnityEngine;

public interface IMouseEvents
{
	void OnMouseEnter();

	void OnMouseDown();

	void OnMouseDrag();

	void OnMouseUp();

	void OnMouseExit();

	MonoBehaviour GetInstance();
}
