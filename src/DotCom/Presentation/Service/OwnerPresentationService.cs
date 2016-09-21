using OwnApt.DotCom.Model.Owner;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Presentation.Service
{
    public interface IOwnerPresentationService
    {
        #region Public Methods

        Task<OwnerIndexModel> BuildIndexModelAsync(ClaimsPrincipal user);

        #endregion Public Methods
    }

    public class OwnerPresentationService : IOwnerPresentationService
    {
        #region Public Methods

        public async Task<OwnerIndexModel> BuildIndexModelAsync(ClaimsPrincipal user)
        {
            var model = new OwnerIndexModel();

            return await Task.FromResult(model);
        }

        #endregion Public Methods
    }
}
