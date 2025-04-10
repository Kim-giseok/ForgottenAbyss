using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
    Jump,
    Dash,
    Attack,
    Interaction,
    Climb
}

public abstract class PlayerStateMachine 
{
    protected ControllerPlayer player;

    public PlayerStateMachine(ControllerPlayer player)
    {
        this.player = player;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnJump() { }
    public virtual void OnDash() { }
    public virtual void OnAttack() { }
    public virtual void OnInteraction() { }
    public virtual void OnCollisionEnter(Collision2D collision) { }
    public virtual void OnMove(Vector2 inputVec) { }
}
