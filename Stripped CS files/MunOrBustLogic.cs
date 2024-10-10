using UnityEngine;

public class MunOrBustLogic : MonoBehaviour
{
	[SerializeField]
	public Material targetMaterial;

	public void Start()
	{
		Texture2D texture = GameDatabase.Instance.GetTexture("Squad/MenuProps/MunOrBust", asNormalMap: false);
		if (texture != null)
		{
			targetMaterial.SetTexture("_MainTex", texture);
		}
	}
}
