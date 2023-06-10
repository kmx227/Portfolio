using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emergency : MonoBehaviour
{
    [SerializeField] private bool _permitEmergency = true;
    [SerializeField] private int _totalEmergencyCount = 2;
    [SerializeField] private int _currentEmergencyCount = 0;

    public bool PermitEmergency { get => _permitEmergency; set => _permitEmergency = value; }

    public void UseEmergency(bool _use)
    {
        if (_use)
        {
            if (_currentEmergencyCount < _totalEmergencyCount && _permitEmergency)
            {

                gameObject.GetComponent<Outline>().eraseRenderer = true;
                _currentEmergencyCount++;
                _permitEmergency = false;

            }
        }
        else
        {
            if(_currentEmergencyCount < _totalEmergencyCount)
            {
                _permitEmergency = true;
                gameObject.GetComponent<Outline>().eraseRenderer = false;
            }
        }
    }
}
