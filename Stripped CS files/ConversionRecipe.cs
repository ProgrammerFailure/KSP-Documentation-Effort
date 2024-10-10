using System.Collections.Generic;

public class ConversionRecipe
{
	public List<ResourceRatio> _inputs = new List<ResourceRatio>();

	public List<ResourceRatio> _outputs = new List<ResourceRatio>();

	public List<ResourceRatio> _reqs = new List<ResourceRatio>();

	public List<ResourceRatio> Inputs => _inputs;

	public List<ResourceRatio> Outputs => _outputs;

	public List<ResourceRatio> Requirements => _reqs;

	public float FillAmount { get; set; }

	public float TakeAmount { get; set; }

	public ConversionRecipe()
	{
		Clear();
	}

	public void SetInputs(List<ResourceRatio> value)
	{
		if (value != null)
		{
			_inputs = value;
		}
	}

	public void SetOutputs(List<ResourceRatio> value)
	{
		if (value != null)
		{
			_outputs = value;
		}
	}

	public void SetRequirements(List<ResourceRatio> value)
	{
		if (value != null)
		{
			_reqs = value;
		}
	}

	public void Clear()
	{
		_inputs.Clear();
		_outputs.Clear();
		_reqs.Clear();
		FillAmount = 1f;
		TakeAmount = 1f;
	}
}
