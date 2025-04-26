using System.Runtime.Serialization;

namespace EpochEditor.SramUtilities;

public class EpochEditorException : Exception
{
    public EpochEditorException()
    {
    }

    public EpochEditorException(String? message) : base(message)
    {
    }

    public EpochEditorException(String? message, Exception? innerException) : base(message, innerException)
    {
    }
}
