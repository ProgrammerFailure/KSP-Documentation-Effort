using System;

namespace ns8;

public class IOException : Exception
{
	public string message;

	public string source;

	public string stack;

	public override string Message => message;

	public override string Source => source;

	public override string StackTrace => stack;

	public IOException(string message, string source, string stack)
	{
		this.message = message;
		this.source = source;
		this.stack = stack;
	}
}
