using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public event Action<Player> OnInteractedByPlayer;


    [Header("Settings")] 
    [SerializeField] private float observeRadius = 3f;

    private List<Player> _subscribeToPlayerList = new List<Player>();
    private SphereCollider _observeSphereCollider;

    [Header("Debug")] 
    [SerializeField] private bool showDebug = false;


    private void Awake() {
        _observeSphereCollider = ColliderCreator.CreateSphereCollider(gameObject, true, observeRadius);
    }


    private void OnPlayerInteract(Player player) {
        if (IsClosestInteractToPlayer(player) == false) return; 
        
        OnInteractedByPlayer?.Invoke(player);
        
        if (showDebug == true) {
            Debug.Log(gameObject.name + "." + this.name + 
                      ": " + player.name + " interacts");            
        }
    }
    
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent(out Player player) == false) return;

        player.OnPlayerPressedInteractButton += OnPlayerInteract;
        _subscribeToPlayerList.Add(player);
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.TryGetComponent(out Player player) == false) return;

        player.OnPlayerPressedInteractButton -= OnPlayerInteract;
        _subscribeToPlayerList.Remove(player);
    }


    private bool IsClosestInteractToPlayer(Player player) { 
        List<Vector3> interactObjectList = new List<Vector3>();
        
        Collider[] colliderArray = Physics.OverlapSphere(player.transform.position, observeRadius);
        
        foreach (Collider candidate in colliderArray) {
            if (candidate == _observeSphereCollider) continue;

            if (candidate.gameObject.TryGetComponent(out Interact interact))
                interactObjectList.Add(candidate.transform.position);
        }

        return Dotting.IsClosestPointTo(interactObjectList, transform.position, player.transform.position);
    }


    private void OnDestroy() {
        foreach (var player in _subscribeToPlayerList) {
            player.OnPlayerPressedInteractButton -= OnPlayerInteract;
        }
        
        if (OnInteractedByPlayer != null) {
            foreach (var d in OnInteractedByPlayer.GetInvocationList())
                OnInteractedByPlayer -= (d as Action<Player>);
        }
    }
    
    
    private void OnDrawGizmos() {
        if (showDebug == false) return;
        
        Gizmos.DrawWireSphere(transform.position, observeRadius);
    }
    
    
}
