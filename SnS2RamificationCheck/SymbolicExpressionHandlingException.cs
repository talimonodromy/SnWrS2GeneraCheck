using System;

public class SymbolicExpressionHandlingException : Exception
{
    public SymbolicExpressionHandlingException()
    {

    }

    public SymbolicExpressionHandlingException(string message) : base(message)
    {

    }

    public SymbolicExpressionHandlingException(string message, Exception inner) : base(message, inner)
    {

    }

}
