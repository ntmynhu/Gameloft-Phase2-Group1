using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakeDamageEventListener : MonoBehaviour
{
    [SerializeField] private UnityEvent<float, string, string> EventResponse;
    [SerializeField] private TakeDamagePublisherSO publisher;

    private void OnEnable()
    {
        publisher.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        publisher.OnEventRaised -= Respond;
    }

    private void Respond(float value, string tag, string myName)
    {
        EventResponse?.Invoke(value, tag, myName);
    }
}