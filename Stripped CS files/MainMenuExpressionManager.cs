using UnityEngine;

public class MainMenuExpressionManager : MonoBehaviour
{
	[SerializeField]
	public Animator animationControl;

	[SerializeField]
	public float expressionValue;

	[SerializeField]
	public float expressionVariance = 0.5f;

	[SerializeField]
	public float expressionSecondVariance = 0.3f;

	public void Start()
	{
		animationControl.SetFloat("Expression", expressionValue);
		animationControl.SetFloat("Variance", expressionVariance);
		animationControl.SetFloat("SecondaryVariance", expressionSecondVariance);
	}
}
