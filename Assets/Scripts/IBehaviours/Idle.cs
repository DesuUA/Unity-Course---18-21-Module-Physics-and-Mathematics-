using UnityEngine;

public class Idle : IBehaviour
{
    private readonly Enemy _me;

    public Idle(Enemy enemy)
    {
        _me = enemy;
    }
        
    public void Behave()
    {
        _me.MoveDirection = Vector3.zero;
    }
}