using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Définition du type d'objet interactable
    [SerializeField] private string InteractableType;

    // Déclaration de l'événement DoorOpen
    public static event Action DoorOpen;

    // Méthode pour interagir avec l'objet
    public void Interact()
    {
        Debug.Log("l'objet a été interacté");

        // Selon le type d'objet interactable, on appelle la méthode correspondante
        switch (InteractableType)
        {
            case "crate":
                CrateInteract();
                break;
            case "door" :
                DoorInteract();
                break;
            case "button" :
                ButtonInteract();
                break;
        }
    }

    // Méthode pour interagir avec une porte
    private void DoorInteract()
    {
        Debug.Log("Interaction: Door");
        bool isDoorOpen = transform.eulerAngles.y == 270;
        Vector3 rotationVector = transform.eulerAngles;
        rotationVector.y = isDoorOpen ? 0 : 270;
        transform.eulerAngles = rotationVector;
    }

    // Méthode pour interagir avec une caisse
    private void CrateInteract()
    {
        Debug.Log("Interaction: Crate");
    }

    // Méthode pour interagir avec un bouton
    private void ButtonInteract()
    {
        Debug.Log("Interaction: button");
        DoorOpen?.Invoke();
    }
}
