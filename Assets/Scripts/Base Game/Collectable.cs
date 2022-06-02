using UnityEngine;

public class Collectable : Contactable
{
    protected override void Contant(GameObject _gObject)
    {
        Datas.Coin.CoinAdd(Value);
        base.Contant(_gObject);
    }
}
