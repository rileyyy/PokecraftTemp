using Godot;

public partial class TransitionZone : Area2D
{
  [Export]
  public string MapToLoad;

  [Export]
  public string MapToUnload;

  [Export]
  public string StitchNodeName;

  private MapManager mapManager;

  public override void _Ready()
  {
    this.mapManager = this.GetNode<MapManager>("/root/Overworld/MapManager");
  }

  private void OnBodyEntered(Node body)
  {
    if (body is Player player)
    {
      if (MapToLoad != null)
      {
        mapManager.LoadMap(
          this.GetParent<Node2D>().GetParent<Node2D>(),
          MapToLoad,
          StitchNodeName);
      }

      if (MapToUnload != null)
      {
        mapManager.UnloadMap(MapToUnload);
      }
    }
  }
}
