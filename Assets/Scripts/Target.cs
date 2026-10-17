using System;
using UnityEngine;

// Colocar no PREFAB de cada alvo (os 4 tipos).
// A posição é calculada pelo tempo (Lerp do ponto de spawn até o ponto final).
[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{
    [SerializeField] private int points = 10;
    [SerializeField] private int maxHealth = 1;

    // Qualquer script (placar, som, efeitos) pode ouvir isso.
    public static event Action<Target> OnKilled;

    public int Points => points;

    private Vector3 startPos, endPos;
    private float startTime, travelTime;
    private int health;
    private bool initialized;

    private void Awake()
    {
        // Física desligada: o alvo só obedece ao tempo.
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        // Collider como trigger (o tiro será por Raycast).
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    // Chamado pelo spawner logo depois de criar o alvo.
    public void Init(Vector3 start, Vector3 end, float travel)
    {
        startPos = start;
        endPos = end;
        travelTime = Mathf.Max(0.01f, travel); // evita dividir por zero
        startTime = Time.time;
        health = maxHealth;
        initialized = true;
        transform.position = start;
    }

    private void Update()
    {
        if (!initialized) return;

        float t = (Time.time - startTime) / travelTime;
        transform.position = Vector3.Lerp(startPos, endPos, t);

        if (t >= 1f) Destroy(gameObject); // chegou ao fim sem ser acertado
    }

    // Chamado pelo sistema de tiro quando o raycast acerta este alvo.
    public void TakeHit(int damage)
    {
        health -= damage;
        if (health > 0) return;

        OnKilled?.Invoke(this);
        Destroy(gameObject);
    }
}
