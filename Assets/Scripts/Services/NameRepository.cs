using UnityEngine;


//Если про остальное - все понятно, то здесь нужно определенно сделать ремарку.
//Дело в том, что при каждом стринговом значении(при задании имени, при взятии ресурса по пути и т.п.) создается новый объект.
//Ради оптимизации, я решил вынести их в статический класс
public static class NameRepository
{
    public static string Art1 = "art_1";
    public static string Art2 = "art_2";
    public static string ClientPackName = "ClientPack";
    public static string QueuePointName = "queue";
    public static string Client = "Client";
    public static string NotExists = " not exists";
}
