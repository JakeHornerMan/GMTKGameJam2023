using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected ParticleSystem featherParticles;
    [SerializeField] protected float featherParticlesZPos = -5;
    [SerializeField] private GameObject chickenSprite;
    [SerializeField] private GameObject hopController;
    [HideInInspector] protected Animator anim;

    [Header("Chicken Health Values")]
    [SerializeField] private int startHealth = 1;

    [HideInInspector] public int health = 0;

    [SerializeField] private float hitStopLength = 0.0f;

    private void Awake()
    {
        health = startHealth;
        anim = chickenSprite.GetComponent<Animator>();
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
            HandleDeath();
    }

    protected virtual void HandleDeath()
    {
        Vector3 particlePos = new(transform.position.x, transform.position.y, featherParticlesZPos);
        Instantiate(featherParticles, particlePos, Quaternion.identity);


        gameObject.SetActive(false);

        //Destroy(gameObject);
    }

    public void RespawnChicken()
    {
        health = startHealth;
        gameObject.SetActive(true);
    }

    public void PlayChickenHitstop()
    {
        HitStop.instance.StartHitStop(hitStopLength);
    }

    public float GetChickenHitstop()
    {
        return hitStopLength;
    }
}
