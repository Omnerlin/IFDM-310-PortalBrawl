using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;

[CustomGridBrush(false, false, false, "Sorted Tile Brush")]
public class SortedTileBrush : GridBrushBase
{
    public GameObject tilePrefab;
    public Sprite sprite;
    public int m_Z;

    public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pivot)
    {
        // Want to make sure people can only pick one prefab at a time
        //BoundsInt bounds = new BoundsInt(min, randomBrush.size);
        //if(position.allPositionsWithin.MoveNext() == position.allPositionsWithin.)
        //{
        //    Debug.LogWarning("You can only pick one tile at a time with sortedTileBrush!");
        //    return;
        //}

        Vector3Int brushPosition = new Vector3Int(position.position.x, position.position.y, 0);
        Tilemap map = brushTarget.GetComponent<Tilemap>();
        PrefabTile tile = map.GetTile<PrefabTile>(brushPosition);
        if (tile)
        {
            tilePrefab = tile.tilePrefab;
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

        GameObject instance = Instantiate(tilePrefab);
        Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");
        if (instance != null)
        {
            // Set up the sorting order component
            instance.transform.SetParent(brushTarget.transform);
            instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, m_Z) + new Vector3(.5f, .5f, .5f)));
            instance.transform.localScale = new Vector3(1,1,1);

            if(instance.GetComponent<SortingGroup>())
            {
                instance.GetComponent<SortingGroup>().sortingOrder = m_Z;
            }

            // Get rid of the sprite collider if should be higher than the player
            if(m_Z > 0)
            {
                instance.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        // Do not allow editing palettes
        if (brushTarget.layer == 31)
            return;

        Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, m_Z));
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

[CustomEditor(typeof(SortedTileBrush))]
public class PrefabBrushEditor : GridBrushEditorBase
{
    private SortedTileBrush sortedTileBrush { get { return target as SortedTileBrush; } }

    private SerializedProperty tilePrefab;
    private SerializedObject m_SerializedObject;

    protected void OnEnable()
    {
        m_SerializedObject = new SerializedObject(target);
        tilePrefab = m_SerializedObject.FindProperty("tilePrefab");
    }

    public override void OnPaintInspectorGUI()
    {
        m_SerializedObject.UpdateIfRequiredOrScript();
        sortedTileBrush.m_Z = EditorGUILayout.IntField("Position Z", sortedTileBrush.m_Z);

        EditorGUILayout.PropertyField(tilePrefab, true);
        m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
    }
}