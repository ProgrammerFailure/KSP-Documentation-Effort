using ns2;
using TMPro;
using UnityEngine;

namespace ns11;

[RequireComponent(typeof(UIListItem))]
public class RDNodeListItem : MonoBehaviour
{
	public RDNode node;

	public TextMeshProUGUI text;
}
