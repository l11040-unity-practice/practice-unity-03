using UnityEngine;

public class MusicZone : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _maxVolume;
    private float _targetVolumn;

    private void Start()
    {
        _targetVolumn = 0;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _targetVolumn;
        _audioSource.Play();
    }

    private void Update()
    {
        if (!Mathf.Approximately(_audioSource.volume, _targetVolumn))
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _targetVolumn, (_maxVolume / _fadeTime) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _targetVolumn = _maxVolume;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _targetVolumn = 0;
        }
    }
}
