
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{
    [SerializeField] private int points = 10;
    [SerializeField] private int maxHealth = 1;

    public static event Action<Target> OnKilled;

    public int Points => points;

    private Vector3 startPos, endPos;
    private float startTime, travelTime;
    private float health;
    private bool initialized;

    private EnemyStatusEffects statusEffects;

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;

        statusEffects = GetComponent<EnemyStatusEffects>();

        if (statusEffects == null)
            statusEffects = gameObject.AddComponent<EnemyStatusEffects>();

        statusEffects.Initialize(this);
    }

    public void Init(Vector3 start, Vector3 end, float travel)
    {
        startPos = start;
        endPos = end;
        travelTime = Mathf.Max(0.01f, travel);
        startTime = Time.time;
        health = maxHealth;
        initialized = true;
        transform.position = start;
    }

    private void Update()
    {
        if (!initialized)
            return;

        // Congelado: pausa o movimento e o progresso até o destino.
        if (statusEffects != null && statusEffects.IsFrozen)
        {
            startTime += Time.deltaTime;
            return;
        }

        float t = (Time.time - startTime) / travelTime;
        transform.position = Vector3.Lerp(startPos, endPos, t);

        if (t >= 1f)
            Destroy(gameObject);
    }

    public void TakeHit(int damage)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f || health <= 0f)
            return;

        health -= damage;

        if (health <= 0f)
        {
            health = 0f;
            OnKilled?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void ApplyBurn(
        float damagePerTick,
        float tickInterval,
        float duration)
    {
        if (health <= 0f)
            return;

        statusEffects.AddBurn(
            damagePerTick,
            tickInterval,
            duration
        );
    }

    public void ApplyFreeze(float duration)
    {
        if (health <= 0f)
            return;

        statusEffects.Freeze(duration);
    }
}