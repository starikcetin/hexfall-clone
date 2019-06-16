using System;
using System.Collections;
using System.Collections.Generic;
using Eflatun.UnityCommon.Utils.CodePatterns;
using starikcetin.hexfallClone;
using UnityEngine;

public class GameManager : SceneSingleton<GameManager>
{
    public event Action ActionSequenceCompleted;

    private bool _isSelectionActive = false;

    private HexagonGroup _selectedGroup;
    private GameObject _highlightGameObject;

    private readonly Queue<HexagonGroup> _matches = new Queue<HexagonGroup>();
    private InputManager _inputManager;
    private bool _matchFound;

    private void Start()
    {
        _inputManager = GetComponentInChildren<InputManager>();

        _inputManager.Swiped += InputManagerOnSwiped;
        _inputManager.Tapped += InputManagerOnTapped;

        ScoreDatabase.Instance.ResetScore();
    }

    protected void OnDestroy()
    {
        _inputManager.Swiped -= InputManagerOnSwiped;
        _inputManager.Tapped -= InputManagerOnTapped;
    }

    private void InputManagerOnTapped(Vector3 worldPosition)
    {
        Debug.Log($"{nameof(GameManager)}: {nameof(InputManagerOnTapped)}({nameof(worldPosition)}: {worldPosition})");

        var closestGroup =
            HexagonGroupDatabase.Instance.FindClosestGroup(worldPosition, GameParamsDatabase.Instance.Size);
        Debug.Log("Center of closest group: " + closestGroup.Center);

        SelectGroup(closestGroup);
    }

    private void SelectGroup(HexagonGroup group)
    {
        Debug.Log($"{nameof(GameManager)}: {nameof(SelectGroup)}({nameof(group)}: {group.Center})");
        _selectedGroup = group;
        _isSelectionActive = true;

        Destroy(_highlightGameObject);
        _highlightGameObject =
            Utils._Debug_Highlight((Vector3) _selectedGroup.Center - new Vector3(0, 0, 1), Color.green);
    }

    private void InputManagerOnSwiped(SwipeDirection swipeDirection)
    {
        Debug.Log($"{nameof(GameManager)}: {nameof(InputManagerOnSwiped)}({nameof(swipeDirection)}: {swipeDirection})");

        if (!_isSelectionActive)
        {
            Debug.Log($"{nameof(GameManager)}.{nameof(InputManagerOnSwiped)}: no active selection, ignoring swipe.");
            return;
        }

        switch (swipeDirection)
        {
            case SwipeDirection.Right:
                StartCoroutine(RotateSequence(RotationDirection.Clockwise));
                break;

            case SwipeDirection.Left:
                StartCoroutine(RotateSequence(RotationDirection.CounterClockwise));
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
        }
    }

    private IEnumerator RotateSequence(RotationDirection direction)
    {
        for (int i = 0; i < 3; i++)
        {
            switch (direction)
            {
                // rotate
                case RotationDirection.Clockwise:
                    yield return RotateOnce_Clockwise();
                    break;

                case RotationDirection.CounterClockwise:
                    yield return RotateOnce_CounterClockwise();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            // check for matches
            yield return CheckAndHandleMatches();

            if (_matchFound)
            {
                Debug.Log(
                    $"{nameof(GameManager)}.{nameof(RotateSequence)}: match found! breaking the rotation sequence.");

                ActionSequenceCompleted?.Invoke();
                yield break;
            }
        }

        ActionSequenceCompleted?.Invoke();
    }

    private IEnumerator RotateOnce_Clockwise()
    {
        var alphaHex = HexagonDatabase.Instance[_selectedGroup.Alpha];
        var bravoHex = HexagonDatabase.Instance[_selectedGroup.Bravo];
        var charlieHex = HexagonDatabase.Instance[_selectedGroup.Charlie];

        // Alpha Hex --> Bravo
        StartCoroutine(Put(alphaHex, _selectedGroup.Bravo));

        // Bravo Hex --> Charlie
        StartCoroutine(Put(bravoHex, _selectedGroup.Charlie));

        // Charlie Hex --> Alpha
        yield return Put(charlieHex, _selectedGroup.Alpha);

        // we only yield on one of them since they need to happen in parallel.
    }

    private IEnumerator RotateOnce_CounterClockwise()
    {
        var alphaHex = HexagonDatabase.Instance[_selectedGroup.Alpha];
        var bravoHex = HexagonDatabase.Instance[_selectedGroup.Bravo];
        var charlieHex = HexagonDatabase.Instance[_selectedGroup.Charlie];

        // Alpha Hex --> Charlie
        StartCoroutine(Put(alphaHex, _selectedGroup.Charlie));

        // Charlie Hex --> Bravo
        StartCoroutine(Put(charlieHex, _selectedGroup.Bravo));

        // Bravo Hex --> Alpha
        yield return Put(bravoHex, _selectedGroup.Alpha);

        // we only yield on one of them since they need to happen in parallel.
    }

    private IEnumerator Put(GameObject hex, OffsetCoordinates coords)
    {
        // set in hexagon database
        HexagonDatabase.Instance[coords] = hex;

        // sync the position of the GameObject
        yield return
            hex.GetComponent<Hexagon>().MoveTo(coords.ToUnity(GameParamsDatabase.Instance.Size), 0.25f);
    }

    private IEnumerator CheckAndHandleMatches()
    {
        _matchFound = RecordAllMatches();

        if (_matchFound)
        {
            HandleAllMatches();
            yield return RequestShift();
            yield return CheckAndHandleMatches();
            _matchFound = true;
        }
    }

    private void HandleAllMatches()
    {
        HashSet<OffsetCoordinates> hexagonsToExplode = GetHexagonsToExplode();

        foreach (var hexagon in hexagonsToExplode)
        {
            Explode(hexagon);
        }
    }

    private HashSet<OffsetCoordinates> GetHexagonsToExplode()
    {
        HashSet<OffsetCoordinates> hexagonsToExplode = new HashSet<OffsetCoordinates>();

        while (_matches.Count != 0)
        {
            var group = _matches.Dequeue();

            hexagonsToExplode.Add(group.Alpha);
            hexagonsToExplode.Add(group.Bravo);
            hexagonsToExplode.Add(group.Charlie);
        }

        return hexagonsToExplode;
    }

    private bool RecordAllMatches()
    {
        bool matchFound = false;

        foreach (var group in HexagonGroupDatabase.Instance.HexagonGroups)
        {
            var isMatch = CheckForMatch(group);

            if (isMatch)
            {
                matchFound = true;
                RecordMatch(group);
            }
        }

        return matchFound;
    }

    private void RecordMatch(HexagonGroup group)
    {
        _matches.Enqueue(group);
    }

    private IEnumerator RequestShift()
    {
        yield return GetComponent<GridShifter>().ShiftAndRefillAll();
    }

    private bool CheckForMatch(HexagonGroup group)
    {
        var (alpha, bravo, charlie) = HexagonDatabase.Instance[group];
        return Utils.IsSameColor(
            alpha.GetComponent<Hexagon>(),
            bravo.GetComponent<Hexagon>(),
            charlie.GetComponent<Hexagon>());
    }

    private void Explode(OffsetCoordinates coords)
    {
        var hex = HexagonDatabase.Instance[coords];
        HexagonDatabase.Instance.MarkAsDestroyed(coords);
        hex.GetComponent<Hexagon>().ExplodeSelf();

        ScoreDatabase.Instance.OnHexagonExploded();
    }
}
