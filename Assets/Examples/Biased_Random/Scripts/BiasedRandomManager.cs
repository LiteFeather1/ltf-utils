using UnityEngine;
using UnityEditor;
using LTF.BiasedRandom;

public class BiasedRandomManager : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    private BiasedRandom _biasedRandom = new(-100f, 100f, 0f, .5f);
    [SerializeField] private float _dotAmount = 100;

    [Header("Seed")]
    [SerializeField] private int _seed;
    [SerializeField] private bool _randomizeSeed = true;

    private const float RADIUS = .032f;

    private static readonly Color sr_unityRandom = new(.23f, .63f, .77f, .33f);
    private static readonly Color sr_biasedRandom = new(.77f, .37f, .23f, .33f);

    private void OnValidate()
    {
        if (!_randomizeSeed)
            return;

        _randomizeSeed = false;
        _seed = (int)System.DateTime.Now.Ticks;

    }

    private void OnDrawGizmos()
    {
        var cam = SceneView.currentDrawingSceneView ? SceneView.currentDrawingSceneView.camera : _cam;

        var zoom = cam.orthographicSize;
        Random.InitState(_seed);
        Gizmos.color = sr_unityRandom;
        for (int i = 0; i < _dotAmount; i++)
        {
            Gizmos.DrawSphere(
                new (
                    cam.transform.position.x + (Random.Range(_biasedRandom.Min, _biasedRandom.Max) * zoom * 0.015f),
                    cam.transform.position.y,
                    cam.transform.position.z + 10f), 
                RADIUS * zoom);
        }
        Gizmos.color = sr_biasedRandom;
    }
}
