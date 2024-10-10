using System.Collections;
using UnityEngine;

namespace TMPro.Examples;

public class ShaderPropAnimator : MonoBehaviour
{
	public Renderer m_Renderer;

	public Material m_Material;

	public AnimationCurve GlowCurve;

	public float m_frame;

	public void Awake()
	{
		m_Renderer = GetComponent<Renderer>();
		m_Material = m_Renderer.material;
	}

	public void Start()
	{
		StartCoroutine(AnimateProperties());
	}

	public IEnumerator AnimateProperties()
	{
		m_frame = Random.Range(0f, 1f);
		while (true)
		{
			float value = GlowCurve.Evaluate(m_frame);
			m_Material.SetFloat(ShaderUtilities.ID_GlowPower, value);
			m_frame += Time.deltaTime * Random.Range(0.2f, 0.3f);
			yield return new WaitForEndOfFrame();
		}
	}
}
