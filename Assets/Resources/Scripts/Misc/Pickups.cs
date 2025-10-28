using System;
using UnityEngine;

public class Pickups : Interactable
{
    [SerializeField] private float defaultPossessionTimer;

    public event EventHandler<OnInteractEventArgs> OnInteract;

    public class OnInteractEventArgs : EventArgs
    {
        public float possessionTimer;
    }

    protected override void Interact()
    {
        OnInteract?.Invoke(this, new OnInteractEventArgs { possessionTimer = defaultPossessionTimer });
        DestroyImmediate(gameObject);
    }
}
