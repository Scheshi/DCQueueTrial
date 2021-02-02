using UnityEngine;


public class GameController : MonoBehaviour
{
    [SerializeField] private ClientData[] _clientDatas;
    //[SerializeField] private ServerData _server;


    private void Start()
    {
        //Паттерн "Единная точка вхождения"
        ClientFabric clientFabric = new ClientFabric();
        HomeFabric homeFabric = new HomeFabric();
        for(var i = 0; i < _clientDatas.Length; i++)
        {
            var pack = new GameObject("ClientPack" + i);
            var home = homeFabric.Contruct(_clientDatas[i].HomeData.Position, _clientDatas[i].Color);
            var homeTransform = home.transform;
            homeTransform.parent = pack.transform;
            var client = clientFabric.Construct(
                _clientDatas[i].ClientStruct,
                homeTransform,
                _clientDatas[i].Color,
                FindObjectOfType<QueueController>(),
                FindObjectOfType<Mutex>());
            home.InjectClient(client);
            client.transform.parent = pack.transform;
        }
    }
}



