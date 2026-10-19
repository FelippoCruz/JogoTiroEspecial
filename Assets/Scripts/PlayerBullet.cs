
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public enum AmmoType
    {
        Normal,
        Heavy,
        Fire,
        Ice
    }

    [Header("Munição selecionada")]
    [SerializeField] private AmmoType ammoType = AmmoType.Normal;

    [Header("Configuração da bala normal")]
    [SerializeField] private float normalSpeed = 20f;
    [SerializeField] private float normalDamage = 1f;

    [Header("Configuração da bala pesada")]
    [SerializeField] private float heavySpeed = 8f;
    [SerializeField] private float heavyDamage = 4f;

    [Header("Configuração da bala de fogo")]
    [SerializeField] private float fireSpeed = 30f;
    [SerializeField] private float burnDamagePerTick = 0.25f;
    [SerializeField] private float burnTickInterval = 1f;
    [SerializeField] private float burnDuration = 4f;

    [Header("Configuração da bala de gelo")]
    [SerializeField] private float iceSpeed = 20f;
    [SerializeField] private float iceDamage = 0.5f;
    [SerializeField] private float freezeDuration = 1f;

    [Header("Geral")]
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private LayerMask hitMask = ~0;

    private Vector3 shootDirection;
    private float speed;
    private float damage;
    private bool initialized;

    public void Initialize(Vector3 direction, AmmoType type)
    {
        ammoType = type;
        shootDirection = direction.normalized;

        // Define os atributos da munição escolhida.
        switch (ammoType)
        {
            case AmmoType.Normal:
                speed = normalSpeed;
                damage = normalDamage;
                break;

            case AmmoType.Heavy:
                speed = heavySpeed;
                damage = heavyDamage;
                break;

            case AmmoType.Fire:
                speed = fireSpeed;
                damage = 0f;
                break;

            case AmmoType.Ice:
                speed = iceSpeed;
                damage = iceDamage;
                break;
        }

        transform.forward = shootDirection;
        initialized = true;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!initialized)
            return;

        float step = speed * Time.deltaTime;

        if (Physics.Raycast(
            transform.position,
            shootDirection,
            out RaycastHit hit,
            step,
            hitMask,
            QueryTriggerInteraction.Collide))
        {
            transform.position = hit.point;
            OnHit(hit.collider);
            return;
        }

        transform.position += shootDirection * step;
    }

    private void OnHit(Collider other)
    {
        Target target = other.GetComponentInParent<Target>();

        if (target != null)
        {
            switch (ammoType)
            {
                case AmmoType.Normal:
                    target.TakeDamage(damage);
                    break;

                case AmmoType.Heavy:
                    target.TakeDamage(damage);
                    break;

                case AmmoType.Fire:
                    target.ApplyBurn(
                        burnDamagePerTick,
                        burnTickInterval,
                        burnDuration
                    );
                    break;

                case AmmoType.Ice:
                    target.TakeDamage(damage);
                    target.ApplyFreeze(freezeDuration);
                    break;
            }
        }

        // A bala desaparece após o impacto, independentemente do tipo.
        Destroy(gameObject);
    }
}