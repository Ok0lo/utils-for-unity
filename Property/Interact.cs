using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public event Action<Player> OnInteractedByPlayer;


    [Header("Settings")] 
    [SerializeField] private float observeRadius;
    private SphereCollider _observeSphereCollider;
    

    private void Awake() {
        _observeSphereCollider = ColliderCreator.CreateSphereCollider(gameObject, true, observeRadius);
    }


    private void OnPlayerInteract(Player player) {
        OnInteractedByPlayer?.Invoke(player);
    }
    
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent(out Player player) == false) return;

        player.OnPlayerPressedInteractButton += OnPlayerInteract;
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.TryGetComponent(out Player player) == false) return;

        player.OnPlayerPressedInteractButton -= OnPlayerInteract;
    }
    
    private bool IsClosestInteractToPlayer(Player player) { 
        List<Vector3> interactObjectList = new List<Vector3>();
        
        Collider[] colliderArray = (Collider[]) Physics.OverlapSphere(player.transform.position, observeRadius).Except(new[] { _observeSphereCollider } );
        
        foreach (Collider candidate in colliderArray)
            if (candidate.gameObject.TryGetComponent(out Interact interact)) interactObjectList.Add(candidate.transform.position);

        return Dotting.IsClosestPointTo(interactObjectList, transform.position, player.transform.position);
    }
    
}
