using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

/// <summary>
/// 	The data contracted operation result class of closed generic.
/// </summary>
/// <typeparam name = "TResult">The type of data expected for the returning result.</typeparam>
[DataContract]
public class OperationResult<TResult>
{
	/// <summary>
	/// 	The operation result data.
	/// </summary>
	[DataMember]
	public TResult Result { get; set; }

	/// <summary>
	/// 	An indicator of the operation suceess. False means no errors.
	/// </summary>
	[DataMember]
	public bool Error { get; set; }

	/// <summary>
	/// 	A string representing messages from the operation.
	/// </summary>
	[DataMember]
	public string Message { get; set; }

	[DataMember]
	public List<string> Messages { get; set; }

	[DataMember]
	public List<string> StackTraces { get; set; }
	#region Ctors

	/// <summary>
	/// 	Initializes a new instance of the <see cref = "OperationResult{TResult}" /> class.
	/// </summary>
	public OperationResult()
	{
		Messages = new List<string>();
		StackTraces = new List<string>();
	}

	public OperationResult(string error)
		: this()
	{
		Error = true;
		Messages.Add(error);
		Message = error;
	}

	public OperationResult(Exception e, bool includeStackTrace = false)
		: this()
	{
		Error = true;
		if (e != null)
		{
			Message = e.Message;
			BuildException(e, includeStackTrace);
		}
	}

	public OperationResult(Exception e, TResult result, bool includeStackTrace = false)
		: this()
	{
		Result = result;
		Error = true;
		if (e != null)
		{
			Message = e.Message;
			BuildException(e, includeStackTrace);
		}
	}

	public OperationResult(OperationResult<TResult> operationResult)
		: this()
	{
		Result = operationResult.Result;
		Error = operationResult.Error;
		Message = operationResult.Message;
		Messages = operationResult.Messages;
		StackTraces = operationResult.StackTraces;
	}

	/// <summary>
	/// 	Initializes a new instance of the <see cref = "OperationResult{TResult}" /> class.
	/// </summary>
	/// <param name = "result">
	/// 	The result.
	/// </param>
	/// <param name = "error">
	/// 	The error.
	/// </param>
	/// <param name = "message"></param>
	public OperationResult(TResult result, bool error = false, string message = null)
		: this()
	{
		Result = result;
		Error = error;
		Message = message;
		if (error)
			Messages.Add(message);
	}

	public OperationResult(TResult result, Exception e)
		: this()
	{
		Result = result;
		Error = true;
		Message = e.Message;
		BuildException(e, false);
	}

	public OperationResult(List<string> errors)
		: this()
	{
		Error = true;
		Messages = errors;
	}

	#endregion
	#region Private methods

	private void BuildException(Exception e, bool includeStackTrace)
	{
		if (e == null)
			return;

		Messages.Add(e.Message);
		if (includeStackTrace)
			StackTraces.Add(e.StackTrace);
		if (e.InnerException != null)
			BuildException(e.InnerException, includeStackTrace);
	}

	#endregion
}