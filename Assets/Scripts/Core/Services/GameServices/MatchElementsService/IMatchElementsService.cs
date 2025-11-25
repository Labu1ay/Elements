using System.Collections.Generic;
using Elements.Game.Elements;

namespace Elements.Core.Services.GameServices
{
    public interface IMatchElementsService
    {
        List<List<Element>> FindConnectedGroups(List<Element> elements);
    }
}