﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Client : MonoBehaviour, IPointerDownHandler
{
    #region Fields
    
    //Рефакторинг - инкапсуляция. Нигде, кроме данного скрипта и едитора не используется.
    [SerializeField] private float _patience = 10f;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _served = 0;
    //Сам объект же не используется, поэтому, думаю, лучше Transform
    [SerializeField] private Transform _home;
    [SerializeField] private SpriteMask _orderTimer;
    //Перенос таймера в список других полей
    private float _timer;
    private Coroutine activeOrder;
    //Кеширование, ибо GetComponent много кушает
    private IBrain _brain;
    
    #endregion


    #region Properties
    
    public Vector3 HomePosition => _home.position;
    public float Speed => _speed;
    
    #endregion

    
    #region UnityMethods

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        _brain.OnTapped();
    }
    public void Start()
    {
        _brain = GetComponent<IBrain>();
        _orderTimer.gameObject.SetActive(false);
    }
    
    #endregion


    #region Methods

    public void SetHome(Transform homeTransform)
    {
        if (homeTransform == null)
        {
            _home = homeTransform;
        }
    }
    
    public void OrderStuff()
    {
        if(activeOrder != null) StopCoroutine(activeOrder);
        activeOrder = StartCoroutine(WaitForOrder());
    }
    
    private IEnumerator WaitForOrder()
    {
        _timer = _patience;
        _orderTimer.gameObject.SetActive(true);
        while(_timer > 0) {
            _timer -= Time.deltaTime;
            _orderTimer.alphaCutoff = _timer / _patience;
            yield return null;
        }
        _orderTimer.gameObject.SetActive(false);
        _brain.OnExpired();
        activeOrder = null;
    }
    public void Serve()
    {
        _served ++;
        _orderTimer.gameObject.SetActive(false);
        if(activeOrder != null) {
            StopCoroutine(activeOrder);
            activeOrder = null;
        }
        _brain.OnServed();
    }
    public void ExpireNow() {
        _timer = 0;
    }
    
    #endregion
}
