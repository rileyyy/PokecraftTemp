using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class MapManager : Node2D
{
  private List<Node2D> loadedMaps = new List<Node2D>();

  /// <summary>
  /// Loads mapName at GlobalPosition 0,0 as the first map.
  /// Should only be used on game start.
  /// </summary>
  /// <param name="mapName">The map to load.</param>
  public void LoadInitialMap(string mapName)
  {
    if (loadedMaps.Any(map => map.Name == mapName))
    {
      return;
    }

    var newMap = (Node2D)ResourceLoader
                    .Load<PackedScene>($"scenes/{mapName}.tscn")
                    .Instantiate();

    AddChild(newMap);
    newMap.GlobalPosition = new Vector2(0, 0);
    loadedMaps.Add(newMap);
  }

  /// <summary>
  /// Loads the next map before the player should arrive to it.
  /// Both maps should have the same Node2D to stitch together in the syntax of Stitches/{stitchNodeName}.
  /// </summary>
  /// <param name="presentMap">The map the player is presently on.</param>
  /// <param name="mapName">The name of the map to be loaded.</param>
  /// <param name="stitchNodeName">The name of the Node2D where the two maps should be stitched together.</param>
  public void LoadMap(Node2D presentMap, string mapName, string stitchNodeName)
  {
    if (loadedMaps.Any(map => map.Name == mapName))
    {
      return;
    }

    var newMap = (Node2D)ResourceLoader
                    .Load<PackedScene>($"scenes/{mapName}.tscn")
                    .Instantiate();

    this.CallDeferred(nameof(this.AddNewMap), presentMap, newMap, stitchNodeName);
  }

  /// <summary>
  /// Loads and returns a map. should be switched.
  /// </summary>
  /// <param name="mapName">The name of the map to find and load.</param>
  /// <returns>The new map if found; else null.</returns>
  public Node2D LoadNewMap(string mapName)
  {
    var newMap = (Node2D)ResourceLoader
                    .Load<PackedScene>($"scenes/{mapName}.tscn")
                    .Instantiate();

    if (newMap == null)
    {
      GD.PrintErr($"Failed to load map: {mapName}");
      return null;
    }

    this.UnloadAllMaps();
    AddChild(newMap);
    loadedMaps.Add(newMap);
    return newMap;
  }

  /// <summary>
  /// Unloads all maps with a name that matches mapName.
  /// </summary>
  /// <param name="mapName">The name of the map(s) to remove.</param>
  public void UnloadMap(string mapName)
  {
    var maps = loadedMaps.Where(map => map.Name == mapName).ToList();
    foreach (var map in maps)
    {
      loadedMaps.Remove(map);
      map.QueueFree();
    }

    GD.Print($"Unloaded {maps.Count()} maps named {mapName}");
  }

  /// <summary>
  /// Unloads all maps in loadedMaps and frees them from memory.
  /// </summary>
  private void UnloadAllMaps()
  {
    // Might need to change this to unload all but the last world map so we don't have to load something so big
    foreach (var map in loadedMaps)
    {
      map.QueueFree();
    }
    loadedMaps.Clear();
  }

  /// <summary>
  /// Deferred call method to avoid changing the map on a physics callback.
  /// </summary>
  /// <param name="presentMap">The map the player is presently on.</param>
  /// <param name="newMap">The map to be loaded.</param>
  /// <param name="stitchNodeName">The name of the Node2D where the two maps should be stitched together.</param>
  private void AddNewMap(Node2D presentMap, Node2D newMap, string stitchNodeName)
  {
    AddChild(newMap);
    loadedMaps.Add(newMap);

    var oldStitchNode = presentMap.FindChild("Stitches", false, false).FindChild(stitchNodeName, false, false) as Node2D;
    var newStitchNode = newMap.FindChild("Stitches", false, false).FindChild(stitchNodeName, false, false) as Node2D;
    newMap.GlobalPosition += oldStitchNode.GlobalPosition - newStitchNode.GlobalPosition;

    GD.Print($"Loaded map: {newMap.Name}");
  }
}
