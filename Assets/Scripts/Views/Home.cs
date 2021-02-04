using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Home : MonoBehaviour, IPointerDownHandler
{
    private IClientView _clientView;

    public void Start()
    {
        /*if(_clientView) {
            GetComponent<SpriteRenderer>().color = _clientView.GetComponent<SpriteRenderer>().color * 0.66f;
            _clientView.SetHome(transform);*/
        //}
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (_clientView != null) _clientView.OnPointerDown(pointerEventData);
    }

    public void InjectClient(IClientView clientView)
    {
        if (_clientView == null)
        {
            _clientView = clientView;
        }
    }
}
