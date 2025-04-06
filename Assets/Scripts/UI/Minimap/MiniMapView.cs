using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapView : MonoBehaviour
{
    public RectTransform playerIcon;
    public Dictionary<MapData, Color> data;
    public GameObject iconPrefab;
    public Transform iconParent;

    [Header("World °Ê MiniMap ∫Ò¿≤")]
    private float worldToMapScale;

    public void SetWorldToMapScale(float _worldToMapScale)
    {
        worldToMapScale = _worldToMapScale;
    }

    public void UpdatePlayerIcon(Vector3 playerWorldPosition)
    {
        playerIcon.anchoredPosition = GetMiniMapPos(playerWorldPosition);
    }

    public void GenerateIcons(MapData data, List<Vector3> posList)
    {
        Color color = GetDataColor(data);
        foreach (Vector3 pos in posList)
        {
            GameObject icon = Instantiate(iconPrefab, iconParent.transform);
            RectTransform tf = icon.GetComponent<RectTransform>();
            Image img = icon.GetComponent<Image>();
            tf.anchoredPosition = GetMiniMapPos(pos);
            img.color = color;
        }
    }

    public Color GetDataColor(MapData data)
    {
        switch (data)
        {
            case MapData.MONEY:
                return Color.green;
            case MapData.POSTIT:
                return Color.yellow;
            case MapData.MAIL:
                return Color.magenta;
            case MapData.COFFEE:
                return new Color(0.6f, 0.3f, 0.1f); //brown color
            case MapData.FILE_STACK:
                return Color.red;
            case MapData.START_POS:
                return Color.black;
        }

        return Color.black;
    }

    public Vector2 GetMiniMapPos(Vector3 pos)
    {
        return new Vector2(pos.x, pos.z) * worldToMapScale;
    }
}