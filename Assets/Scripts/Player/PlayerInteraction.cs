using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _checkRate = 0.05f;
    [SerializeField] private float _maxCheckDistance;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _curInteractGameObject;
    [SerializeField] private IInteractable _curInteractable;
    [SerializeField] private TextMeshProUGUI _promptText;

    private Camera _camera;
    private float _lastCheckTime;
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - _lastCheckTime > _checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _maxCheckDistance, _layerMask))
            {
                if (hit.collider.gameObject != _curInteractGameObject)
                {
                    _curInteractGameObject = hit.collider.gameObject;
                    _curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                _curInteractGameObject = null;
                _curInteractable = null;
                _promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        _promptText.gameObject.SetActive(true);
        _promptText.text = _curInteractable.GetInteractPromp();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && _curInteractable != null)
        {
            _curInteractable.OnInteract();
            _curInteractGameObject = null;
            _curInteractable = null;
            _promptText.gameObject.SetActive(false);
        }
    }
}
