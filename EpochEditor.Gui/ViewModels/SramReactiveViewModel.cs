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

    private Sram? _sram;
    private IMapper _characterMapper;
    private int _egoi;
    private int _slotIndex = 0;
    private IList<ushort> _checksums;

    public SramReactiveViewModel() {
        this._sram = null;

        var config = new MapperConfiguration(cfg => cfg.CreateMap<ICharacterSheet, CharacterReactiveViewModel>());
        this._characterMapper = config.CreateMapper();

        Characters = [ ];
        EditorGroupOptions = [ ];
        SlotIndices = [ ];

        this.WhenAnyValue(o => o.EditorGroupOptionIndex).Subscribe(_ => { 
            var x = _.HasValue ? _.Value : -1;
         });

        this._checksums = [0, 0, 0];

        this.PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        
    }

    public int SlotIndex { 
        get {
             return this._slotIndex;
        }
        set {
            this.RaiseAndSetIfChanged(ref _slotIndex, value);
            if (false == SramIsLoading) {
                this.RaisePropertyChanged(nameof(SlotChecksum));

                SlotIsLoading = true;
                UpdateViewModelFromSlotIndex();
                SlotIsLoading = false;
            }
        }
    }

    public int? EditorGroupOptionIndex { 
        get { return this._egoi; }
        set {
            if (false == SlotIsLoading) {
                this._egoi = value ?? throw new EpochEditorAvaloniaSetupException();
                var selectedEgo = EditorGroupOptions[this._egoi];
                foreach (var c in Characters) { c.ShouldBeVisible = false; }
                if (selectedEgo is CharacterEditorGroupOption charEgo) {
                    Characters[this._egoi].ShouldBeVisible = true;
                }
            }
        } 
    }

    public List<int> SlotIndices { get; private set; }

    public List<EditorGroupOption> EditorGroupOptions { get; private set; }

    public IList<CharacterReactiveViewModel> Characters { get; private set; }

    public Sram? Sram { get { return this._sram; } set { this._sram = value; UpdateViewModelFromSram(); } }

    public Boolean SramIsLoading { get; set; } = true;
    public Boolean SlotIsLoading { get; set; } = true;

    public UInt16? SlotChecksum { get => this._sram?.GameSlots?.Skip(this._slotIndex)?.FirstOrDefault()?.ComputeChecksum(); }

    // public IList<UInt16>? ComputedChecksums { get => _sram. }

    private void UpdateViewModelFromSram()
    {
        if (null == this._sram) {
            throw new EpochEditorException("Cannot update viewmodel from NULL SRAM.");
        }

        SramIsLoading = true;

        SlotIndices = this._sram.GameSlots.Select((s, i) => i).ToList();
        this.RaisePropertyChanged(nameof(SlotIndices));

        SramIsLoading = false;
    }

    private void UpdateViewModelFromSlotIndex() {
        var sram = this._sram ?? throw new EpochEditorAvaloniaSetupException("Selecting a slot without an SRAM loaded!");
        var gameSlot =
            sram
                .GameSlots
                .Skip(_slotIndex)
                .FirstOrDefault() ?? throw new EpochEditorAvaloniaSetupException($"Slot index {_slotIndex} exceeds number of game slots {sram.GameSlots.Length}.");

        List<EditorGroupOption> newEgos =
            gameSlot
                .CharacterSheets
                .Select((cs, i) => new CharacterEditorGroupOption { CharacterIndex = i, CharacterName = cs.Name })
                .OfType<EditorGroupOption>()
                .ToList();

        // This isn't yet supported; comment this back in when it is.
        // newEgos.Add(new SlotEditorGroupOption { Group = "Inventory" });
        // newEgos.Add(new SlotEditorGroupOption { Group = "The Rest" });

        EditorGroupOptions = newEgos;
        this.RaisePropertyChanged(nameof(EditorGroupOptions));

        Characters =
            gameSlot
                .CharacterSheets
                .Select((cs, i) => new CharacterReactiveViewModel(this, i, cs, false))
                .ToList();
        this.RaisePropertyChanged(nameof(Characters));
        EditorGroupOptionIndex = 0;
        this.RaisePropertyChanged(nameof(EditorGroupOptionIndex));

        RaiseSlotChecksumChange();
    }

    public void RaiseSlotChecksumChange()
    {
        this.RaisePropertyChanged(nameof(SlotChecksum));
    }
}
