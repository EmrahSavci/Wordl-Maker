using UnityEngine;

public class Obstacle : Contactable
{
    protected override void Contant(GameObject _gObject)
    {
        _gObject.GetComponent<Player>().HealthSystem(Value);
        base.Contant(_gObject);
    }
}
