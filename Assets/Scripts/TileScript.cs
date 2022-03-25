using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileScript : MonoBehaviour
{
    public int x;
    public int y;
    private ItemScript _item;
    public ItemScript Item
    {
        get => _item;

        set
        {
            if (_item == value) return;

            _item = value;
            icon.sprite = _item.sprite;
        }
    }
    public Image icon;
    public Button button;

    public TileScript Left => x > 0 ? BoardScript.Instance.tiles[x - 1, y] : null;
    public TileScript Top => y > 0 ? BoardScript.Instance.tiles[x, y - 1] : null;
    public TileScript Right => x < BoardScript.Instance.width - 1 ? BoardScript.Instance.tiles[x + 1, y] : null;
    public TileScript Bottom => y < BoardScript.Instance.height - 1 ? BoardScript.Instance.tiles[x, y + 1] : null;

    public TileScript[] Neighbours => new[]
    {
        Left,
        Top,
        Right,
        Bottom,
    };

    private void Start()
    {
        button.onClick.AddListener(() => BoardScript.Instance.Select(this));
    }

    public List<TileScript> GetConnectedTiles(List<TileScript> exclude = null)
    {
        var result = new List<TileScript> { this, };

        if (exclude == null)
        {
            exclude = new List<TileScript> { this, };
        }
        else
        {
            exclude.Add(this);
        }

        foreach (var neighbour in Neighbours)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item) continue;

            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }

        return result;
    }
}
