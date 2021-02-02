using Assets.Scripts.Test;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;


internal class ClientFabric
    {

        public Client Construct(ClientStruct dataClientStruct, Transform home, Color color)
        {
            //Пока что Null-object паттерн
            var timeSpan = new GameObject("timer")
                .AddOrGetComponent<SpriteMask>();
            
                timeSpan.sprite = Resources.Load<Sprite>("time_gradient");
                timeSpan.alphaCutoff = 1.0f;
                var client = Client.Create(dataClientStruct, home, new VoidBrain(), timeSpan);
                client.gameObject
                    .SetSprite(Resources.Load<Sprite>("art_1"))
                    .ChangeColor(color)
                    .SetAnimatorController(Resources.Load<AnimatorController>("Client"))
                    .AddOrGetComponent<NavMeshAgent>();
            timeSpan.transform.parent = client.transform;
                timeSpan.transform.localPosition = new Vector3(0.0100f, 1.29f, -0.25f);
            return client;
        }
    }