using System.Collections.Generic;

// navSurface 가 여러개일 수 있어서
public class NavManager
{
    public static NavManager instance { get; private set; } = new NavManager();
    public static List<NavSurface> navTileMapSurfaces = new(); 
}