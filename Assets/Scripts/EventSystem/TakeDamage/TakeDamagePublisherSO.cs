using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New TakeDamage Pulisher", menuName = "Scriptable Objects/Events/TakeDamage Publisher")]
public class TakeDamagePublisherSO : ScriptableObject
{
    public UnityAction<float, string, string> OnEventRaised;

    public void RaiseEvent(float value, string tag, string myName)
    {
        OnEventRaised?.Invoke(value, tag, myName);
    }
}