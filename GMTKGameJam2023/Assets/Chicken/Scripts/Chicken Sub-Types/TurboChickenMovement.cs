using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboChickenMovement: MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject chickenSprite;
    [SerializeField] public Collider2D chickenCollider;
    [HideInInspector] private SpriteRenderer spriteRenderer;
    [HideInInspector] public LayerMask cementLayer;
    [HideInInspector] private string cementLayerName = "Cement";
    [SerializeField] private Color freezeColor;
    [HideInInspector] private Color originalColor = Color.white;
    
    [Header("Turbo Chicken Values")]
    [SerializeField] private float speed = 1f;
    [SerializeField] public bool stopMovement = false;
    [SerializeField] public bool ignoreCement = false;
    [HideInInspector] public bool isStuck = false;
    [SerializeField] private float hitStopLength = 0.0f;

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        int cementLayerIndex = LayerMask.NameToLayer(cementLayerName);
        spriteRenderer = chickenSprite.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        soundManager.PlayTurboChicken();
    }

    // protected override Vector2 ChooseNextDirection()
    // {
    //     return new Vector2(1, 0);
    // }

    private void Update()
    {
        if(!stopMovement)
            TurboMovement();
    }
    
    public void TurboMovement(){
        isStuck = chickenCollider.IsTouchingLayers(cementLayer);

        if (!isStuck || ignoreCement)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    private void OnDestroy()
    {
        soundManager.PlayTurboChickenDeath();
    }

    public IEnumerator FreezeChicken(float stopTime, bool isFreeze)
    {
        if(isFreeze){
            spriteRenderer.color = freezeColor;
            chickenSprite.GetComponent<Animator>().enabled = false;
        }
        stopMovement = true;

        yield return new WaitForSeconds(stopTime);

        if(isFreeze){
            spriteRenderer.color = originalColor;
            chickenSprite.GetComponent<Animator>().enabled = true;
        }
        stopMovement = false;
    }

    public IEnumerator FlashChicken(float stopTime, bool isFlash)
    {
        if (isFlash)
        {
            chickenSprite.GetComponent<Animator>().enabled = false;
        }
        stopMovement = true;

        yield return new WaitForSeconds(stopTime);

        if (isFlash)
        {
            chickenSprite.GetComponent<Animator>().enabled = true;
        }
        stopMovement = false;
    }

    public float GetChickenHitstop()
    {
        return hitStopLength;
    }
}
