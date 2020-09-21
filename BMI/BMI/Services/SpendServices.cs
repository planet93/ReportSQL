using BMI.Context;
using BMI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BMI.Services
{
    public class SpendServices : BaseService
    {
        public SpendServices(UnitOfWork db = null) : base(db)
        { }

        public List<Spend> GetSpends()
        {
            var spend1List = mDb.Classifiers.GetAll().Where(x => x.ClassifierType.Name == "Spend 1").ToList();

            List<Spend> res = new List<Spend>();

            for (int s1 = 0; s1 < spend1List.Count; s1++)
            {
                var spend1 = spend1List[s1];

                var spend2list = mDb.Classifiers.GetAll().Where(x => x.ClassifierType.Name == "Spend 2" && x.Parent != null && x.Parent.Id == spend1.Id).ToList();

                List<Spend> res2 = new List<Spend>();

                for (int s2 = 0; s2 < spend2list.Count; s2++)
                {
                    var spend2 = spend2list[s2];

                    var spend3list = mDb.Classifiers.GetAll().Where(x => x.ClassifierType.Name == "Spend 3" && x.Parent != null && x.Parent.Id == spend2.Id).ToList();

                    List<Spend> res3 = new List<Spend>();

                    for (int s3 = 0; s3 < spend3list.Count; s3++)
                    {
                        var spend3 = spend3list[s3];

                        var spend4list = mDb.Classifiers.GetAll().Where(x => x.ClassifierType.Name == "Spend 4" && x.Parent != null && x.Parent.Id == spend3.Id).ToList();

                        List<Spend> res4 = new List<Spend>();
                        for (int s4 = 0; s4 < spend4list.Count; s4++)
                        {
                            var spend4 = spend4list[s4];
                            res4.Add(new Spend
                            {
                                Level = 4,
                                Name = spend4.Name,
                            });
                        }

                        res3.Add(new Spend
                        {
                            Level =3,
                            Name =spend3.Name,
                            SubSpend = res4,
                        });
                    }
                    res2.Add(new Spend
                    {
                        Level = 2,
                        Name = spend2.Name,
                        SubSpend = res3,
                    });
                }
                res.Add(new Spend
                {
                    Level = 1,
                    Name = spend1.Name,
                    SubSpend = res2,
                });
            }

            return res;
        }

        public List<Spend> GetSpendsFast()
        {
            var classifiers = mDb.Classifiers.GetAll().Include(x => x.Parent).ToList();

            var spend1List = classifiers.Where(x => x.ClassifierType.Name == "Spend 1").ToList();

            List<Spend> res = new List<Spend>();

            for (int s1 = 0; s1 < spend1List.Count; s1++)
            {
                var spend1 = spend1List[s1];

                var spend2list = classifiers.Where(x => x.ClassifierType.Name == "Spend 2" && x.Parent != null && x.Parent.Id == spend1.Id).ToList();

                List<Spend> res2 = new List<Spend>();

                for (int s2 = 0; s2 < spend2list.Count; s2++)
                {
                    var spend2 = spend2list[s2];

                    var spend3list = classifiers.Where(x => x.ClassifierType.Name == "Spend 3" && x.Parent != null && x.Parent.Id == spend2.Id).ToList();

                    List<Spend> res3 = new List<Spend>();

                    for (int s3 = 0; s3 < spend3list.Count; s3++)
                    {
                        var spend3 = spend3list[s3];

                        var spend4list = classifiers.Where(x => x.ClassifierType.Name == "Spend 4" && x.Parent != null && x.Parent.Id == spend3.Id).ToList();

                        List<Spend> res4 = new List<Spend>();
                        for (int s4 = 0; s4 < spend4list.Count; s4++)
                        {
                            var spend4 = spend4list[s4];
                            res4.Add(new Spend
                            {
                                Level = 4,
                                Name = spend4.Name,
                            });
                        }

                        res3.Add(new Spend
                        {
                            Level = 3,
                            Name = spend3.Name,
                            SubSpend = res4,
                        });
                    }
                    res2.Add(new Spend
                    {
                        Level = 2,
                        Name = spend2.Name,
                        SubSpend = res3,
                    });
                }
                res.Add(new Spend
                {
                    Level = 1,
                    Name = spend1.Name,
                    SubSpend = res2,
                });
            }

            return res;
        }

    }
}