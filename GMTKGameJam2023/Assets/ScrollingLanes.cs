using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScrollingLanes : MonoBehaviour
{
    [SerializeField] private Transform[] laneChildren;
    private Transform currentLowestChild;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float childHeight;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        // This will hold the lowest position found, initialized to a very high value
        float lowestYPosition = float.MaxValue;

        // Assuming all children have the same size, get the first child's renderer to calculate height
        if (laneChildren.Length > 0)
        {
            Renderer renderer = laneChildren[0].GetComponent<Renderer>();
            if (renderer != null)
            {
                childHeight = renderer.bounds.size.y;
            }

            // Initialize currentLowestChild to the first child as a starting point
            currentLowestChild = laneChildren[0];

            // Find the actual lowest child
            foreach (Transform child in laneChildren)
            {
                if (child.position.y < lowestYPosition)
                {
                    lowestYPosition = child.position.y;
                    currentLowestChild = child;
                }
            }
        }
    }

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {

        // Move the child down
        transform.position += Vector3.down * scrollSpeed * Time.deltaTime;

        // Check if the child is off-screen
        if ((currentLowestChild.position.y) < -childHeight)
        {
            float highestYPosition = GetHighestChildYPosition();
            currentLowestChild.position = new Vector3(currentLowestChild.position.x, highestYPosition + childHeight, currentLowestChild.position.z);

            currentLowestChild = GetLowestChildYPosition();

            //transform.position += new Vector3(0, childHeight, 0);

        }
    }

    private float GetHighestChildYPosition()
    {
        float highestY = float.NegativeInfinity;
        foreach (Transform child in laneChildren)
        {
            if (child.position.y > highestY)
            {
                highestY = child.position.y;
            }
        }
        return highestY;
    }

    private Transform GetLowestChildYPosition()
    {
        Transform lowestY = currentLowestChild;
        foreach (Transform child in laneChildren)
        {
            if (child.position.y < lowestY.position.y)
            {
                lowestY = child;
            }
        }
        return lowestY;
    }


}