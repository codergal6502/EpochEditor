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


public class GameSlotViewModel : ReactiveObject {
    
    private MainWindowViewModel _mainWindowViewModel;
    private int                 _editorGroupSelectionIndex = -1;
    private bool                _shouldBeVisible = false;

    private GameSlot _gameSlot;

    public GameSlotViewModel(MainWindowViewModel mainWindowViewModel, GameSlot gameSlot)
    {
        this._mainWindowViewModel = mainWindowViewModel;
        this._gameSlot = gameSlot;

        var newEditorGroupOptions = new List<EditorGroupOption>();
        for (var i = 0; i < gameSlot.CharacterSheets.Length; i++)
        {
            var character = gameSlot.CharacterSheets[i];
            newEditorGroupOptions.Add(new CharacterEditorGroupOption { CharacterName = character.Name, CharacterIndex = i });
        }

        foreach (var groupType in SlotEditorGroupOption.GetAllTypes())
        {
            newEditorGroupOptions.Add(new SlotEditorGroupOption { GroupType = groupType });
        }

        EditorGroupOptions = newEditorGroupOptions;

        CharacterViewModels =
            gameSlot
                .CharacterSheets
                .Select((cs, i) => new CharacterViewModel(this, i, cs, false))
                .ToList();
        this.RaisePropertyChanged(nameof(CharacterViewModels));

        this.InventoryViewModel = new InventoryViewModel(this, gameSlot.Inventory);
        this.GameStateViewModel = new GameStateViewModel(this, gameSlot);

        NormalizeEditorGroupSelectionIndex();
    }

    private void NormalizeEditorGroupSelectionIndex()
    {
        if (0 > EditorGroupSelectionIndex) EditorGroupSelectionIndex = 0;
        else if (EditorGroupSelectionIndex >= EditorGroupOptions.Count) EditorGroupSelectionIndex = EditorGroupOptions.Count - 1;
    }

    // Combo Box Options
    public IList<EditorGroupOption> EditorGroupOptions { get; set; }

    // Display Logic Properties
    public bool ShouldBeVisible { get => _shouldBeVisible; set => this.RaiseAndSetIfChanged(ref _shouldBeVisible, value); }

    // Display Logic Utilities
    public bool IsSramLoading() => this._mainWindowViewModel.IsSramLoading();

    // Child View-Models
    public IList<CharacterViewModel> CharacterViewModels { get; }
    public InventoryViewModel InventoryViewModel { get; set; }
    public GameStateViewModel GameStateViewModel { get; set; }

    // Mappings to SRAM
    public UInt16? SlotChecksum { get => this._gameSlot.ComputeChecksum(); }

    public int EditorGroupSelectionIndex { get => _editorGroupSelectionIndex; set { this.RaiseAndSetIfChanged(ref _editorGroupSelectionIndex, value); UpdateEditorGroupVisibility(); } }

    public void RaiseSlotChecksumChange()
    {
        this.RaisePropertyChanged(nameof(SlotChecksum));
    }

    private void UpdateEditorGroupVisibility() {
        if (0 > this._editorGroupSelectionIndex || this._editorGroupSelectionIndex >= this.EditorGroupOptions.Count) {
            return;
        }
        
        var selectedEditorGroup = EditorGroupOptions[this._editorGroupSelectionIndex];

        foreach (var c in CharacterViewModels) { c.ShouldBeVisible = false; }
        GameStateViewModel.ShouldBeVisible = false;
        InventoryViewModel.ShouldBeVisible = false;
        
        if (selectedEditorGroup is CharacterEditorGroupOption characterEditorGroup) {
            CharacterViewModels[characterEditorGroup.CharacterIndex].ShouldBeVisible = true;
        }
        else if (selectedEditorGroup is SlotEditorGroupOption slotEditorGroup) {
            switch(slotEditorGroup.GroupType) {
                case SlotEditorGroupOption.SlotEditorGroupType.Inventory: {
                    InventoryViewModel.ShouldBeVisible = true;
                    break;
                }
                case SlotEditorGroupOption.SlotEditorGroupType.GameState: {
                    GameStateViewModel.ShouldBeVisible = true;
                    break;
                }
            }
        }
    }
}
