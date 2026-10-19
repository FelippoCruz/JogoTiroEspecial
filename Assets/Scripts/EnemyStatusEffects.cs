using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffects : MonoBehaviour
{
    private class BurnEffect
    {
        public float damagePerTick;
        public float tickInterval;
        public float remainingDuration;
        public float tickTimer;
    }

    private readonly List<BurnEffect> burns =
        new List<BurnEffect>();

    private Target target;
    private float frozenUntil;

    public bool IsFrozen => Time.time < frozenUntil;

    public void Initialize(Target owner)
    {
        target = owner;
    }

    public void AddBurn(
        float damagePerTick,
        float tickInterval,
        float duration)
    {
        if (damagePerTick <= 0f || duration <= 0f)
            return;

        BurnEffect burn = new BurnEffect
        {
            damagePerTick = damagePerTick,
            tickInterval = Mathf.Max(0.01f, tickInterval),
            remainingDuration = duration,
            tickTimer = Mathf.Max(0.01f, tickInterval)
        };

        // Cada impacto cria uma queimadura separada.
        burns.Add(burn);
    }

    public void Freeze(float duration)
    {
        if (duration <= 0f)
            return;

        // Um novo impacto pode prolongar o congelamento.
        frozenUntil = Mathf.Max(
            frozenUntil,
            Time.time + duration
        );
    }

    private void Update()
    {
        if (target == null)
            return;

        // Percorre de trás para frente para poder remover efeitos.
        for (int i = burns.Count - 1; i >= 0; i--)
        {
            BurnEffect burn = burns[i];

            burn.remainingDuration -= Time.deltaTime;
            burn.tickTimer -= Time.deltaTime;

            if (burn.tickTimer <= 0f)
            {
                burn.tickTimer += burn.tickInterval;

                target.TakeDamage(burn.damagePerTick);

                // O dano pode ter matado o alvo.
                if (target == null)
                    return;
            }

            if (burn.remainingDuration <= 0f)
                burns.RemoveAt(i);
        }
    }
}