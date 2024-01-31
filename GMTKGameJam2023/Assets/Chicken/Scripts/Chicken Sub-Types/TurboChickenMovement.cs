using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboChickenMovement : ChickenMovement
{
    [Header("Turbo Chicken Values")]
    [SerializeField] private float speed = 1f;

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    protected override void StartMovement()
    {
        soundManager.PlayTurboChicken();
    }

    protected override Vector2 ChooseNextDirection()
    {
        return new Vector2(1, 0);
    }

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
}
