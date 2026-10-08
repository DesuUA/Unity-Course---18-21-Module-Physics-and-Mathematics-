using UnityEngine;

public class PointerController
{
    private readonly Transform _grabHoldPoint;
    private readonly IPointerAction _grabberAction;
    private readonly IPointerAction _bombAction;
    
    private float _offsetFromCamera;
    private readonly float _zoomSpeed;
    private readonly float _minScale;
    private readonly float _maxScale;

    public PointerController(BombExploder.Settings bombSettings, float initialOffset, float zoomSpeed, float minScale, float maxScale)
    {
        _offsetFromCamera = initialOffset;
        _zoomSpeed = zoomSpeed;
        _minScale = minScale;
        _maxScale = maxScale;

        GameObject holdPointObject = new GameObject("[GrabHoldPoint]");
        _grabHoldPoint = holdPointObject.transform;

        _grabberAction = new ObjectGrabber(_grabHoldPoint);
        _bombAction = new BombExploder(bombSettings);
    }

    public void UpdateHoldPoint(Ray ray)
    {
        _grabHoldPoint.position = ray.origin + ray.direction * _offsetFromCamera;
    }

    public void PerformGrab(Ray ray) => _grabberAction.Execute(ray);
    
    public void PerformBomb(Ray ray) => _bombAction.Execute(ray);

    public void Zoom(float scrollDelta)
    {
        _offsetFromCamera += scrollDelta * _zoomSpeed;
        _offsetFromCamera = Mathf.Clamp(_offsetFromCamera, _minScale, _maxScale);
    }
}
