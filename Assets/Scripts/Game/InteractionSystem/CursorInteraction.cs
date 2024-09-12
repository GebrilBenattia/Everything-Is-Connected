using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorInteraction : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Interaction Settings
    [Header("Interaction Settings")]
    [SerializeField] private float m_MaxDistance;
    [SerializeField] private LayerMask m_AllowedLayers;

    // Private Variables
    IClickableObject m_LastClickableObject = null;

    // ######################################### FUNCTIONS ########################################

    private void Update()
    {
        // On Mouse Left Button Clicked
        if (Input.GetMouseButtonDown(0)) {

            // Create a raycast
            Ray ray = GameplayManager.Instance.camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, m_MaxDistance, m_AllowedLayers)) {

                // Call Event On Click
                if (hitInfo.collider.gameObject.TryGetComponent(out IClickableObject clickableObject)) {
                    clickableObject.EventOnLeftButtonDown(hitInfo);
                    m_LastClickableObject = clickableObject;
                }
            }
        }

        // On Mouse Left Button Released
        else if (Input.GetMouseButtonUp(0)) {

            // Call Event On Release
            if (m_LastClickableObject != null) {
                m_LastClickableObject.EventOnLeftButtonUp();
                m_LastClickableObject = null;
            }
        }
    }
}
