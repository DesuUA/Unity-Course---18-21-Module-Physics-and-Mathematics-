using UnityEngine;

public class ScreenMouseRayProvider : IRayProvider
{
    private Camera _mainCamera;

    public Ray GetRay()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;

        return _mainCamera.ScreenPointToRay(Input.mousePosition);
    }
}
