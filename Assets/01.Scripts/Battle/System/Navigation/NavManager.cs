using System.Collections.Generic;

public class NavManager
{
    public static NavManager instance { get; private set; } = new NavManager();
    public static List<NavSurface> navTileMapSurfaces = new(); 
}