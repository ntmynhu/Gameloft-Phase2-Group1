using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletEventListener : MonoBehaviour
{
    [SerializeField] private UnityEvent<Bullet, string> EventResponse;
    [SerializeField] private BulletPublisherSO publisher;

    private void OnEnable()
    {
        publisher.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        publisher.OnEventRaised -= Respond;
    }

    private void Respond(Bullet obj, string tag)
    {
        EventResponse?.Invoke(obj, tag);
    }
}