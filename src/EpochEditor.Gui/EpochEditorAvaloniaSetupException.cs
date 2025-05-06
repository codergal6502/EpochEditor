using EpochEditor.SramUtilities;
using System;

namespace EpochEditor.Gui;

public class EpochEditorAvaloniaSetupException : EpochEditorException
{
    public EpochEditorAvaloniaSetupException()
    {
    }

    public EpochEditorAvaloniaSetupException(string? message) : base(message)
    {
    }

    public EpochEditorAvaloniaSetupException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
