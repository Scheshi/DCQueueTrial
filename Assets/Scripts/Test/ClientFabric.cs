using Assets.Scripts.Test;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;


internal class ClientFabric
    {

        public ClientView Construct(ClientStruct dataClientStruct, Transform home, Color color,
            QueueController queueController, Mutex mutex)
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
                var client = new GameObject("client").AddComponent<ClientView>();

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

                var animator = client.GetComponent<Animator>();
                IClientController controller = new ClientController(home, timeSpan, client, dataClientStruct);
                
                IBrain brain = new ClientDirectionController(
                    mutex,
                    queueController,
                    client,
                    new ClientMoveController(client, navAgent, animator, dataClientStruct.Speed),
                    animator,
                    controller,
                    client
                    );

                controller.SetBrain(brain);
                
                client.SetBrain(brain);
                client.InjectController(controller);
                client.transform.position = home.position;
            return client;
        }
    }