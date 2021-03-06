namespace PCSX2GridView.Backend
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public interface IApplicationAssetService
    {
        Task<IList<GameModel>> Fetch();

        Stream LoadCoverBitmap(string coverArt);
    }
}