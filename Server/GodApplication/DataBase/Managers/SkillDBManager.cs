using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Managers
{
    public class SkillDBManager
    {
        public List<SkillDB> GetSkillDBList(Role role)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var result = session.QueryOver<SkillDB>().Where(x => x.Role == role);
                    return (List<SkillDB>)result.List();
                }
            }
        }

        public void AddSkill(SkillDB skillDB, Role role)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(role);
                    session.Save(skillDB);
                    transaction.Commit();
                }
            }
        }

        public void UpgradeSkill(SkillDB skillDB, Role role)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(role);
                    session.Save(skillDB);  //SaveOrUpdate
                    transaction.Commit();
                }
            }
        }
    }
}
