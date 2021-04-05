namespace PCSX2GridView.Backend
{
    using System.Collections.Generic;

    public interface IApplicationAssetService
    {
        IList<GameModel> Fetch();
    }
}