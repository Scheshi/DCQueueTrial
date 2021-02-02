using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Home : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Client _client;
    public void Start()
    {
        if(_client) {
            GetComponent<SpriteRenderer>().color = _client.GetComponent<SpriteRenderer>().color * 0.66f;
            _client.SetHome(transform);
        }
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(_client) _client.OnPointerDown(pointerEventData);
    }
}
