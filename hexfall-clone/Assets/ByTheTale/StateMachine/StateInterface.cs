namespace ByTheTale.StateMachine
{
    public interface StateInterface
    {
        void Initialize();

        void Enter();

        void Execute();
        void PhysicsExecute();
        void PostExecute();

        void Exit();

        void OnCollisionEnter(UnityEngine.Collision collision);
        void OnCollisionStay(UnityEngine.Collision collision);
        void OnCollisionExit(UnityEngine.Collision collision);

        void OnTriggerEnter(UnityEngine.Collider collider);
        void OnTriggerStay(UnityEngine.Collider collider);
        void OnTriggerExit(UnityEngine.Collider collider);

        void OnAnimatorIK(int layerIndex);

        bool isActive { get; }

        MachineInterface machine { get; }

        T GetMachine<T>() where T : MachineInterface;
    }
}