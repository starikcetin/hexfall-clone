using UnityEngine;

public class ExampleCharacterIdle : ByTheTale.StateMachine.State
{
    public ExampleCharacter exampleCharacter { get { return (ExampleCharacter)machine; } }
    public float timeInState { get; protected set; }

    public override void Enter()
    {
        base.Enter();

        timeInState = 0;
    }

    public override void Execute()
    {
        base.Execute();

        timeInState += Time.deltaTime;

        if (exampleCharacter.TimeToIdle <= timeInState)
        {
            machine.ChangeState<ExampleCharacterWander>();
        }
    }
}
