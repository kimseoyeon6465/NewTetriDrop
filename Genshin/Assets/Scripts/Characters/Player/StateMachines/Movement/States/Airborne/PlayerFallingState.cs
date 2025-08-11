using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerFallingState : PlayerAirborneState
    {
        private PlayerFallData fallData;
        public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            fallData = airborneData.FallData;
        }
        #region IState Methods
        //Interface(상태) 패턴을 나타내는 함수들
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            ResetVerticalVelocity();
        }


        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVelocity();
        }
        #endregion

        #region Reusable Methods
        protected override void ResetSprintState()
        {
        }

        //protected override void OnContactWithGround(Collider collider)
        //{
        //    Debug.Log("[FallingState] Ground contact detected → Idle");
        //    base.OnContactWithGround(collider);
        //    stateMachine.ChangeState(stateMachine.IdlingState);
        //}

        #endregion

        #region Main Methods
        private void LimitVerticalVelocity()//너무 빠르게 낙하하지 않도록 속도 조절
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            if (stateMachine.Player.Rigidbody.velocity.y >= -fallData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitedVelocity = new Vector3(0f, -fallData.FallSpeedLimit - stateMachine.Player.Rigidbody.velocity.y, 0f);
            stateMachine.Player.Rigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
        }
        #endregion
    }
}
