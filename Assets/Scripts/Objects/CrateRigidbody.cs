using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CrateRigidbody : MonoBehaviour , IExplodable, IGrabbable
{
    private Rigidbody _rigidbody;
    private Transform _grabber;
    private bool _isGrabbed;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isGrabbed)
            transform.position = _grabber.position;
    }
    
    public void ExplodeDirection(Vector3 direction, float force)
    {
        _rigidbody.AddForce(Vector3.Normalize(direction) * force, ForceMode.Impulse);
    }

    public void Grab(Transform grabber)
    {
        if (_isGrabbed)
        {
            return;
        }

        _grabber = grabber;
        _isGrabbed = true;
        _rigidbody.isKinematic = true;
    }

    public void Ungrab()
    {
        _grabber = null;
        _isGrabbed = false;
        _rigidbody.isKinematic = false;
    }
}
