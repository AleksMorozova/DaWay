using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Bound
{
    public interface IBoundRepository
    {
        List<BoundModel> getUserBounds(int userId);
        void AddBound(BoundModel bound);
        void SetInstagramToken(BoundModel bound);
        void SetInstagramInfo(BoundModel bound);
        void UpdateBound(BoundModel bound);
        void DeleteBound(int boundId);
        List<BoundModel> getAllBounds();
    }
}
