using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


[CustomGridBrush(false, false, false, "Isolated Tile Brush")]
public class IsolatedTileBrush : GridBrushBase
{
    public Sprite spriteToPaint;
    public bool addCollisionBox;

    public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pivot)
    {
        Vector3Int brushPosition = new Vector3Int(position.position.x, position.position.y, 0);
        Tilemap map = brushTarget.GetComponent<Tilemap>();
        Tile tile = map.GetTile<Tile>(brushPosition);
        if (tile)
        {
            spriteToPaint = tile.sprite;
        }
        else
        {
            Debug.Log("Brush position: " + brushPosition);
        }

    }
    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        // Do not edit the palette //
        if (brushTarget.layer == 31)
            return;

        // Instantiate our gameobject with the sprite that we want
        GameObject instance = new GameObject();
        SpriteRenderer renderer = instance.AddComponent <SpriteRenderer>();
        renderer.sprite = spriteToPaint;

        if(addCollisionBox)
        {
            instance.AddComponent<PolygonCollider2D>();
        }
        
        Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Tiles");
        if (instance != null)
        {
            // Set up the sorting order component
            instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, 0) + new Vector3(.5f, .5f, .5f)));
            instance.transform.SetParent(brushTarget.transform);
            instance.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        // Do not allow editing palettes
        if (brushTarget.layer == 31)
            return;

        Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, 0));
        if (erased != null)
            Undo.DestroyObjectImmediate(erased.gameObject);
    }

    private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
    {
        int childCount = parent.childCount;
        Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
        Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
        Bounds bounds = new Bounds((max + min) * .5f, max - min);

        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (bounds.Contains(child.position))
                return child;
        }
        return null;
    }
}

[CustomEditor(typeof(IsolatedTileBrush))]
public class IsolatedTileBrushEditor : GridBrushEditorBase
{
    private IsolatedTileBrush sortedTileBrush { get { return target as IsolatedTileBrush; } }

    public override void OnPaintInspectorGUI()
    {
        sortedTileBrush.addCollisionBox = EditorGUILayout.Toggle("Add Collision", sortedTileBrush.addCollisionBox);
    }
}
