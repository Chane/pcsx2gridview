namespace PCSX2GridView.Backend
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicationAssetService : IApplicationAssetService
    {
        private readonly IGameMediaService gameMediaService;
        private readonly ICoverArtService coverArtService;

        public ApplicationAssetService(IGameMediaService gameMediaService, ICoverArtService coverArtService)
        {
            this.gameMediaService = gameMediaService;
            this.coverArtService = coverArtService;
        }

        public async Task<IList<GameModel>> Fetch()
        {
            var result = new List<GameModel>();

            var games = await this.gameMediaService.Fetch().ConfigureAwait(false);
            var art = await this.coverArtService.Fetch().ConfigureAwait(false);
            foreach (var game in games)
            {
                var coverArt = art.FirstOrDefault(
                    a => a.Name.Replace(".jpg", string.Empty, StringComparison.OrdinalIgnoreCase) ==
                                game.Name.Replace(".iso", string.Empty, StringComparison.OrdinalIgnoreCase));
                var model = new GameModel
                {
                    FileName = game.Name,
                    CoverArt = coverArt?.Name ?? string.Empty,
                };

                result.Add(model);
            }

            return result;
        }
    }
}