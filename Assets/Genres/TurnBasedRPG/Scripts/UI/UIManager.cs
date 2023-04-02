using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using Zenject;
using TurnBasedRPG.Input;

namespace TurnBasedRPG.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance {get; private set;}
        private static TextMeshProUGUI _battleText;
        [SerializeField] private PokemonInfoUIHandler _myPokemonHandler;
        [SerializeField] private PokemonInfoUIHandler _theirPokemonHandler;
        [SerializeField] private TextMeshProUGUI[] _moveTexts;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private RectTransform _moveGO;
        [SerializeField] private RectTransform _pokemonSelectScreen;
        private PokemonInfoUIHandler[] _pokemonSelections;
        [SerializeField] private RectTransform _itemSelectScreen;
        private ScrollRect _itemSelectionScrollRect;
        [SerializeField] private GameObject _itemInfoPanelPrefab;
        private CanvasGroup _canvasGroup;
        private TurnBasedRPGInput _input;
        private ActionSelection _currentSelection;
        private Trainer _player;
        private bool _isSelected;
        private bool _cannotBack;

        [Inject]
        public void Init(TurnBasedRPGInput input)
        {
            _input = input;
        }

        private void Awake()
        {
            // Singleton
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }

        private void Start()
        {
            // TODO: avoid hardcode path
            _battleText = transform.Find("MainBattleScreen/BottomPanel/Text")?.GetComponent<TextMeshProUGUI>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _pokemonSelections = _pokemonSelectScreen.GetComponentsInChildren<PokemonInfoUIHandler>();
            _itemSelectionScrollRect = _itemSelectScreen.GetComponentInChildren<ScrollRect>();
        }

        private void OnEnable()
        {
            _input.Event_Back += OnBack;
        }

        private void OnDisable()
        {
            _input.Event_Back -= OnBack;
        }

        public void OnBattleStart()
        {
            _pokemonSelectScreen.gameObject.SetActive(false);
            _canvasGroup.alpha = 1f;
        }

        public void OnBattleEnd()
        {
            _canvasGroup.alpha = 0f;
        }

        private void OnBack()
        {
            if (_isSelected || _cannotBack) return;
            switch (_currentSelection.Type)
            {
                case TurnActionType.Fight: _moveGO.gameObject.SetActive(false); break;
                case TurnActionType.Pokemon: _pokemonSelectScreen.gameObject.SetActive(false); break;
                case TurnActionType.Items: _itemSelectScreen.gameObject.SetActive(false); break;
                default: break;
            }
        }

        public void OnFightPressed()
        {
            if (_isSelected) return;
            _currentSelection.Type = TurnActionType.Fight;
            _moveGO.gameObject.SetActive(true);
        }

        public void OnPokemonPressed()
        {
            if (_isSelected) return;
            _currentSelection.Type = TurnActionType.Pokemon;
            SetupPokemonSelection();
            _pokemonSelectScreen.gameObject.SetActive(true);
        }

        public async UniTask<int> GetPlayerPokemonSelection()
        {
            _isSelected = false;
            _cannotBack = true;
            SetupPokemonSelection();
            _pokemonSelectScreen.gameObject.SetActive(true);
            await _GetPlayerActionSelection_1v1();
            _pokemonSelectScreen.gameObject.SetActive(false);
            return _currentSelection.Index;
        }

        private void SetupPokemonSelection()
        {
            if (_player == null) return;

            for (int i = 0; i < _pokemonSelections.Length; i++)
            {
                var uiHandler = _pokemonSelections[i];
                if (_player.PokemonsOnHand.Count > i)
                {
                    var curr = _player.PokemonsOnHand[i];
                    uiHandler.gameObject.SetActive(true);
                    uiHandler.SetupUI(curr);
                    if (uiHandler.TryGetComponent<Button>(out var button))
                    {
                        button.interactable = _player.PokemonInBattle != curr && !curr.IsFainted;
                    }
                }
                else uiHandler.gameObject.SetActive(false);
            }
        }

        public void OnPokemonSelected(int index)
        {
            if (_isSelected) return;
            _currentSelection.Type = TurnActionType.Pokemon;
            _currentSelection.Index = index;
            _isSelected = true;
        }

        public void OnPokemonSelectedFinished()
        {
            _pokemonSelectScreen.gameObject.SetActive(false);
        }

        public void OnItemsPressed()
        {
            if (_isSelected) return;
            _currentSelection.Type = TurnActionType.Items;
            SetupItemSelection();
            _itemSelectScreen.gameObject.SetActive(true);
        }

        private void SetupItemSelection()
        {
            if (_player == null) return;

            foreach (Transform child in _itemSelectionScrollRect.content)
            {
                Destroy(child.gameObject);
            }

            foreach (var (item, amount) in _player.Inventory)
            {
                var itemInfoPanel = Instantiate(_itemInfoPanelPrefab, Vector3.zero, Quaternion.identity, _itemSelectionScrollRect.content);
                if (itemInfoPanel.TryGetComponent<ItemInfoUIHandler>(out var handler))
                {
                    handler.SetUI(item, amount);
                }
                if (itemInfoPanel.TryGetComponent<Button>(out var button))
                {
                    button.onClick.AddListener(() => {
                        OnItemSelected(item.Name);
                    });
                }
            }
        }

        public void OnItemSelected(string name)
        {
            if (_isSelected) return;
            _currentSelection.Type = TurnActionType.Items;
            _currentSelection.Name = name;
            _isSelected = true;
        }

        public void OnItemSelectedFinished()
        {
            _itemSelectScreen.gameObject.SetActive(false);
        }

        public void OnRunPressed()
        {
            if (_isSelected) return;
            _currentSelection.Type = TurnActionType.Run;
            _currentSelection.Index = 0;
            _isSelected = true;
        }

        public void OnMovePressed(int index)
        {
            if (_isSelected) return;
            _currentSelection.Type = TurnActionType.Fight;
            _currentSelection.Index = index;
            _isSelected = true;
        }

        public async UniTask<ActionSelection> GetPlayerActionSelection_1v1()
        {
            _isSelected = false;
            _panel.gameObject.SetActive(true);
            _moveGO.gameObject.SetActive(false);
            SetBattleTextInstantly($"What will {_player.PokemonInBattle.Name} do?");
            await _GetPlayerActionSelection_1v1();
            _pokemonSelectScreen.gameObject.SetActive(false);
            SetPanelVisible(false);
            return _currentSelection;
        }

        private async UniTask _GetPlayerActionSelection_1v1()
        {
            await UniTask.WaitUntil(() => _isSelected);
            _cannotBack = false;
        }

        public void RemoveListener()
        {
            _myPokemonHandler.RemoveListener();
            _theirPokemonHandler.RemoveListener();
        }

        public void SetBattleTextInstantly(string text)
        {
            if (_battleText != null)
            {
                _battleText.text = text;
            }
        }

        public async UniTask SetBattleText(string text)
        {
            if (_battleText != null)
            {
                _battleText.text = text;
                if (text == "") return;
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.DIALOG_DURATION));
            }
        }

        public void SetPokemon(Trainer trainer)
        {
            Pokemon pkmn = trainer.PokemonInBattle ?? trainer.GetFirstAvailablePokemon();
            if (trainer.IsPlayer)
            {
                // TODO: avoid storing player reference
                _player = trainer;
                _myPokemonHandler.SetupUI(pkmn);
                SetMoveText(pkmn);
            }
            else
            {
                _theirPokemonHandler.SetupUI(pkmn);
            }
        }

        public void SetPokemon(Pokemon pokemon)
        {
            _theirPokemonHandler.SetupUI(pokemon);
        }

        public void SetMoveText(Pokemon pokemon)
        {
            for (int i = 0; i < Constants.MAX_MOVE_COUNT; i++)
            {
                Move move = pokemon.GetMoveByIndex(i);
                SetMoveTextHelper(i, move == null ? "" : move.Name);
            }
        }

        private void SetMoveTextHelper(int index, string name)
        {
            var text = _moveTexts[index];
            if (text == null) return;
            text.text = name ?? "";
            text.transform.parent.gameObject.SetActive(name != null && name != "");
        }

        public void SetPanelVisible(bool visible)
        {
            if (_panel == null) return;
            _panel.gameObject.SetActive(visible);
        }
    }

    public struct ActionSelection
    {
        public TurnActionType Type;
        public int Index;
        public string Name;
    }
}