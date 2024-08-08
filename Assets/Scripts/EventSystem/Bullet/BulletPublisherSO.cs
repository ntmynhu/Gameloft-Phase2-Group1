using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Bullet Pulisher", menuName = "Scriptable Objects/Events/Bullet Publisher")]
public class BulletPublisherSO : ScriptableObject
{
    public UnityAction<Bullet, string> OnEventRaised;

    public void RaiseEvent(Bullet obj, string tag)
    {
        OnEventRaised?.Invoke(obj, tag);
    }
}