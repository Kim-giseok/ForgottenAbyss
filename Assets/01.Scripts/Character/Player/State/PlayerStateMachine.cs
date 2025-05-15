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
    Climb,
    Slide,
    Fall,
    Drop
}

public abstract class PlayerStateMachine 
{
    protected ControllerPlayer player;
    protected PlayerSound playerSound;

    public PlayerStateMachine(ControllerPlayer player)
    {
        this.player = player;
        this.playerSound = player.GetComponent<PlayerSound>();
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
    public virtual void OnCollisionExit(Collision2D collision) { }
    public virtual void OnTriggerEnter(Collider2D collision) { }
    public virtual void OnTriggerExit(Collider2D collision) { }
    public virtual void OnTriggerStay(Collider2D collision) { }
    public virtual void OnMove(Vector2 inputVec) { }
}
