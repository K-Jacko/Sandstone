using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntity : MonoBehaviour
{
    private StateMachine _stateMachine;
    
    // Start is called before the first frame update
    void Awake()
    {
        _stateMachine = new StateMachine();
        At(new OriginColor(), new NewColor(), CloseToTarget());
        
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        Func<bool> CloseToTarget() => () => Target != null;
    }

    public void ChangeColor(Color color)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
