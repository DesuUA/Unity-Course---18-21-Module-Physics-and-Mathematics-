using System;
using UnityEngine;

public class BombExploder : IPointerAction
{
    [Serializable]
    public struct Settings
    {
        public ParticleSystem particleSystemPrefab;
        public LayerMask groundLayer;
        public LayerMask objectsLayer;
        public float radius;
        public float explodeForce;
    }

    private readonly Settings _settings;

    public BombExploder(Settings settings)
    {
        _settings = settings;
    }

    public void Execute(Ray cameraRay)
    {
        if (Physics.Raycast(cameraRay, out RaycastHit hit, Mathf.Infinity, _settings.groundLayer) == false)
            return;

        if (_settings.particleSystemPrefab != null)
        {
            UnityEngine.Object.Instantiate(_settings.particleSystemPrefab, hit.point, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(hit.point, _settings.radius, _settings.objectsLayer);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IExplodable explodable))
            {
                Vector3 direction = collider.transform.position - hit.point;
                explodable.ExplodeDirection(direction, _settings.explodeForce);
            }
        }
    }
}