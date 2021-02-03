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

        var markers = FindObjectsOfType<QueueMarker>();
        Transform[] queuePoints = new Transform[markers.Length];
        
        for (int i = 0; i < queuePoints.Length; i++)
        {
            queuePoints[i] = markers[i].transform;
        }
        
        QueueController queue = new QueueController(mutex.transform.position, queuePoints);
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
        foreach (var item in _clientDatas)
        {
            Gizmos.color = item.Color;
            Gizmos.DrawSphere(item.HomeData.Position, 0.5f);
        }

        Gizmos.color = _server.Color;
        Gizmos.DrawCube(_server.Position, new Vector3(0.7f, 0.7f, 0.7f));
    }
}



