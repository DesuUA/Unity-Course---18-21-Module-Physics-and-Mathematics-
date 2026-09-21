using UnityEngine;
using UnityEngine.Serialization;

public class Pointer : MonoBehaviour
{
    [SerializeField] private KeyCode _grabKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _bombKey = KeyCode.Mouse1;
    
    [SerializeField] private float _offsetFromCamera = 5f;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _minScale = 2f;
    [SerializeField] private float _maxScale = 20f;
    
    private const string ScrollAxis = "MouseScrollWheel";

    private bool _objectGrabbed;
    private IGrabbable _grabbedObject;
    private Transform _grabHoldPoint;
    
    private void Start()
    {
        GameObject holdPointObject = new GameObject("GrabHoldPoint");
        _grabHoldPoint = holdPointObject.transform;
    }
    private void Update()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        _grabHoldPoint.position = cameraRay.origin + cameraRay.direction * _offsetFromCamera;
        
        if (Input.GetKeyDown(_grabKey))
        {
            if (_objectGrabbed == false)
            {
                if (Physics.Raycast(cameraRay, out RaycastHit hit))
                {
                    _grabbedObject = hit.collider.GetComponent<IGrabbable>();
                    if (_grabbedObject != null)
                    {
                        _grabbedObject.Grab(_grabHoldPoint);
                        _objectGrabbed = true;
                    }
                }
            }
            else
            {
                _grabbedObject.Grab(null);
                _grabbedObject = null;
                _objectGrabbed = false;
            }
        }
        
        float scrollInput = Input.GetAxis(ScrollAxis);

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            ZoomObject(scrollInput);
        }
    }

    private void ZoomObject(float input)
    {
        _offsetFromCamera += input * _zoomSpeed;
        _offsetFromCamera = Mathf.Clamp(_offsetFromCamera, _minScale, _maxScale);
    }
}
