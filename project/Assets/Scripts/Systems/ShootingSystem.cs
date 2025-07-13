using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    [Header("Dependecies")]
    [SerializeField] private Transform _shootOrigin;
    [SerializeField] private LayerMask _hitLayers;

    [Header("Fire Modes")]
    [SerializeField] private bool _isAutomatic = false;
    [SerializeField, Min(0f)] private float _autoFireRate = 0.1f;
    private bool _isFiring;
    private float _nextFireTime;

    [Header("Raycast Settings")]
    [SerializeField, Min(0f)] private float _range = 100f;
    [SerializeField, Min(1)] private int _damage = 10;

    [Header("Visual Feedback")]
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private LineRenderer _bulletTracePrefab;
    [SerializeField, Min(0f)] private float _tracerDuration = .05f;

    void Update()
    {

    }

    public void StartFiring()
    {
        if (_isAutomatic)
        {
            _isFiring = true;
            _nextFireTime = Time.time;
        }
        else
        {
            Fire();
        }
    }

    public void StopFiring()
    {
        _isFiring = false;
    }

    private void Fire()
    {
        _muzzleFlash?.Play();

        Ray ray = new Ray(_shootOrigin.position, _shootOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _range, _hitLayers, QueryTriggerInteraction.Ignore))
        {
            SpawnTracer(_shootOrigin.position, hit.point);

            if (hit.collider.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(_damage, hit.point, hit.normal);
            }
        }
        else
        {
            SpawnTracer(_shootOrigin.position, _shootOrigin.position + _shootOrigin.forward * _range);
        }
    }

    private void SpawnTracer(Vector3 start, Vector3 end)
    {
        if (_bulletTracePrefab == null) return;

        var tracer = Instantiate(_bulletTracePrefab);
        tracer.positionCount = 2;
        tracer.SetPosition(0, start);
        tracer.SetPosition(1, end);
        Destroy(tracer.gameObject, _tracerDuration);
    }

    public void SetCanShoot(bool canShoot)
    {
        _isFiring = false;
        enabled = canShoot;
    }

    public void ConfigureFireMode(bool isAutomatic, float fireRate = 0.1f)
    {
        _isAutomatic = isAutomatic;
        _autoFireRate = Mathf.Max(0f, fireRate);
    }
}
