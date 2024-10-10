using UnityEngine;

namespace ns2;

[RequireComponent(typeof(Renderer))]
public class RendererSortOrder : MonoBehaviour
{
	[SerializeField]
	public string layerName;

	[SerializeField]
	public int sortingOrder;

	public void Start()
	{
		Set();
	}

	[ContextMenu("Set")]
	public void Set()
	{
		Renderer component = GetComponent<Renderer>();
		component.sortingLayerName = layerName;
		component.sortingOrder = sortingOrder;
	}
}
