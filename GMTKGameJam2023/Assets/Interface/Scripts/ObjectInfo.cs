using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    [SerializeField] public string objectName;
    [SerializeField] public Sprite objectImage;
    [SerializeField][TextArea(10, 2)] public string objectDiscription; 
}
