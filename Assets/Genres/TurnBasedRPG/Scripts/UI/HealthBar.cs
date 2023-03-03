using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class HealthBar : MonoBehaviour
{
    private const float ANIMATION_DURATION = 0.5f;
    [SerializeField] private RectTransform _bar;
    private Image _barImage;
    private readonly (float cutoff, string color) GREEN_BAR = (1f, "#7CFF81");
    private readonly (float cutoff, string color) YELLOW_BAR = (0.5f, "#EBA734");
    private readonly (float cutoff, string color) RED_BAR = (0.2f, "#E46342");
    private float? _originalSizeDelta = null;
    private CoroutineHandle _handle;

    public void SetHealthBar(float percentage, bool isInstant = false)
    {
        if (!_originalSizeDelta.HasValue) _originalSizeDelta = _bar.sizeDelta.x;
        _barImage = _bar.GetComponent<Image>();
        Timing.KillCoroutines(_handle);
        _handle = Timing.RunCoroutine(_SetHealthBar(percentage, isInstant));
    }

    private IEnumerator<float> _SetHealthBar(float percentage, bool isInstant)
    {
        if (isInstant)
        {
            _bar.sizeDelta = new Vector2(_originalSizeDelta.Value * percentage, _bar.sizeDelta.y);
            SetBarColor(_bar.sizeDelta.x / _originalSizeDelta.Value);
            yield break;
        }

        float previousLength = _bar.sizeDelta.x;
        for (float t = 0f; t < 1f; t += Timing.DeltaTime / ANIMATION_DURATION) {
            _bar.sizeDelta = new Vector2(Mathf.SmoothStep(previousLength, _originalSizeDelta.Value * percentage, t) , _bar.sizeDelta.y);
            SetBarColor(_bar.sizeDelta.x / _originalSizeDelta.Value);

            yield return Timing.WaitForOneFrame;
        }
    }

    private void SetBarColor(float currentPercent)
    {
        string choosen = GREEN_BAR.color;
        if (currentPercent <= YELLOW_BAR.cutoff && currentPercent > RED_BAR.cutoff)
        {
            choosen = YELLOW_BAR.color;
        }
        else if (currentPercent <= RED_BAR.cutoff)
        {
            choosen = RED_BAR.color;
        }

        if (ColorUtility.TryParseHtmlString(choosen, out Color color))
        {
            _barImage.color = color;
        }
    }
}
