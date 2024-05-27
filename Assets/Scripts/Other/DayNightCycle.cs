using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField][Range(0.0f, 1.0f)] private float _time;
    [SerializeField] private float _fullDayLength;
    [SerializeField] private float _startTime = 0.4f;
    [SerializeField] private Vector3 _moon;
    private float _timeRate;

    [Header("Sun")]
    [SerializeField] private Light _sunLight;
    [SerializeField] private Gradient _sunColor;
    [SerializeField] private AnimationCurve _sunIntensity;

    [Header("Moon")]
    [SerializeField] private Light _moonLight;
    [SerializeField] private Gradient _moonColor;
    [SerializeField] private AnimationCurve _moonIntensity;

    [Header("Other Lighting")]
    [SerializeField] private AnimationCurve _lightingIntensityMultiplier;
    [SerializeField] private AnimationCurve _reflectionIntensityMultiplier;

    private void Start()
    {
        _timeRate = 1.0f / _fullDayLength;
        _time = _startTime;
    }

    private void Update()
    {
        _time = (_time + _timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(_sunLight, _sunColor, _sunIntensity);
        UpdateLighting(_moonLight, _moonColor, _moonIntensity);

        RenderSettings.ambientIntensity = _lightingIntensityMultiplier.Evaluate(_time);
        RenderSettings.reflectionIntensity = _reflectionIntensityMultiplier.Evaluate(_time);
    }

    private void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(_time);
        lightSource.transform.eulerAngles = (_time - (lightSource == _sunLight ? 0.25f : 0.75f)) * _moon * 4f;
        lightSource.color = gradient.Evaluate(_time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
