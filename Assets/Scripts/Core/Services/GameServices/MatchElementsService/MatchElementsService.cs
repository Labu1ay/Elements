using System.Collections.Generic;
using System.Linq;
using Elements.Game.Elements;
using UnityEngine;

namespace Elements.Core.Services.GameServices
{
    public class MatchElementsService : IMatchElementsService
    {
        public List<List<Element>> FindConnectedGroups(List<Element> elements)
        {
            var groups = new List<List<Element>>();
            var visitedPositions = new HashSet<Vector2Int>();
            var positionToElement = elements.ToDictionary(e => e.RoundPosition);

            foreach (var element in elements)
            {
                if (!visitedPositions.Add(element.RoundPosition)) continue;

                var connectedGroup = FindConnectedGroup(element, positionToElement, visitedPositions);
                if (connectedGroup.Count >= 3 && HasValidLine(connectedGroup, positionToElement))
                    groups.Add(connectedGroup);
            }

            return groups;
        }

        private List<Element> FindConnectedGroup(Element startElement,
            Dictionary<Vector2Int, Element> positionToElement, HashSet<Vector2Int> visitedPositions)
        {
            var connectedGroup = new List<Element>();
            var queue = new Queue<Element>();
            queue.Enqueue(startElement);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                connectedGroup.Add(current);

                foreach (var neighborPosition in GetNeighborPositions(current.RoundPosition))
                {
                    if (positionToElement.TryGetValue(neighborPosition, out var neighbor) &&
                        neighbor.Type == startElement.Type &&
                        visitedPositions.Add(neighborPosition))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return connectedGroup;
        }

        private bool HasValidLine(List<Element> group, Dictionary<Vector2Int, Element> positionToElement)
        {
            var groupPositions = group.Select(e => e.RoundPosition).ToHashSet();
            var targetType = group[0].Type;

            foreach (var element in group)
            {
                int horizontalCount = CountLine(element.RoundPosition, Vector2Int.right, groupPositions,
                                          positionToElement, targetType)
                                      + CountLine(element.RoundPosition, Vector2Int.left, groupPositions,
                                          positionToElement, targetType)
                                      + 1;

                if (horizontalCount >= 3) return true;

                int verticalCount = CountLine(element.RoundPosition, Vector2Int.up, groupPositions, positionToElement,
                                        targetType)
                                    + CountLine(element.RoundPosition, Vector2Int.down, groupPositions,
                                        positionToElement, targetType)
                                    + 1;

                if (verticalCount >= 3) return true;
            }

            return false;
        }

        private int CountLine(Vector2Int startPosition, Vector2Int direction,
            HashSet<Vector2Int> groupPositions, Dictionary<Vector2Int, Element> positionToElement,
            ElementType targetType)
        {
            int count = 0;
            var currentPosition = startPosition + direction;

            while (groupPositions.Contains(currentPosition) &&
                   positionToElement.TryGetValue(currentPosition, out var element) &&
                   element.Type == targetType)
            {
                count++;
                currentPosition += direction;
            }

            return count;
        }

        private IEnumerable<Vector2Int> GetNeighborPositions(Vector2Int position)
        {
            yield return position + Vector2Int.right;
            yield return position + Vector2Int.left;
            yield return position + Vector2Int.up;
            yield return position + Vector2Int.down;
        }
    }
}