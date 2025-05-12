using Godot;
using System;
using System.Linq;

public partial class Door : Area2D
{
  [Export]
  public string DestinationSceneName = string.Empty;

  [Export]
  public string DestinationDoorName = string.Empty;

  [Export]
  public string SpawnDirection = "up";

  public Marker2D SpawnLocation;

  private MapManager mapManager;

  public override void _Ready()
  {
    this.SpawnLocation = this.GetNode<Marker2D>("SpawnLocation");
    this.mapManager = this.GetNode<MapManager>("/root/Overworld/MapManager");
  }

  public void OnBodyEntered(Node2D body)
  {
    if (body is Player player)
    {
      this.CallDeferred(nameof(this.LoadNewMapUnderPlayer), player);
    }
  }

  /// <summary>
  /// When a player enters the door, replace the map with the destination map (identified by DestinationSceneName)
  /// and ensure the player is located on the spawnMarker facing SpawnDirection.
  /// </summary>
  /// <param name="player">The player entity.</param>
  private void LoadNewMapUnderPlayer(Player player)
  {
    var newMap = mapManager.LoadNewMap(DestinationSceneName);
    if (newMap == null)
    {
      return;
    }

    var destinationDoor = newMap?
                            .GetChildren()
                            .First(node => node.Name == "Doors")
                            .GetChildren()
                            .OfType<Door>()
                            .FirstOrDefault(door => door.Name == DestinationDoorName);

    if (destinationDoor == null)
    {
      GD.PrintErr($"Destination door '{DestinationDoorName}' not found in '{DestinationSceneName}'");
      return;
    }

    var spawnMarker = destinationDoor.GetNode<Marker2D>("SpawnLocation");
    player.GlobalPosition = spawnMarker.GlobalPosition;
    player.CurrentDirection = destinationDoor.SpawnDirection;
  }
}
