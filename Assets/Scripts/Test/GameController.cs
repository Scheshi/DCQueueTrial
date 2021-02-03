using Assets.Scripts.Test;
using UnityEngine;


public class GameController : MonoBehaviour
{
    [SerializeField] private ClientData[] _clientDatas;
    [SerializeField] private ServerData _server;


    private void Start()
    {
        //Паттерн "Единная точка вхождения"
        ClientFabric clientFabric = new ClientFabric();
        HomeFabric homeFabric = new HomeFabric();
        ServerFactory serverFactory = new ServerFactory();
        
        Mutex mutex = serverFactory.Construct(_server.Position, _server.Color);
        Transform[] queuePoints = new Transform[_clientDatas.Length];

        Vector3 mutexPosition = mutex.transform.position;

        float distanceFromPoints = Resources.Load<Sprite>("art_1").bounds.size.x;
        for (int i = 0; i < queuePoints.Length; i++)
        {
            queuePoints[i] = new GameObject("queue" + i)
                .AddOrGetComponent<QueueMarker>()
                .transform;
            queuePoints[i].position = new Vector3(mutexPosition.x + distanceFromPoints * (i + 1), mutexPosition.y,
                mutexPosition.z);
        }
        
        QueueController queue = new QueueController(mutexPosition, queuePoints);
        for(var i = 0; i < _clientDatas.Length; i++)
        {
            var pack = new GameObject("ClientPack" + i).transform;
           pack.position = _clientDatas[i].HomeData.Position;
            var home = homeFabric.Contruct(_clientDatas[i].HomeData.Position, _clientDatas[i].Color);
            var homeTransform = home.transform;
            homeTransform.parent = pack.transform;
            var client = clientFabric.Construct(
                _clientDatas[i].ClientStruct,
                homeTransform,
                _clientDatas[i].Color,
                queue,
                mutex);
            home.InjectClient(client);
            client.transform.parent = pack;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _server.Color;
        Gizmos.DrawCube(_server.Position, new Vector3(0.7f, 0.7f, 0.7f));
        float distance = Resources.Load<Sprite>("art_1").bounds.size.x;
        var exampleMutexPosition = _server.Position + Vector3.back;
        for (int i = 0; i < _clientDatas.Length; i++)
        {
            Gizmos.color = _clientDatas[i].Color;
            Gizmos.DrawSphere(_clientDatas[i].HomeData.Position, 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(exampleMutexPosition.x + distance * (i + 1), exampleMutexPosition.y, exampleMutexPosition.z), 0.1f);
        }
    }
}



