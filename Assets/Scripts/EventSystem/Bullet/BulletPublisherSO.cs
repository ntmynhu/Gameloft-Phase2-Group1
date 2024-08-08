using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Bullet Pulisher", menuName = "Scriptable Objects/Events/Bullet Publisher")]
public class BulletPublisherSO : ScriptableObject
{
    public UnityAction<Bullet, string, int> OnEventRaised;

    public void RaiseEvent(Bullet obj, string tag, int name)
    {
        OnEventRaised?.Invoke(obj, tag, name);
    }
}