using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
//[SerializeField] private List<string> InteractableType = new List<string>() { "crate", "door"};
[SerializeField] private string InteractableType;
public static event Action DoorOpen;

 public void Interact(){
    Debug.Log("l'objet a été interacté");

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


private void DoorInteract(){
    Debug.Log("Interaction: Door");
    bool isDoorOpen = transform.eulerAngles.y == 270;
        Vector3 rotationVector = transform.eulerAngles;
        rotationVector.y = isDoorOpen ? 0 : 270;
        transform.eulerAngles = rotationVector;
}
private void CrateInteract(){
    Debug.Log("Interaction: Crate");

}
private void ButtonInteract(){
    Debug.Log("Interaction: button");
    DoorOpen?.Invoke();
}
}
