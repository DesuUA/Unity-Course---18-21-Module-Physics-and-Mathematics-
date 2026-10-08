using UnityEngine;

public class ObjectGrabber : IPointerAction
{
    private IGrabbable _grabbedObject;
    private bool _objectGrabbed;
    private readonly Transform _grabHoldPoint;

    public ObjectGrabber(Transform grabHoldPoint)
    {
        _grabHoldPoint = grabHoldPoint;
    }
    
    public void Execute(Ray cameraRay)
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
            _grabbedObject.Ungrab();
            _grabbedObject = null;
            _objectGrabbed = false;
        }
    }
}
