using UnityEngine;

public class PointerInputHandler : MonoBehaviour
{
    [Header("Input Keybindings")]
    [SerializeField] private KeyCode _grabKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _bombKey = KeyCode.Mouse1;

    [Header("Zoom Settings")]
    [SerializeField] private float _offsetFromCamera = 5f;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _minScale = 2f;
    [SerializeField] private float _maxScale = 20f;

    [Header("Abilities Settings")]
    [SerializeField] private BombExploder.Settings _bombSettings;

    private const string ScrollAxis = "MouseScrollWheel";

    private IRayProvider _rayProvider;
    private PointerController _controller;

    private void Awake()
    {
        _rayProvider = new ScreenMouseRayProvider();
        
        _controller = new PointerController(
            _bombSettings, 
            _offsetFromCamera, 
            _zoomSpeed, 
            _minScale, 
            _maxScale
        );
    }

    private void Update()
    {
        Ray currentRay = _rayProvider.GetRay();

        _controller.UpdateHoldPoint(currentRay);

        if (Input.GetKeyDown(_grabKey))
            _controller.PerformGrab(currentRay);

        if (Input.GetKeyDown(_bombKey))
            _controller.PerformBomb(currentRay);

        float scrollInput = Input.GetAxis(ScrollAxis);
        if (Mathf.Abs(scrollInput) > 0.01f)
            _controller.Zoom(scrollInput);
    }
}
