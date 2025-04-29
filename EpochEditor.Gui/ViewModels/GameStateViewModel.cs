using System;
using System.Collections.Generic;
using System.Linq;
using EpochEditor.SramUtilities;
using ReactiveUI;

namespace EpochEditor.Gui.ViewModels;

public class GameStateViewModel : ReactiveObject
{
    private bool _shouldBeVisible;
    private readonly SramReactiveViewModel _sramReactiveViewModel;
    private readonly GameSlot? _gameSlot;

    public GameStateViewModel(SramReactiveViewModel sramReactiveViewModel, GameSlot? gameSlot) {
        this._sramReactiveViewModel = sramReactiveViewModel;
        this._gameSlot = gameSlot;
    }

    public Boolean ShouldBeVisible { get => _shouldBeVisible; set => this.RaiseAndSetIfChanged(ref _shouldBeVisible, value); }

    private GameSlot GetNotNullGameSlot()
    {
        return _gameSlot ?? throw new EpochEditorAvaloniaSetupException("Cannot perform action on game state with null slot.");
    }

    public int PartyMemberOneComboBoxIndex { 
        get
        {
            if (false == _sramReactiveViewModel?.SramIsLoading) {
                GameSlot gameSlot = GetNotNullGameSlot();
                var options = PartyMemberOptions;
                var selected = options.Where(pmo => pmo.Index == gameSlot.PartyMemberOne).SingleOrDefault();
                selected ??= options.First(pmo => null == pmo.Index);
                var result = options.IndexOf(selected);
                return result;
            }
            else {
                return default;
            }
        }
        set {
            if (false == this._sramReactiveViewModel.IsSlotUpdating) {
                var valueToStore = 0 > value || value > PartyMemberOptions.Count ? 0 : value;
                var selected = PartyMemberOptions[valueToStore];
                GameSlot gameSlot = GetNotNullGameSlot();

                if (gameSlot.PartyMemberOne != selected.Index) {
                    gameSlot.PartyMemberOne = selected.Index;
                    this.RaisePropertyChanged();
                    _sramReactiveViewModel.RaiseSlotChecksumChange();
                }
            }
        }
    }

    public int PartyMemberTwoComboBoxIndex { 
        get
        {
            if (false == _sramReactiveViewModel?.SramIsLoading) {
                GameSlot gameSlot = GetNotNullGameSlot();
                var options = PartyMemberOptions;
                var selected = options.Where(pmo => pmo.Index == gameSlot.PartyMemberTwo).SingleOrDefault();
                selected ??= options.First(pmo => null == pmo.Index);
                var result = options.IndexOf(selected);
                return result;
            }
            else {
                return default;
            }
        }
        set {
            var valueToStore = 0 > value || value > PartyMemberOptions.Count ? 0 : value;
            var selected = PartyMemberOptions[valueToStore];
            GameSlot gameSlot = GetNotNullGameSlot();

            if (gameSlot.PartyMemberTwo != selected.Index) {
                gameSlot.PartyMemberTwo = selected.Index;
                this.RaisePropertyChanged();
                _sramReactiveViewModel.RaiseSlotChecksumChange();
            }
        }
    }

    public int PartyMemberThreeComboBoxIndex { 
        get
        {
            if (false == _sramReactiveViewModel?.SramIsLoading) {
                GameSlot gameSlot = GetNotNullGameSlot();
                var options = PartyMemberOptions;
                var selected = options.Where(pmo => pmo.Index == gameSlot.PartyMemberThree).SingleOrDefault();
                selected ??= options.First(pmo => null == pmo.Index);
                var result = options.IndexOf(selected);
                return result;
            }
            else {
                return default;
            }
        }
        set {
            var valueToStore = 0 > value || value > PartyMemberOptions.Count ? 0 : value;
            var selected = PartyMemberOptions[valueToStore];
            GameSlot gameSlot = GetNotNullGameSlot();

            if (gameSlot.PartyMemberThree != selected.Index) {
                gameSlot.PartyMemberThree = selected.Index;
                this.RaisePropertyChanged();
                _sramReactiveViewModel.RaiseSlotChecksumChange();
            }
        }
    }

    public List<PartyMemberOption> PartyMemberOptions4Bind { get => PartyMemberOptions; } 

    public List<PartyMemberOption> PartyMemberOptions {
        get {
            if (false == _sramReactiveViewModel?.SramIsLoading) {
                List<PartyMemberOption> result = new List<PartyMemberOption>();
                result.Add(new PartyMemberOption { Index = null });
                var optionsForCharacters = GetNotNullGameSlot().CharacterSheets.Select((cs, i) => new PartyMemberOption { Index = (Byte) i, Name = cs.Name } );
                result.AddRange(optionsForCharacters);
                return result;
            }
            else {
                return [];
            }
        }
    }

    public Byte? SaveCount {
        get => _gameSlot?.SaveCount;
        set {
            if (null != _gameSlot && value != _gameSlot.SaveCount) {
                _gameSlot.SaveCount = value;
                this.RaisePropertyChanged();
                this._sramReactiveViewModel.RaiseSlotChecksumChange();
            }
        }
    }

    public UInt32 GOLD_MAX { get => SramConstants.GOLD_MAX; }

    public UInt32? Gold {
        get => _gameSlot?.Gold;
        set {
            if (null != _gameSlot && value != _gameSlot.Gold) {
                _gameSlot.Gold = value;
                this.RaisePropertyChanged();
                this._sramReactiveViewModel.RaiseSlotChecksumChange();
            }
        }
    }

    public String TotalPlayTime {
        get => "--:--";
    }

    public UInt16? World { 
        get => _gameSlot?.World;
        set {
            if (null != _gameSlot && value != _gameSlot.World) {
                _gameSlot.World = value;
                this.RaisePropertyChanged();
                this._sramReactiveViewModel.RaiseSlotChecksumChange();
            }
        }
    }

    public Byte? PlayerX {
        get => _gameSlot?.PlayerX;
        set {
            if (null != _gameSlot && value != _gameSlot.PlayerX) {
                _gameSlot.PlayerX = value;
                this.RaisePropertyChanged();
                this._sramReactiveViewModel.RaiseSlotChecksumChange();
            }
        }
    }

    public Byte? PlayerY {
        get => _gameSlot?.PlayerY;
        set {
            if (null != _gameSlot && value != _gameSlot.PlayerY) {
                _gameSlot.PlayerY = value;
                this.RaisePropertyChanged();
                this._sramReactiveViewModel.RaiseSlotChecksumChange();
            }
        }
    }
}
