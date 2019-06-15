using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ExampleCharacter : ByTheTale.StateMachine.MachineBehaviour
{
    public Rigidbody rigidBody { get; protected set; }
    public CapsuleCollider capsuleCollider { get; protected set; }
    public float TimeToIdle { get { return timeToIdle; } }
    public float TimeToWander { get { return timeToWander; } }
    public float Speed { get { return speed; } }

    public override void AddStates()
    {
        AddState<ExampleCharacterIdle>();
        AddState<ExampleCharacterWander>();

        SetInitialState<ExampleCharacterIdle>();
    }

    public void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(Vector3.forward * 0.4F + Vector3.up * 0.05F, Vector3.forward * 0.8F + Vector3.right * 0.25F);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Vector3.right * 0.4F + Vector3.up * 0.05F, Vector3.forward * 0.25F + Vector3.right * 0.8F);
    }

    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (null != uiInfo)
        {
            if (IsCurrentState<ExampleCharacterIdle>())
            {
                uiInfo.text = name + " (Idle)";
            }
            else if (IsCurrentState<ExampleCharacterWander>())
            {
                uiInfo.text = name + " (Wandering)";
            }

            RectTransform rt = (RectTransform)uiInfo.transform;
            float height = capsuleCollider.height;
            Vector2 P = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position + Vector3.up * height);
            rt.position = P;
        }
    }

    [SerializeField] protected Text uiInfo;

    [SerializeField] protected float timeToIdle = 1.337F;
    [SerializeField] protected float timeToWander = 3.37F;
    [Range(1, 10)][SerializeField] protected float speed = 7.33F;
}
