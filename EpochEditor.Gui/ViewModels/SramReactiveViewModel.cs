using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using AutoMapper;
using DynamicData.Binding;
using EpochEditor.SramUtilities;
using Microsoft.VisualBasic;
using ReactiveUI;

namespace EpochEditor.Gui.ViewModels;

public class SramReactiveViewModel : ReactiveObject {

    private Sram?           _sram;
    private IList<SlotOption> _slotOptions = [];
    private IList<EditorGroupOption> _editorGroupOptions = [];
    private int _slotSelectionIndex = -1;
    private int _editorGroupSelectionIndex = -1;

    public SramReactiveViewModel() {
        this._sram = null;

        var config = new MapperConfiguration(cfg => cfg.CreateMap<ICharacterSheet, CharacterReactiveViewModel>());

        Characters = [ ];
    }

    public IList<CharacterReactiveViewModel> Characters { get; private set; }

    public Sram? Sram { get { return this._sram; } set { this._sram = value; UpdateViewModelFromSram(); } }

    public Boolean SramIsLoading { get; set; } = true;
    public Boolean SlotIsLoading { get; set; } = true;

    public UInt16? SlotChecksum { get => this._sram?.GameSlots?.Skip(this._slotSelectionIndex)?.FirstOrDefault()?.ComputeChecksum(); }

    public IList<SlotOption> SlotOptions { get => _slotOptions; set => this.RaiseAndSetIfChanged(ref _slotOptions, value); }

    public IList<EditorGroupOption> EditorGroupOptions { get => _editorGroupOptions; set => this.RaiseAndSetIfChanged(ref _editorGroupOptions, value); }

    public int SlotSelectionIndex { get => _slotSelectionIndex; set { this.RaiseAndSetIfChanged(ref _slotSelectionIndex, value); this.RaisePropertyChanged(nameof(SlotChecksum)); UpdateEditorGroupUiElements(); } }

    public int EditorGroupSelectionIndex { get => _editorGroupSelectionIndex; set { this.RaiseAndSetIfChanged(ref _editorGroupSelectionIndex, value); UpdateCharacterSheetVisibility(); } }

    private void UpdateViewModelFromSram()
    {
        if (null == this._sram) {
            throw new EpochEditorException("Cannot update viewmodel from null SRAM.");
        }

        SramIsLoading = true;
        SlotOptions =
            this
                ._sram
                .GameSlots
                .Select((s, i) => new SlotOption { SlotIndex = i })
                .ToList();
        SramIsLoading = false;
    }

    public void RaiseSlotChecksumChange()
    {
        this.RaisePropertyChanged(nameof(SlotChecksum));
    }

    private void UpdateEditorGroupUiElements() {
        if (0 > this._slotSelectionIndex || this._slotSelectionIndex >= this._slotOptions.Count) {
            return;
        }

        var gameSlot = this._sram?.GameSlots[this._slotSelectionIndex] ?? throw new EpochEditorAvaloniaSetupException();

        Characters =
            gameSlot
                .CharacterSheets
                .Select((cs, i) => new CharacterReactiveViewModel(this, i, cs, false))
                .ToList();
        this.RaisePropertyChanged(nameof(Characters));
        
        var oldEditorGroupSelectionIndex = EditorGroupSelectionIndex;
        this._editorGroupOptions = new List<EditorGroupOption>();
        for (var i = 0; i < gameSlot.CharacterSheets.Length; i++)
        {
            var character = gameSlot.CharacterSheets[i];
            this._editorGroupOptions.Add(new CharacterEditorGroupOption { CharacterName = character.Name, CharacterIndex = i });
        }

        this.RaisePropertyChanged(nameof(EditorGroupOptions));
        
        EditorGroupSelectionIndex = oldEditorGroupSelectionIndex;
    }

    private void UpdateCharacterSheetVisibility() {
        if (0 > this._editorGroupSelectionIndex || this._editorGroupSelectionIndex >= this.EditorGroupOptions.Count) {
            return;
        }
        
        foreach (var c in Characters) { c.ShouldBeVisible = false; }
        Characters[_editorGroupSelectionIndex].ShouldBeVisible = true;
    }
}
