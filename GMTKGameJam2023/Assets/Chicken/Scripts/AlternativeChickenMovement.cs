using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeChickenMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject chickenSprite;
    [SerializeField] public Collider2D chickenCollider;
    [HideInInspector] private SpriteRenderer spriteRenderer;
    [HideInInspector] public LayerMask cementLayer;
    [HideInInspector] private string cementLayerName = "Cement";
    [SerializeField] private Color freezeColor;
    [HideInInspector] private Color originalColor = Color.white;
    
    [Header("Chicken Values")]
    [SerializeField] private float speed = 1f;
    [SerializeField] public bool stopMovement = false;
    [SerializeField] public bool ignoreCement = false;
    [HideInInspector] public bool isStuck = false;
    [SerializeField] private float hitStopLength = 0.0f;
    [HideInInspector] public bool isTurboChicken = false;
    [HideInInspector] public bool isWagonChicken = false;

    [Header("Vertical Info")]
    [SerializeField] private bool canMoveVertical = false;
    [HideInInspector] private int verticalDirection = 1;
    [SerializeField] private static float maxVerticalHeight = 7.5f;
    [SerializeField] private static float minVerticalHeight = -7.5f;

    [Header("Wheelbarrow Chicken Values")]
    [SerializeField] private float substanceDurationSeconds = 20f;
    private List<RoadHighlight> affectedRoads;
    [SerializeField] private GameObject slowSubstancePrefab;
    [SerializeField] private Transform dropPoint;

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        int cementLayerIndex = LayerMask.NameToLayer(cementLayerName);
        spriteRenderer = chickenSprite.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(this.gameObject.name.Contains("Turbo")){
            isTurboChicken = true;
            soundManager.PlayTurboChicken();
        }
        if(this.gameObject.name.Contains("WheelBarrow")){
            isWagonChicken = true;
            soundManager.PlayWagonChicken();
            affectedRoads = new List<RoadHighlight>();
        }

        if (canMoveVertical)
        {
            float random = Random.Range(0, 2);

            if (random == 0)
            {
                verticalDirection = 1;
            }
            else
            {
                verticalDirection = -1;
            }
        }
    }

    // protected override Vector2 ChooseNextDirection()
    // {
    //     return new Vector2(1, 0);
    // }

    private void Update()
    {
        if(!stopMovement)
            Movement();
        
        if(isWagonChicken)
            DropOnRoadCenter();
    }
    
    public void Movement(){
        isStuck = chickenCollider.IsTouchingLayers(cementLayer);

        if (!isStuck || ignoreCement)
        {
            if (canMoveVertical)
            {
                transform.position += new Vector3((speed * 0.7f) * Time.deltaTime, (speed * 0.7f) * Time.deltaTime * verticalDirection, 0);

                CheckVertical();
            }
            else
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            
        }
    }

    private void OnDestroy()
    {
        if(isTurboChicken){
            soundManager.PlayTurboChickenDeath();
        }
        if(isWagonChicken){
            soundManager.PlayWagonChickenDeath();
        }
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

    private void DropOnRoadCenter()
    {
        // Raycast down from drop point to find the road that the chicken is currently on
        Collider2D raycastHit = Physics2D.Raycast(dropPoint.position, Vector2.zero).collider;
        RoadHighlight road = raycastHit?.GetComponent<RoadHighlight>();
        if (
            raycastHit != null // Hit something
            && road != null // Is a road
            && !affectedRoads.Contains(road) // Not already placed upon
        )
        {
            // Drop horizontally centered on road
            DropSubstance(new Vector2(road.transform.position.x, transform.position.y));
            // Add to exclusion list
            affectedRoads.Add(road);
        }
    }

    private void DropSubstance(Vector2 dropPos)
    {
        GameObject substance = Instantiate(
            slowSubstancePrefab,
            // dropPoint.position,
            dropPos,
            transform.rotation
        );
        Destroy(substance, substanceDurationSeconds);
    }

    private void CheckVertical()
    {
        if (transform.position.y > maxVerticalHeight || transform.position.y < minVerticalHeight)
        {
            FlipVertical();
        }
    }

    private void FlipVertical()
    {
        verticalDirection *= -1;
    }
}
