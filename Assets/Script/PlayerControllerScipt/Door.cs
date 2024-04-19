using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float doorSpeed = 1f; // Vitesse à laquelle la porte s'ouvre
    [SerializeField] float doorDistance = 2f; // Distance que la porte parcourt lorsqu'elle s'ouvre
    bool opened = false; // Etat de la porte
    Vector3 closedPosition; // Position de la porte lorsqu'elle est fermée

    void OnEnable() => Interactable.DoorOpen += ToggleDoor;
    void OnDisable() => Interactable.DoorOpen -= ToggleDoor;

    void Start()
    {
        closedPosition = transform.position; // Enregistre la position initiale de la porte
    }

    void Update()
    {
        // Calcule la position cible de la porte
        Vector3 targetPosition = opened ? closedPosition + transform.right * doorDistance : closedPosition;

        // Déplace la porte vers sa position cible à la vitesse spécifiée
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, doorSpeed * Time.deltaTime);
    }

    void ToggleDoor()
    {
        opened = !opened; // Change l'état de la porte
    }
}
