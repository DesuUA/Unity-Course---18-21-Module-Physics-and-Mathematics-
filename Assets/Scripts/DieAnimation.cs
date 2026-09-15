using UnityEngine;

public class DieAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystemPrefab;
    [SerializeField] private float _verticalOffset = 0.5f;
    
    private IDie _die;
    
    private void Start()
    {
        _die = GetComponent<IDie>();
        if (_die == null)
            Debug.LogError($"Interface {_die} not found on {gameObject.name}");
    }

    private void Update()
    {
        if (_die.Die)
            Instantiate(_particleSystemPrefab, transform.position + new Vector3(0, _verticalOffset, 0), transform.rotation);
    }
}