
namespace GenshinImpactMovementSystem
{
    public abstract class StateMachine
    {
        protected IState currentState;

        public void ChangeState(IState newState)
        {
            currentState?.Exit();//null 이 아니면 Exit()실행
            currentState = newState;
            currentState.Enter();

        }

        public void HandleInput()
        {
            currentState?.HandleInput();
        }
        public void Update()
        {
            currentState?.Update();
        }
        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate();
        }
    }
}
