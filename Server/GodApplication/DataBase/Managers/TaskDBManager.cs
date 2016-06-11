using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Managers
{
    public class TaskDBManager
    {
        public List<TaskDB> GetTaskDBList(Role role)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var result = session.QueryOver<TaskDB>().Where(x => x.Role == role);
                    transaction.Commit();
                    return (List<TaskDB>)result.List();
                }
            }
        }

        public void AddTaskDB(TaskDB taskDB)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    taskDB.LastUpdateTime = DateTime.Now;  //使用服务端的时间
                    session.Save(taskDB);
                    transaction.Commit();
                }
            }
        }

        public void UpdateTaskDB(TaskDB taskDB)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    taskDB.LastUpdateTime = DateTime.Now;  //使用服务端的时间
                    session.Update(taskDB);
                    transaction.Commit();
                }
            }
        }
    }
}
