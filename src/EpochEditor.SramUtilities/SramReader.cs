using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace EpochEditor.SramUtilities;

public class SramReader
{
    public SramReader() { }

    public Sram ReadBytes(Byte[] bytes) {
        Sram sram = new();
        Buffer.BlockCopy(bytes, 0, sram.RawBytes, 0, sram.RawBytes.Length);

        int currentSlotOffset = SramConstants.SLOT_1_START;
        for (var slotIndex = 0; slotIndex < sram.GameSlots.Length; slotIndex++) {
            GameSlot gameSlot = sram.GameSlots[slotIndex];

            if (bytes.Length != SramConstants.SRAM_FILE_LENGTH) {
                throw new Exception($"bad length {bytes.Length}");
            }

            for(int i = 0; i < SramConstants.INVENTORY_SIZE; i++) {
                gameSlot.Inventory[i] = new InventoryItem(
                    gameSlot
                  , bytes[currentSlotOffset + i]
                  , bytes[currentSlotOffset + SramConstants.INVENTORY_COUNT_START_OFFSET + i]
                );
            }

            int currentCharacterOffset = SramConstants.FIRST_CHARACTER_OFFSET;
            for (int i = 0; i < gameSlot.CharacterSheets.Length; i++) {
                CharacterSheet characterSheet = new CharacterSheet();

                foreach(PropertyInfo prop in typeof(ICharacterSheet).GetProperties()) {
                    CharacterSheetPropertyAttribute? attr = Attribute.GetCustomAttribute(prop, typeof(CharacterSheetPropertyAttribute)) as CharacterSheetPropertyAttribute;
                    if (null != attr) {
                        int pos = currentSlotOffset + currentCharacterOffset + attr.Offset;

                        if (typeof(Byte) == prop.PropertyType) {
                            Byte i8 = bytes[pos];
                            prop.GetSetMethod()?.Invoke(characterSheet, [i8]);
                        }
                        else if (typeof(Int16) == prop.PropertyType) {
                            Int16 i16 = BitConverter.ToInt16(bytes, pos);
                            prop.GetSetMethod()?.Invoke(characterSheet, [i16]);
                        }
                    }
                }

                characterSheet.Name = String.Concat(bytes.Skip(currentSlotOffset + 0x5B0 + characterSheet.NameId * 6).Take(6).TakeWhile(b => b != 0x00).Select(b => SramConstants.CT_CHAR_TO_ASCII[b]));
                gameSlot.CharacterSheets[i] = CharacterSheetProxy.CreateProxy(characterSheet, gameSlot);
                currentCharacterOffset += SramConstants.CHARACTER_SHEET_LENGTH;
            }

            var checksumPosition = SramConstants.CHECKSUM_OFFSET + slotIndex * 2;
            Span<byte> checksumSpan = new(bytes, checksumPosition, 2);
            gameSlot.LoadedChecksum = BitConverter.ToUInt16(checksumSpan);

            currentSlotOffset += SramConstants.SLOT_LENGTH;
        }

        return sram;
    }
}
