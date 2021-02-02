using Assets.Scripts.Test;
using UnityEngine;

internal class HomeFabric
{
    public Transform Contruct(Vector3 position, Color clientColor)
    { 
        //Вроде так было захардкодино в оригинале.
        clientColor.a *= 0.66f;
       var home = new GameObject("home")
           //Свои методы расширения сюда притащил, извините.
           .SetSprite(Resources.Load<Sprite>("art_1"))
           .ChangeColor(clientColor)
           .SetScale(new Vector3(2.5f, 1.5f, 1.0f))
           .AddComponent<Client>();
       var homeTransform = home.transform;
       homeTransform.position = position;
       return homeTransform;
    }
}