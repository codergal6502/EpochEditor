using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection.Metadata;
using AutoMapper;
using DynamicData.Binding;
using EpochEditor.SramUtilities;
using Microsoft.VisualBasic;
using ReactiveUI;

namespace EpochEditor.Gui.ViewModels;

public class SramReactiveViewModel : ReactiveObject {

    private Sram? _sram;
    private IList<SlotOption> _slotOptions = [];
    private IList<EditorGroupOption> _editorGroupOptions = [];
    private int _slotSelectionIndex = -1;
    private int _editorGroupSelectionIndex = -1;

    public SramReactiveViewModel() {
        this._sram = null;

        var config = new MapperConfiguration(cfg => cfg.CreateMap<ICharacterSheet, CharacterReactiveViewModel>());

        Characters = [ ];
        
        GameStateViewModel = new();
        InventoryViewModel = null;
    }

    public InventoryViewModel? InventoryViewModel { get; set; }

    public IList<CharacterReactiveViewModel> Characters { get; private set; }
    
    public GameStateViewModel GameStateViewModel { get; private set; }

    public Sram? Sram { get { return this._sram; } set { this._sram = value; UpdateViewModelFromSram(); } }

    public Boolean SramIsLoading { get; set; } = true;

    public UInt16? SlotChecksum { get => this._sram?.GameSlots?.Skip(this._slotSelectionIndex)?.FirstOrDefault()?.ComputeChecksum(); }

    public IList<SlotOption> SlotOptions { get => _slotOptions; set => this.RaiseAndSetIfChanged(ref _slotOptions, value); }

    public IList<EditorGroupOption> EditorGroupOptions { get => _editorGroupOptions; set => this.RaiseAndSetIfChanged(ref _editorGroupOptions, value); }

    public int SlotSelectionIndex { get => _slotSelectionIndex; set { this.RaiseAndSetIfChanged(ref _slotSelectionIndex, value); this.RaisePropertyChanged(nameof(SlotChecksum)); UpdateViewModelForSlot(); } }

    public Byte? LastSaveSlotIndex { get => this._sram?.LastSaveSlotIndex; set { if (null != this._sram && false == SramIsLoading) { this._sram.LastSaveSlotIndex = value ?? 0; }; } }

    public int EditorGroupSelectionIndex { get => _editorGroupSelectionIndex; set { this.RaiseAndSetIfChanged(ref _editorGroupSelectionIndex, value); UpdateEditorGroupVisibility(); } }

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
        this.RaisePropertyChanged(nameof(LastSaveSlotIndex));
        SramIsLoading = false;
    }

    public void RaiseSlotChecksumChange()
    {
        this.RaisePropertyChanged(nameof(SlotChecksum));
    }

    private void UpdateViewModelForSlot() {
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

        this.InventoryViewModel = new InventoryViewModel(this, gameSlot.Inventory);
        this.RaisePropertyChanged(nameof(InventoryViewModel));
        
        var oldEditorGroupSelectionIndex = EditorGroupSelectionIndex;
        this._editorGroupOptions = new List<EditorGroupOption>();
        for (var i = 0; i < gameSlot.CharacterSheets.Length; i++)
        {
            var character = gameSlot.CharacterSheets[i];
            this._editorGroupOptions.Add(new CharacterEditorGroupOption { CharacterName = character.Name, CharacterIndex = i });
        }

        foreach (var groupType in SlotEditorGroupOption.GetAllTypes()) {
            this._editorGroupOptions.Add(new SlotEditorGroupOption { GroupType = groupType });
        }

        this.RaisePropertyChanged(nameof(EditorGroupOptions));
        
        EditorGroupSelectionIndex = oldEditorGroupSelectionIndex;
    }

    private void UpdateEditorGroupVisibility() {
        if (0 > this._editorGroupSelectionIndex || this._editorGroupSelectionIndex >= this.EditorGroupOptions.Count) {
            return;
        }
        
        var selectedEditorGroup = this._editorGroupOptions[this._editorGroupSelectionIndex];

        foreach (var c in Characters) { c.ShouldBeVisible = false; }
        // this.PlaceholderViewModel.ShouldBeVisible = false;
        this.GameStateViewModel.ShouldBeVisible = false;
        if (null != InventoryViewModel) InventoryViewModel.ShouldBeVisible = false;
        
        if (selectedEditorGroup is CharacterEditorGroupOption characterEditorGroup) {
            Characters[characterEditorGroup.CharacterIndex].ShouldBeVisible = true;
        }
        else if (selectedEditorGroup is SlotEditorGroupOption slotEditorGroup) {
            switch(slotEditorGroup.GroupType) {
                case SlotEditorGroupOption.SlotEditorGroupType.Inventory: {
                    // this.PlaceholderViewModel.ShouldBeVisible = true;
                    if (null != InventoryViewModel) InventoryViewModel.ShouldBeVisible = true;
                    break;
                }
                case SlotEditorGroupOption.SlotEditorGroupType.GameState: {
                    this.GameStateViewModel.ShouldBeVisible = true;
                    break;
                }
            }
        }
    }
}

public class GameStateViewModel : ReactiveObject
{
    private bool _shouldBeVisible;
    public Boolean ShouldBeVisible { get => _shouldBeVisible; set => this.RaiseAndSetIfChanged(ref _shouldBeVisible, value); }
}