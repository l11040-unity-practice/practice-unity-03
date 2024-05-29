using UnityEngine;

public class PlayerFootStep : MonoBehaviour
{
    [SerializeField] private AudioClip[] _footStepClips;
    [SerializeField] private float _footStepThreshold;
    [SerializeField] private float _footStepRate;
    private float _footStepTime;
    private AudioSource _audioSource;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
        {
            if (_rigidbody.velocity.magnitude > _footStepThreshold)
            {
                if (Time.time - _footStepTime > _footStepRate)
                {
                    _footStepTime = Time.time;
                    _audioSource.PlayOneShot(_footStepClips[Random.Range(0, _footStepClips.Length)]);
                }
            }
        }
    }
}
