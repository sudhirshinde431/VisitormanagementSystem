using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorsManagement.Models
{
    public interface IWorkContext
    {
        int CurrentUserId { get; }
        CurrentUserDto currentUserDto { get; set; }
        void SetCurrentUserID(int userId);

    }
    public class WorkContext : IWorkContext
    {
        private int _currentUserId;

        //public int CurrentUserId => throw new NotImplementedException();

        public CurrentUserDto currentUserDto { get ; set ; }

        public void SetCurrentUserID(int userId)
        {
            _currentUserId = userId;
        }
    }
}
