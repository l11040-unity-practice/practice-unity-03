using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
  [SerializeField] Image _image;
  [SerializeField] float _flashSpeed;
  Color _color;
  Coroutine _coroutine;

  private void Start()
  {
    _color = _image.color;
    CharacterManager.Instance.Player.Condition.OnDamage += Flash;
  }
  public void Flash()
  {
    if (_coroutine != null)
    {
      StopCoroutine(_coroutine);
    }
    _image.enabled = true;
    _image.color = _color;
    _coroutine = StartCoroutine(FadeAway());
  }


  IEnumerator FadeAway()
  {
    float startAlpha = 0.3f;
    float a = startAlpha;
    Color color = _color;

    while (a > 0)
    {
      a -= (startAlpha / _flashSpeed) * Time.deltaTime;
      color.a = a;
      _image.color = color;
      yield return null;
    }
    _image.enabled = false;
  }
}
