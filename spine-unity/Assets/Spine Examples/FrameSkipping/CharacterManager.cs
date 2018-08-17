using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour {
    public float minX = -65f;
    public float maxX = 65f;
    public float minY = -30f;
    public float maxY = 30f;

    private static readonly List<object> _characters = new List<object>();
    private FrameSkippingManager _frameSkippingManager;

    // Use this for initialization
    void Start()
    {
        _frameSkippingManager = GameObject.FindObjectOfType<FrameSkippingManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateCharacters(int count, Transform characterTransform)
    {
        for (int i = 0; i < count; i++)
        {
            var position = GetRandomPosition();
            var instance = GameObject.Instantiate(characterTransform, position, Quaternion.identity);

            _characters.Add(instance);

            if (_frameSkippingManager != null)
            {
                _frameSkippingManager.RegisterRenderer(instance.gameObject.GetComponent<SkeletonRenderer>());
            }
        }
    }

    public void RemoveAll()
    {
        foreach (var character in _characters)
        {
            var component = (Component)character;
            Destroy(component.gameObject);

            if (_frameSkippingManager != null)
            {
                _frameSkippingManager.UnregisterRenderer(component.gameObject.GetComponent<SkeletonRenderer>());
            }
        }

        _characters.Clear();
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        return new Vector3(x, y, 0);
    }
}
