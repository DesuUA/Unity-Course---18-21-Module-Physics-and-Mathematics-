using UnityEngine;

public class Die : IBehaviour
{
    private const float WaitTime = 2f;
    
    private readonly Enemy _me;
    private float _time;
    
    public Die(Enemy enemy)
    {
        _me = enemy;
    }

    public void Behave()
    {
        if (_me.LockBehaviour == false)
        {
            _me.LockBehaviour = true;
            _me.MoveDirection = Vector3.zero;
        }
        
        if (_time > WaitTime && _me.Die == false)
            _me.Die = true;
        else if (_time < WaitTime)
            _time += Time.deltaTime;
        
        //Debug.Log($"Time to die: {_time}");
    }
}
