using System.Collections.Generic;

namespace ns7;

public class TestResults
{
	public int failed;

	public int success;

	public List<TestState> states = new List<TestState>();
}
