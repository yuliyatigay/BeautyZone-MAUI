using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Domain.Interfaces
{
    public interface IBeautyTechService
    {
        Task<ObservableCollection<BeautyTech>> GetAllBeautyTechs(bool forceRefresh = false);
        Task<BeautyTech?> CreateBeautyTechAsync(BeautyTech beautyTech);
        Task<BeautyTech> GetBeautyTechById(Guid id);
        Task<bool> UpdateBeautyTechAsync(BeautyTech beautyTech);
        Task DeleteBeautyTechAsync(Guid id);
    }
}
