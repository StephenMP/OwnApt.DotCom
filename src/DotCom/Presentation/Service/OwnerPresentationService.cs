using DotCom.Model.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotCom.Presentation.Service
{
    public interface IOwnerPresentationService
    {
        Task<OwnerIndexModel> BuildIndexModelAsync(ClaimsPrincipal user);
    }

    public class OwnerPresentationService : IOwnerPresentationService
    {
        public async Task<OwnerIndexModel> BuildIndexModelAsync(ClaimsPrincipal user)
        {
            var model = new OwnerIndexModel();

            return await Task.FromResult(model);
        }
    }
}
