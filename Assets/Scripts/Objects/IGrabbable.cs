using UnityEngine;

public interface IGrabbable
{
    public void Grab(Transform grabber);
    
    public void Ungrab();
}
