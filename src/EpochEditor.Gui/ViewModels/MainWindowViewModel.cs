using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EpochEditor.SramUtilities;
using ReactiveUI;

namespace EpochEditor.Gui.ViewModels;

public partial class MainWindowViewModel : ReactiveObject
{
    private string                      _currentPath = String.Empty;
    private Sram?                       _sram = null;
    private IList<SlotOption>           _slotOptions = [];
    private IList<GameSlotViewModel>    _gameSlotViewModels = [];
    private bool                        _sramIsLoading = true;
    private int                         _slotSelectionIndex = -1;

    // Display Logic Properties
    public String CurrentPath {
        get => _currentPath;
        set {
            this.RaiseAndSetIfChanged(ref _currentPath, value);
            this.RaisePropertyChanged(nameof(HasFileBeenLoaded)); 
        }
    }    
    public bool HasFileBeenLoaded { get => !String.IsNullOrWhiteSpace(CurrentPath); } 
    public PlatformUtilities PlatformUtilities { get => PlatformUtilities.Instance; }

    public int SlotSelectionIndex {
        get => _slotSelectionIndex;
        set {
            var oldEgo = CurrentGameSlotViewModel?.EditorGroupSelectionIndex;

            foreach (var gs in GameSlotViewModels) {
                gs.ShouldBeVisible = false;
            }

            this.RaiseAndSetIfChanged(ref _slotSelectionIndex, value);
            if (null != CurrentGameSlotViewModel) {
                // This keeps Avalonia from losing the combo box index when the combo box items change.
                this.RaisePropertyChanged(nameof(CurrentGameSlotViewModel));
                CurrentGameSlotViewModel.ShouldBeVisible = true;
                CurrentGameSlotViewModel.EditorGroupSelectionIndex = oldEgo ?? default;
            }
        }
    }

    // Display Logic Utilities
    public bool IsSramLoading() => null != this._sram && _sramIsLoading;
    public Sram GetNonNullSram() => _sram ?? throw new EpochEditorAvaloniaSetupException($"Unexpected null {nameof(_sram)}.");

    // Combo Box Options    
    public IList<SlotOption> SlotOptions { get => _slotOptions; set => this.RaiseAndSetIfChanged(ref _slotOptions, value); }

    // Child View-Models
    public GameSlotViewModel? CurrentGameSlotViewModel {
        get {
            if (0 > _slotSelectionIndex) return null;
            if (0 == _gameSlotViewModels.Count) return null;
            return _gameSlotViewModels.Skip(_slotSelectionIndex).FirstOrDefault();
        }
    }

    public IList<GameSlotViewModel> GameSlotViewModels { get => _gameSlotViewModels; set => this.RaiseAndSetIfChanged(ref _gameSlotViewModels, value); }

    // Mappings to SRAM
    public void LoadSram(string localPath)
    {
        SramReader sr = new SramReader();
        this._sram = sr.ReadBytes(File.ReadAllBytes(localPath));
        this._currentPath = localPath;
        UpdateWithSram();
    }

    public Byte? SramLastSaveSlotUsed {
        get => this._sram?.LastSaveSlotIndex;
        set {
            if (IsSramLoading()) {
                GetNonNullSram().LastSaveSlotIndex = value ?? 0; 
            }; 
        }
    }

    public void UpdateWithSram() {
        if (null == this._sram) {
            throw new EpochEditorException("Cannot update viewmodel from null SRAM.");
        }

        this._sramIsLoading = true;
        SlotOptions =
            this
                ._sram
                .GameSlots
                .Select((s, i) => new SlotOption { SlotIndex = i })
                .ToList();
        if (0 > _slotSelectionIndex) _slotSelectionIndex = 0;
        else if (_slotSelectionIndex >= SlotOptions.Count) _slotSelectionIndex = SlotOptions.Count - 1;
        
        GameSlotViewModels = 
            this
                ._sram
                .GameSlots
                .Select((gs, i) => new GameSlotViewModel(this, gs) { ShouldBeVisible = false})
                .ToList();

        this.RaisePropertyChanged(nameof(SlotSelectionIndex));
        this.RaisePropertyChanged(nameof(SramLastSaveSlotUsed));
        this.RaisePropertyChanged(nameof(CurrentGameSlotViewModel));
        this.RaisePropertyChanged(nameof(HasFileBeenLoaded));
        
        if (null != CurrentGameSlotViewModel) {
            CurrentGameSlotViewModel.ShouldBeVisible = true;
        }

        this._sramIsLoading = false;
    }

    public void SaveToCurrentPath()
    {
        File.WriteAllBytes(
            this._currentPath ?? throw new EpochEditorAvaloniaSetupException($"Cannot save with null {nameof(_currentPath)}")
          , this.GetNonNullSram().RawBytes ?? throw new EpochEditorAvaloniaSetupException($"Cannot save with null {nameof(Sram.RawBytes)}.")
        );
    }

    public void SaveToNewPathPath(string localPath)
    {
        this._currentPath = localPath;
        SaveToCurrentPath();
    }
}
