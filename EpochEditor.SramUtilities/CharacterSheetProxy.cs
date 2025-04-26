using System.Reflection;

namespace EpochEditor.SramUtilities;

public class CharacterSheetProxy : DispatchProxy
{
    private CharacterSheet? _target = null;
    private GameSlot? _gameSlot = null;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        var result = targetMethod?.Invoke(_target, args);

        var setter = typeof(ICharacterSheet).GetProperties().Where(p => p.GetSetMethod() == targetMethod).FirstOrDefault();
        if (null != setter && null != _target && null != _gameSlot) {
            if (_gameSlot.CharacterSheets.All(cs => false == Object.ReferenceEquals(cs, this))) {
                throw new Exception("character sheet isn't part of sram");
            }

            // This is the cheap and cheerful way of doing this; a more elegant way would be to only update the bytes that are necessary.
            _gameSlot.UpdateBytesFromCharacterSheets();
        }

        return result;
    }

    public static ICharacterSheet CreateProxy(CharacterSheet target, GameSlot sram)
    {
        if (null == target) {
            throw new Exception();
        }

        if (null == sram) {
            throw new Exception();
        }

        var proxy = Create<ICharacterSheet, CharacterSheetProxy>() as CharacterSheetProxy;

        if (proxy is ICharacterSheet) {
            proxy._target = target;
            proxy._gameSlot = sram;
            return (ICharacterSheet)proxy;
        }
        else {
            throw new Exception();
        }
    }
}
