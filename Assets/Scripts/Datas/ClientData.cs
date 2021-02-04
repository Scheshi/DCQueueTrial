using Assets.Scripts.Test;
using UnityEngine;


[CreateAssetMenu(fileName = "Data/ClientData")]
internal sealed class ClientData : ScriptableObject
{
    public ClientStruct ClientStruct;
    public Color Color;
    public HomeData HomeData;
}

