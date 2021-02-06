using UnityEngine;
using UnityEngine.EventSystems;

public class Server : MonoBehaviour, IPointerDownHandler
{
    public IClientView CurrentClientView;
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(CurrentClientView != null) 
        {
            CurrentClientView.Controller.Serve();
        }
    }
}
