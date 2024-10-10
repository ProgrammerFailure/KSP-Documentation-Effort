using System.Collections;
using TMPro;
using UnityEngine;

public class EnvMapAnimator : MonoBehaviour
{
	public Vector3 RotationSpeeds;

	public TMP_Text m_textMeshPro;

	public Material m_material;

	public void Awake()
	{
		m_textMeshPro = GetComponent<TMP_Text>();
		m_material = m_textMeshPro.fontSharedMaterial;
	}

	public IEnumerator Start()
	{
		Matrix4x4 matrix = default(Matrix4x4);
		while (true)
		{
			matrix.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * RotationSpeeds.x, Time.time * RotationSpeeds.y, Time.time * RotationSpeeds.z), Vector3.one);
			m_material.SetMatrix("_EnvMatrix", matrix);
			yield return null;
		}
	}
}
