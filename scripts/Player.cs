using Godot;
using System;

public partial class Player : CharacterBody2D
{
  [Export]
  public float MovementSpeed = 100f;

  [Export]
  public string CurrentDirection = "down";

  private AnimatedSprite2D animation;

  public override void _Ready()
  {
    base._Ready();

    this.animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
  }

  private Vector2 lastPos;
  public override void _PhysicsProcess(double delta)
  {
    this.PlayerMovement(delta);
  }

  public void PlayerMovement(double delta)
  {
    this.Velocity = new Vector2(
      Input.GetActionStrength("right") - Input.GetActionStrength("left"),
      Input.GetActionStrength("down") - Input.GetActionStrength("up")).Normalized() * MovementSpeed;

    if (this.Velocity.Y > 0) this.CurrentDirection = "down";
    if (this.Velocity.Y < 0) this.CurrentDirection = "up";
    if (this.Velocity.X > 0) this.CurrentDirection = "right";
    if (this.Velocity.X < 0) this.CurrentDirection = "left";

    PlayAnimation((this.Velocity.X + this.Velocity.Y == 0));
    MoveAndSlide();
  }

  public void PlayAnimation(bool isMoving)
  {
    switch (this.CurrentDirection)
    {
      case "right":
        animation.FlipH = false;
        animation.Play(isMoving ? "side_idle" : "side_walk");
        break;

      case "left":
        animation.FlipH = true;
        animation.Play(isMoving ? "side_idle" : "side_walk");
        break;

      case "up":
        animation.FlipH = false;
        animation.Play(isMoving ? "back_idle" : "back_walk");
        break;

      case "down":
        animation.FlipH = false;
        animation.Play(isMoving ? "front_idle" : "front_walk");
        break;

      default:
        animation.FlipH = false;
        animation.Play("front_idle");
        break;
    }
  }

  public void OnSpawn(Vector2 position, string direction)
  {
    this.GlobalPosition = position;
    this.CurrentDirection = direction;
  }
}
