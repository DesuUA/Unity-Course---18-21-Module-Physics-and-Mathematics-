using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Interact : MonoBehaviour
{
    [SerializeField] private KeyCode _interactKey = KeyCode.F;
    [SerializeField] private GrabPosition _grabPosition;
    
    private InteractableItem _interactableItem;
    private bool _isEmpty = true;
    
    private void Update()
    { 
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(_interactKey) && _isEmpty == false)
        {
            _interactableItem.Interact();
            _interactableItem.transform.SetParent(null);
            _isEmpty = true;
        }
        else if (Input.GetKeyDown(_interactKey) && _isEmpty) 
            Debug.Log("Nothing to interact with");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isEmpty)
            if (other.TryGetComponent<InteractableItem>(out _interactableItem))
            {
                _isEmpty = false;
                ProcessSnapTo();
            }
    }

    private void ProcessSnapTo()
    {
        _interactableItem.transform.SetParent(_grabPosition.gameObject.transform);
        _interactableItem.transform.localPosition = Vector3.zero;
        _interactableItem.Grabbed = true;
    }
}
