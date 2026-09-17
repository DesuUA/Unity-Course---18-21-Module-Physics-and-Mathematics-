using UnityEngine;

public class Enemy : MonoBehaviour, IDie
{
    private IBehaviour _activeBehaviour;
    private IBehaviour _idleBehaviour;
    private IBehaviour _agroBehaviour;
    
    private Mover _mover;
    
    public bool Die { get; set; }
    public bool LockBehaviour { get; set; }
    public Transform Target { get; private set; }
    public Vector3 MoveDirection { get; set; }
    
    public void Init(IBehaviour idleBehaviour, IBehaviour agroBehaviour)
    {
        _idleBehaviour = idleBehaviour;
        _agroBehaviour = agroBehaviour;
        _activeBehaviour = _idleBehaviour;
        _mover = GetComponent<Mover>();
        MoveDirection = Vector3.zero;
    }

    private void Update()
    {
        _activeBehaviour.Behave();
        _mover.SetDirection(MoveDirection);
    }

    private void LateUpdate()
    {
        if (Die)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent(typeof(Player)) && _agroBehaviour != null)
        {
            ChangeBehaviour(_agroBehaviour);
            Target = other.transform;
            //Debug.Log($"{this.name}: Target {Target.name} spotted! \nActive behaviour: {_activeBehaviour.GetType()}");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent(typeof(Player)) && _idleBehaviour != null)
        {
            ChangeBehaviour(_idleBehaviour);
            //Debug.Log($"{this.name}: Target {Target.name} leave! \nActive behaviour: {_activeBehaviour.GetType()}");
            Target = null;
        }
    }
    
    private void ChangeBehaviour(IBehaviour behaviour)
    {
        if (LockBehaviour) return;
        _activeBehaviour = behaviour;
    }
}
