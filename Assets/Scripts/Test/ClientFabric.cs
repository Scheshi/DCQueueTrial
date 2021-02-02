using Assets.Scripts.Test;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;


internal class ClientFabric
    {

        public Client Construct(ClientStruct dataClientStruct, Transform home, Color color,
            QueueController controller, Mutex mutex)
        {
            //Пока что Null-object паттерн
            var timeSpan = new GameObject("timer")
                .AddOrGetComponent<SpriteMask>();
            
                timeSpan.sprite = Resources.Load<Sprite>("time_gradient");
                var timeFill = new GameObject("fill")
                    .AddOrGetComponent<SpriteRenderer>();
                timeFill.sprite = Resources.Load<Sprite>("art_2");

                timeFill.transform.parent = timeSpan.transform;
                timeFill.transform.localPosition = Vector3.zero;
                timeFill.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                var client = Client.Create(dataClientStruct, home, timeSpan);

                client.gameObject
                    .SetSprite(Resources.Load<Sprite>("art_1"))
                    .ChangeColor(color)
                    .SetAnimatorController(Resources.Load<AnimatorController>("Client"))
                    .SetBoxCollider2D(new Vector2(0.0f, 0.9f), new Vector2(0.9f, 1.8f));
                
                
                
                var navAgent = client.gameObject.AddOrGetComponent<NavMeshAgent>();
                navAgent.height = 1.0f;
                navAgent.radius = 0.1f;
                navAgent.baseOffset = 0.0f;
                navAgent.angularSpeed = 1000.0f;
                navAgent.acceleration = 100.0f;
                
                timeSpan.transform.parent = client.transform;
                timeSpan.transform.localPosition = new Vector3(0.0100f, 1.29f, -0.25f);
                
                client.SetBrain(new ClientDirectionController(
                    mutex,
                    controller,
                    client,
                    client.gameObject.AddComponent<Legs>(),
                    client.GetComponent<Animator>()));
            return client;
        }
    }