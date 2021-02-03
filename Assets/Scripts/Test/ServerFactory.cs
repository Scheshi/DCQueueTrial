using Assets.Scripts.Test;
using UnityEngine;


    public class ServerFactory
    {
        public Mutex Construct(Vector3 position, Color color)
        {
            color.a *= 0.66f;
            Server server = new GameObject("Server")
                .SetSprite(Resources.Load<Sprite>("art_1"))
                .ChangeColor(color)
                .SetScale(new Vector3(2.0f, 1.25f, 1.0f))
                .SetBoxCollider2D(new Vector2(0.0f, 0.9f), new Vector2(0.9f, 1.8f))
                .AddOrGetComponent<Server>();
            server.transform.position = position;

            
            
            Mutex mutex = new GameObject("serverPoint")
                .AddOrGetComponent<Mutex>();

            Transform mutexTransform = mutex.transform;
            mutexTransform.parent = server.transform;
            mutexTransform.localPosition = Vector3.back;

            return mutex;
        }
    }
