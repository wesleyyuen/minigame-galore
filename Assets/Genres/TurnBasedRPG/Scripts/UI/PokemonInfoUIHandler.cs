using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PokemonInfoUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Transform _spriteParent;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _levelText;
    private Pokemon _pokemon;

    public void SetupUI(Pokemon pokemon)
    {
        _pokemon = pokemon;
        _pokemon.Event_TookDamage += SetHealthBar;

        SetName();
        SetLevel();
        SetSprite();
        InitHealthBar();
    }

    public void RemoveListener()
    {
        _pokemon.Event_TookDamage -= SetHealthBar;
    }

    private void SetName()
    {
        _name.text = _pokemon.Name;
    }

    private void SetLevel()
    {
        _levelText.text = $"lv.{_pokemon.Level}";
    }

    private void SetSprite()
    {
        if (_spriteParent == null) return;

        // Destroy All Child
        foreach (Transform child in _spriteParent)
        {
            Destroy(child.gameObject);
        }

        GameObject sprite = Instantiate(_pokemon.Species.Sprite, Vector3.zero, Quaternion.identity, _spriteParent);
        sprite.transform.localPosition = Vector3.zero;
        
        var trainer = _pokemon.Trainer;
        if (trainer != null && trainer.IsPlayer && sprite.TryGetComponent<Animator>(out var animator))
        {
            animator.SetBool("Back", true);
        }
    }

    public void SetHealthBar(Pokemon pokemon)
    {
        _healthBar.SetHealthBar(_pokemon.CurrentHP / _pokemon.Stat.HP);
        SetHealthBarText();
    }

    private void InitHealthBar()
    {
        _healthBar.SetHealthBar(_pokemon.CurrentHP / _pokemon.Stat.HP, true);
        SetHealthBarText();
    }

    private void SetHealthBarText()
    {
        if (_hpText == null) return;
        _hpText.text = $"{Mathf.CeilToInt(_pokemon.CurrentHP)}/{Mathf.CeilToInt(_pokemon.Stat.HP)}";
    }
}