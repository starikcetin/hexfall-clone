namespace ByTheTale.StateMachine
{
    public interface MachineInterface
    {
        void SetInitialState<T>() where T : State;
        void SetInitialState(System.Type T);

        void ChangeState<T>() where T : State;
        void ChangeState(System.Type T);

        bool IsCurrentState<T>() where T : State;
        bool IsCurrentState(System.Type T);

        T CurrentState<T>() where T : State;
        T GetState<T>() where T : State;

        void AddState<T>() where T : State, new();
        void AddState(System.Type T);

        void RemoveState<T>() where T : State;
        void RemoveState(System.Type T);

        bool ContainsState<T>() where T : State;
        bool ContainsState(System.Type T);

        void OnCollisionEnter(UnityEngine.Collision collision);
        void OnCollisionStay(UnityEngine.Collision collision);
        void OnCollisionExit(UnityEngine.Collision collision);

        void OnTriggerEnter(UnityEngine.Collider collider);
        void OnTriggerStay(UnityEngine.Collider collider);
        void OnTriggerExit(UnityEngine.Collider collider);

        void RemoveAllStates();

        string name { get; set; }
    }
}