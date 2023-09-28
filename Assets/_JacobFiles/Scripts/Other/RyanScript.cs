using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyanScript : MonoBehaviour
{
    [SerializeField] private WaypointPath _waypointPath;
    [SerializeField] private float _speed;

    private int _targetWaypointIndex;
    private Transform _previousWaypoint;
    private Transform _targetWaypoint;
    private float _timeToWaypoint;
    private float _elapsedTime;

    // Store references to rigidbodies that were on the platform
    private HashSet<Rigidbody> _rigidbodiesOnPlatform = new HashSet<Rigidbody>();

    void Start()
    {
        TargetNextWaypoint();
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);

        // Move the platform
        transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }

        // Move the rigidbodies along with the platform
        foreach (var rb in _rigidbodiesOnPlatform)
        {
            rb.MovePosition(rb.position + (transform.position - _previousWaypoint.position));
        }
    }

    private void TargetNextWaypoint()
    {
        _previousWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);
        _targetWaypointIndex = _waypointPath.GetNextWaypointIndex(_targetWaypointIndex);
        _targetWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);

        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb))
        {
            // Parent the rigidbody to the platform
            rb.transform.SetParent(transform);
            _rigidbodiesOnPlatform.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb))
        {
            // Unparent the rigidbody from the platform
            rb.transform.SetParent(null);
            _rigidbodiesOnPlatform.Remove(rb);
        }
    }
}
