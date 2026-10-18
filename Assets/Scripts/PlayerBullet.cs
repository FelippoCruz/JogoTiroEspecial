using UnityEngine;

// Colocar no PREFAB da bala (PlayerBullet).
// Move a bala por código e testa o caminho com Raycast a cada quadro,
// então não atravessa a parede fina e não depende de Tag nem de Rigidbody.
public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;
    [Tooltip("Quais layers a bala pode acertar (padrão: tudo).")]
    [SerializeField] private LayerMask hitMask = ~0;

    private void Start()
    {
        Destroy(gameObject, lifetime); // rede de segurança se não acertar nada
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;

        // Olha o trecho que a bala vai andar neste quadro.
        // QueryTriggerInteraction.Collide: os alvos são triggers, então precisam ser detectados.
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,
                            step, hitMask, QueryTriggerInteraction.Collide))
        {
            transform.position = hit.point;
            OnHit(hit.collider);
            return;
        }

        transform.position += transform.forward * step;
    }

    private void OnHit(Collider other)
    {
        // Se for um alvo, causa dano. Se for parede ou qualquer outra coisa, só some.
        if (other.TryGetComponent(out Target target))
        {
            target.TakeHit(damage);
        }

        Destroy(gameObject);
    }
}
