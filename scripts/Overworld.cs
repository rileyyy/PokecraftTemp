using Godot;

public partial class Overworld : Node2D
{
  private MapManager mapManager;
  private Player player;

  public override void _Ready()
  {
    base._Ready();

    this.mapManager = this.GetNode<MapManager>("MapManager");
    this.mapManager.LoadInitialMap("World");

    this.player = this.GetNode<Player>("Player");
    this.player.GlobalPosition = World.InitialSpawn;
  }
}
