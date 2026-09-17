using UnityEngine;

public class Chase : IBehaviour
{
    private readonly Enemy _me;
        
    public Chase(Enemy enemy)
    {
        _me = enemy;
    }
    
    public void Behave()
    {
        if (_me.Target == null)
        {
            Debug.Log("Target not found");
            return;
        }

        _me.MoveDirection = (_me.Target.position - _me.transform.position).normalized;
    }
}
