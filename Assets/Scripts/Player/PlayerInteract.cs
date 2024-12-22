using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    private Interactable lastInteractable;

    void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        
        RaycastHit hitInfo;
        bool hitSomething = Physics.Raycast(ray, out hitInfo, distance, mask);

        if (hitSomething && hitInfo.collider.GetComponent<Interactable>() != null) {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

            // If this is a new interactable, disable the outline of the last one
            if (lastInteractable != interactable) {
                if (lastInteractable != null) {
                    lastInteractable.GetComponent<Outline>().enabled = false;
                }
                lastInteractable = interactable;
            }

            interactable.GetComponent<Outline>().enabled = true;
            playerUI.UpdateText(interactable.promptMessage);

            if (inputManager.onFoot.Interact.triggered) {
                interactable.BaseInteract();
            }
        } 
        else {
            if (lastInteractable != null) {
                lastInteractable.GetComponent<Outline>().enabled = false;
                lastInteractable = null;
            }
        }
    }
}
