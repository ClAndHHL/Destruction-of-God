using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Managers
{
    public class RoleManager
    {
        public List<Role> GetRoleListByUser(User user)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    var result = session.QueryOver<Role>().Where(role => role.User == user);
                    transcation.Commit();
                    return (List<Role>)result.List<Role>();
                }
            }
        }

        public void AddRole(Role role)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    session.Save(role);
                    transcation.Commit();
                }
            }
        }

        public void UpdateRole(Role role)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    session.Update(role);
                    transcation.Commit();
                }
            }
        }
    }
}
