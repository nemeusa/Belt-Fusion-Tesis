using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SmallPortalSpawner : MonoBehaviour
{
    public GameObject portalPrefab;
    public int poolSize = 6;

    [Header("Zona de aparición (local a este objeto)")]
    public Vector3 areaSize = new Vector3(3f, 5f, 1f);

    public Vector2 spawnInterval = new Vector2(0.4f, 1.2f);
    public Vector2 lifetime = new Vector2(1.5f, 3f);
    public Vector2 sizeRange = new Vector2(0.3f, 0.8f);

    readonly Queue<GameObject> _pool = new Queue<GameObject>();
    Coroutine _routine;

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var go = Instantiate(portalPrefab, transform);
            go.SetActive(false);
            _pool.Enqueue(go);
        }
    }

    public void StartSpawning()
    {
        if (_routine == null) _routine = StartCoroutine(Loop());
    }

    public void StopSpawning()
    {
        if (_routine != null) StopCoroutine(_routine);
        _routine = null;
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnInterval.x, spawnInterval.y));
            if (_pool.Count == 0) continue;

            GameObject p = _pool.Dequeue();
            Vector3 local = new Vector3(
                Random.Range(-areaSize.x, areaSize.x) * 0.5f,
                Random.Range(-areaSize.y, areaSize.y) * 0.5f,
                Random.Range(-areaSize.z, areaSize.z) * 0.5f);

            p.transform.position = transform.TransformPoint(local);
            p.transform.localScale = Vector3.zero;
            p.SetActive(true);

            float size = Random.Range(sizeRange.x, sizeRange.y);
            Sequence s = DOTween.Sequence();
            s.Append(p.transform.DOScale(size, 0.35f).SetEase(Ease.OutBack));
            s.AppendInterval(Random.Range(lifetime.x, lifetime.y));
            s.Append(p.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
            s.OnComplete(() => { p.SetActive(false); _pool.Enqueue(p); });
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, areaSize);
    }
}