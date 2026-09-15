using UnityEngine;

public class RandomMove : IBehaviour
{
    private const float WaitTime = 1f;
    
    private Enemy _me;
    private Vector3 MyPosition => _me.transform.position;
    private Vector3 _moveDirection;
    private float _currentTime;
    
    public RandomMove(Enemy enemy)
    {
        _me = enemy;
        
        _moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }
    
    public void Behave()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime > WaitTime)
        {
            _moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            _currentTime = 0f;
        }

        _me.MoveDirection = _moveDirection;
    }
}