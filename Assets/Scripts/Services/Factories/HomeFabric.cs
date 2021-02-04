using System;
using Assets.Scripts.Test;
using UnityEngine;

internal class HomeFabric
{
    public Home Contruct(Vector3 position, Color clientColor)
    { 
        //Вроде так было захардкодино в оригинале.
        clientColor.a *= 0.66f;
        var sprite = Resources.Load<Sprite>(NameRepository.Art1);
        if (!sprite)
        {
            throw new NullReferenceException(NameRepository.Art1 + "not exists");
        }

        var home = new GameObject("home")
           //Свои методы расширения сюда притащил, извините.
           .SetSprite(sprite)
           .ChangeColor(clientColor)
           .SetScale(new Vector3(2.5f, 1.5f, 1.0f))
           .AddComponent<Home>();
       home.transform.position = position;
       return home;
    }
}