using UnityEngine;

public class RunAway : IBehaviour
{
    private readonly Enemy _me;
    
    public RunAway(Enemy enemy)
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
        
        _me.MoveDirection = (_me.transform.position - _me.Target.position).normalized; //Direction from target to me
    }
}
