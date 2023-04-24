using System;
using DefaultNamespace;
using UnityEngine;

[Serializable]
public class Drifter
{
    [Header("Drifting Settings")]
    [SerializeField] private float driftSteeringPower = 8f;
    [SerializeField] private float driftDrag = 0.8f;
    [SerializeField] public float driftAccelerationSpeed = 17f;
    [SerializeField] private float miniDriftEarnedPerSecond = 5f;
    [SerializeField] private float maxDriftEarnedPerSecond = 23f;
    [SerializeField] private float driftBoostDuration = 1f;
    [SerializeField] private AnimationCurve driftBoostCurve;
    
    private float _driftPower, _boostTimeElapsed;

    public float defaultDrag;
    
    private int _driftDir;
    private bool StartedDrift => Input.GetButtonDown("Jump") && Input.GetAxis("Horizontal") != 0 && !_isDrifting;
    private static bool IsHoldingDriftButton => Input.GetButton("Jump");
    private static bool EndedDrift => Input.GetButtonUp("Jump");

    private bool _isDrifting;

    public bool IsDrifting => _isDrifting;
    
    private float _driftBoost;

    private Rigidbody _sphere;

    public Rigidbody Sphere
    {
        set => _sphere = value;
    }

    
    /// <summary>
    /// Calculates the current drift boost. This should be called from fixed update.
    /// </summary>
    /// <returns>Returns 0 if no boost is active.</returns>
    public float GetCurrentDriftBoost() {
        
        float fromBoost = 0;
        if (_boostTimeElapsed < driftBoostDuration)
        {
            float t = _boostTimeElapsed / driftBoostDuration;
            fromBoost = driftBoostCurve.Evaluate(t) * _driftBoost;
            _boostTimeElapsed += Time.deltaTime;
        }
        
        return fromBoost;
    }
    
    
    /// <summary>
    /// Handles all the drifting. This should be called from update
    /// </summary>
    /// <param name="steer">The steering that should be applied if it is drifting. If not drifting then this is 0</param>
    /// <returns>If it is drifting</returns>
    public bool HandleDrifting(out float steer)
    {
        steer = 0;
        if (EndedDrift)
        {
            EndDrift();
            return false;
        }
        
        if (!IsHoldingDriftButton )
            return false;
        
        if (StartedDrift)
            StartDrift();
        else if (!_isDrifting)
            return false;


        float horizontalInput = Input.GetAxis("Horizontal");
        
        float currentDriftPower = (_driftDir == 1) ? horizontalInput.Map( -1, 1, 0, 2) : horizontalInput.Map(-1, 1, 2, 0);
        _driftPower += (_driftDir == 1) 
            ? horizontalInput.Map( -1, 1, miniDriftEarnedPerSecond, maxDriftEarnedPerSecond) * Time.deltaTime
            : horizontalInput.Map(-1, 1, maxDriftEarnedPerSecond, miniDriftEarnedPerSecond) * Time.deltaTime;

        steer = driftSteeringPower * _driftDir * currentDriftPower;
        return true;
    }

    private void EndDrift()
    {
        _isDrifting = false;
        _sphere.drag = defaultDrag;

        _driftBoost = _driftPower;
        _boostTimeElapsed = 0f;
    }
    
    private void StartDrift()
    {
        //Horizontal is somewhere between -1 and 1, to make it be between 0 and 2 we add 1
        _driftDir = Input.GetAxis("Horizontal") < 0 ? -1 : 1;

        _isDrifting = true;

        _driftPower = 0f;
        
        _sphere.drag = driftDrag;
    }
    
}