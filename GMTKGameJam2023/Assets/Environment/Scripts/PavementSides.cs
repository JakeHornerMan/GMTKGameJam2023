using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PavementSides : MonoBehaviour
{
    [Header("Edge Settings")]
    [SerializeField] private bool leftEdge = true;  // Check for pavement on the left
    [SerializeField] private bool rightEdge = true; // Check for pavement on the right

    [Header("Raycast Settings")]
    [SerializeField] private float raycastOffset = 0.5f; // Offset from the pavement's edge
    [SerializeField] private float raycastDistance = 1.0f; // Distance to check for adjacent pavement

    private void Start()
    {
        CheckEdgesAndDeactivate();
    }

    private void CheckEdgesAndDeactivate()
    {
        if (leftEdge)
        {
            Vector2 raycastOriginLeft = new Vector2(transform.position.x - raycastOffset, transform.position.y);
            if (CheckForAdjacentPavement(raycastOriginLeft, Vector2.left))
            {
                // Deactivate the GameObject if an adjacent pavement is detected on the left
                gameObject.SetActive(false);
            }
        }

        if (rightEdge)
        {
            Vector2 raycastOriginRight = new Vector2(transform.position.x + raycastOffset, transform.position.y);
            if (CheckForAdjacentPavement(raycastOriginRight, Vector2.right))
            {
                // Deactivate the GameObject if an adjacent pavement is detected on the right
                gameObject.SetActive(false);
            }
        }
    }

    private bool CheckForAdjacentPavement(Vector2 origin, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, raycastDistance);
        return hit.collider != null && hit.collider.CompareTag("Pavement");
    }
}
