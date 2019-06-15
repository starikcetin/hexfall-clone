using ByTheTale.StateMachine;

namespace starikcetin.hexfallClone.game.code.gameStateMachine
{
    public class GameStateMachine : MachineBehaviour
    {
        public override void AddStates()
        {
            AddState<PlayState>();
            AddState<DiscoverState>();
            AddState<ExplodeState>();
            AddState<ShiftAndRefillState>();

            SetInitialState<PlayState>();
        }
    }
}
