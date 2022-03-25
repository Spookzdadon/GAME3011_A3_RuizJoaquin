using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public sealed class BoardScript : MonoBehaviour
{
    public static BoardScript Instance { get; private set; }

    [SerializeField]
    private AudioClip matchSound;
    [SerializeField]
    private AudioSource audioSource;

    public RowScript[] rows;

    public TileScript[,] tiles { get; private set; }

    public int width => tiles.GetLength(0);
    public int height => tiles.GetLength(1);

    private readonly List<TileScript> selection = new List<TileScript>();
    private const float TweenDuration = 0.25f;
    private void Awake() => Instance = this;

    private void Start()
    {
        tiles = new TileScript[rows.Max(rows => rows.tiles.Length), rows.Length];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var tile = rows[y].tiles[x];
                tile.x = x;
                tile.y = y;

                tile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
                tiles[x, y] = tile;

            }
        }

        if (CanMatch())
        {
            MatchStart();
        }
    }

    public async void Select(TileScript tile)
    {
        if (!selection.Contains(tile))
        {
            if (selection.Count > 0)
            {
                if (System.Array.IndexOf(selection[0].Neighbours, tile) != -1)
                {
                    selection.Add(tile);
                }
            }
            else
            {
                selection.Add(tile);
            }
        }

        if (selection.Count < 2) return;

        print("Selected tiles at ({" + selection[0].x + "}, {" + selection[0].y + "}) and ({" + selection[1].x + "}, {" + selection[1].y + "})");

        await Swap(selection[0], selection[1]);

        if (CanMatch())
        {
            Match();
        }
        else
        {
            await Swap(selection[0], selection[1]);
        }

        selection.Clear();
    }

    public async Task Swap(TileScript tile1, TileScript tile2)
    {
        var icon1 = tile1.icon;
        var icon2 = tile2.icon;

        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();

        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration))
                .Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));

        await sequence.Play().AsyncWaitForCompletion();

        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);

        tile1.icon = icon2;
        tile2.icon = icon1;

        var tile1Item = tile1.Item;

        tile1.Item = tile2.Item;
        tile2.Item = tile1Item;
    }

    private bool CanMatch()
    {
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2) return true;
            }
        }

        return false;
    }

    private async void Match()
    {
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var tile = tiles[x, y];

                var connectedTiles = tile.GetConnectedTiles();

                if (connectedTiles.Skip(1).Count() < 2) continue;

                var shrinkSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    shrinkSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, TweenDuration));
                }

                audioSource.PlayOneShot(matchSound);

                ScoreCounter.Instance.Scores += tile.Item.value * connectedTiles.Count;

                await shrinkSequence.Play().AsyncWaitForCompletion();


                var expandSequene = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];

                    expandSequene.Join(connectedTile.icon.transform.DOScale(Vector3.one, TweenDuration));
                }

                await expandSequene.Play().AsyncWaitForCompletion();

                x = 0;
                y = 0;
            }
        }
    }

    private async void MatchStart()
    {
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var tile = tiles[x, y];

                var connectedTiles = tile.GetConnectedTiles();

                if (connectedTiles.Skip(1).Count() < 2) continue;

                var shrinkSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    shrinkSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, TweenDuration));
                }

                //audioSource.PlayOneShot(matchSound);

                //ScoreCounter.Instance.Scores += tile.Item.value * connectedTiles.Count;

                await shrinkSequence.Play().AsyncWaitForCompletion();


                var expandSequene = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];

                    expandSequene.Join(connectedTile.icon.transform.DOScale(Vector3.one, TweenDuration));
                }

                await expandSequene.Play().AsyncWaitForCompletion();

                x = 0;
                y = 0;
            }
        }
    }
}
