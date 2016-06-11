using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Managers
{
    public class InventoryDBManager
    {
        public List<InventoryDB> GetInventoryList(Role role)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var result = session.QueryOver<InventoryDB>().Where(x => x.Role == role);
                    return (List<InventoryDB>)result.List();
                }
            }
        }

        public void AddInventoryDB(InventoryDB inventoryDB)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(inventoryDB);
                    transaction.Commit();
                }
            }
        }

        public void ChangeEquipment(List<InventoryDB> list)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var temp in list)
                    {
                        session.Update(temp);
                    }
                    transaction.Commit();
                }
            }
        }

        public void UpdateInventoryDB(InventoryDB inventoryDB)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(inventoryDB);
                    transaction.Commit();
                }
            }
        }

        public void UpgradeEquipment(InventoryDB inventoryDB, Role role)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(inventoryDB);
                    session.Update(role);
                    transaction.Commit();
                }
            }
        }
    }
}
