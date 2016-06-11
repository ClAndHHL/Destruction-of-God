using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Managers
{
    public class UserManager
    {
        public User GetUserByUsername(string username)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    var result = session.QueryOver<User>().Where(x => x.Username == username);
                    transcation.Commit();
                    if (result.List() != null && result.List().Count > 0)
                    {
                        return result.List()[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public void AddUser(User user)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    session.Save(user);
                    transcation.Commit();
                }
            }
        }
    }
}
