using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using UnityEditor;

namespace UnityEngine
{
    //[Serializable]
    [CreateAssetMenu]
    public class PrefabTile : TileBase
    {
        // public settings to set for the tile
        public Sprite defaultSprite;
        public GameObject tilePrefab;
        public Tile.ColliderType m_DefaultColliderType;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = defaultSprite;
            tileData.colliderType = m_DefaultColliderType;
            tileData.flags = TileFlags.LockTransform;
            tileData.transform = Matrix4x4.identity;
            // We don't event want our tile to actually use the prefab. It is too glitchy, so screw that noise
            // we'll just get the prefab data from the SortedTileBrush class
            tileData.gameObject = null;
        }

        public override void RefreshTile(Vector3Int location, ITilemap tileMap)
        {
            base.RefreshTile(location, tileMap);
        }
    }
}
