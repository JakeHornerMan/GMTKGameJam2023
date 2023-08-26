using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Rigidbody2D hooked;
    public GameObject[] ropeSegments;
    public int numLinks = 8;

    void Start()
    {
        GenerateRope();
    }

    public void GenerateRope(){
        Rigidbody2D prevBod = hooked;
        for(int i =0; i < numLinks; i++){
            int index = Random.Range(0, ropeSegments.Length);
            GameObject newSeg = Instantiate(ropeSegments[index]);
            newSeg.transform.parent = transform;
            newSeg.transform.position = transform.position;
            HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBod;

            prevBod = newSeg.GetComponent<Rigidbody2D>();
        }
    }
}
