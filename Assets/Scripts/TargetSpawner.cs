using System.Collections;
using UnityEngine;

// Colocar em um GameObject VAZIO chamado "TargetSpawner" (não no prefab do alvo).
public class TargetSpawner : MonoBehaviour
{
    [Header("Os 4 tipos de alvo")]
    public Target[] targetPrefabs = new Target[4];

    [Header("Trajetos (mesma ordem: spawn[i] vai até end[i])")]
    public Transform[] spawnPoints;
    public Transform[] endPoints;

    [Header("Tempo")]
    public float travelTime = 3f;      // segundos do spawn até o fim
    public float spawnInterval = 1f;   // segundos entre alvos

    private bool spawning;

    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (spawning) return;
        spawning = true;
        StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        spawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnLoop()
    {
        var wait = new WaitForSeconds(spawnInterval);

        while (spawning)
        {
            yield return wait;

            if (spawnPoints.Length == 0 || spawnPoints.Length != endPoints.Length)
            {
                Debug.LogWarning("TargetSpawner: spawnPoints e endPoints precisam ter o mesmo tamanho (e não estar vazios).");
                continue;
            }

            Target prefab = targetPrefabs[Random.Range(0, targetPrefabs.Length)];
            if (prefab == null)
            {
                Debug.LogWarning("TargetSpawner: há um prefab vazio em Target Prefabs.");
                continue;
            }

            int i = Random.Range(0, spawnPoints.Length);
            Vector3 start = spawnPoints[i].position;
            Vector3 end = endPoints[i].position;

            Target t = Instantiate(prefab, start, prefab.transform.rotation);
            t.Init(start, end, travelTime);
        }
    }
}
