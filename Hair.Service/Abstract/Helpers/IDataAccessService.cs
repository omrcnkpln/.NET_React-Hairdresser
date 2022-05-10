using Hair.Core.Models.System;

namespace Hair.Service.Abstract.Helpers
{
    public interface IDataAccessService
    {
        Task<bool> CheckAccess(int UserID, string Controller, string Action);
        
        Task<bool> CheckUserGrantFromToken(string Token, string ControllerName, string ActionName);
        
        IEnumerable<Page> ReadAllPAges();
    }
}