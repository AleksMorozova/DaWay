using InstaBotLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Repositories
{
    public interface IBoundRepository
    {
        List<BoundModel> getUserBounds(int userId);
        void AddBound(BoundModel bound);
        void UpdateBound(BoundModel bound);
        void DeleteBound(int boundId);
    }
}
