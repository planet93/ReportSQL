using BMI.Context;
using BMI.Models;
using BMI.Services.Util;
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
        public List<Spend> GetSpendFromDDM()
        {
            string sql = $@"SELECT
                            mS1.Id AS Spend1_Id,
                            mS1.Name AS Spend1_Name,
                            mS2.Id AS Spend2_Id,
                            mS2.Name AS Spend2_Name,
                            mS3.Id AS Spend3_Id,
                            mS3.Name AS Spend3_Name,
                            mS4.Id AS Spend4_Id,
                            mS4.Name AS Spend4_Name

                            FROM
                            dbo.Classifier mS1
                            FULL OUTER JOIN dbo.Classifier mS2
                            ON mS1.Id=mS2.Parent_Id AND mS2.ClassifierType_Id=2
                            FULL OUTER JOIN dbo.Classifier mS3
                            ON mS2.Id=mS3.Parent_Id AND mS3.ClassifierType_Id=3
                            FULL OUTER JOIN dbo.Classifier mS4
                            ON mS3.Id=mS4.Parent_Id AND mS4.ClassifierType_Id=4

                            WHERE mS1.ClassifierType_Id=1";

            DirectDataManager ddm = new DirectDataManager(sql);
            var ListSpend = ddm.ToList<SpendDDM>().ToList();

            var spend1list = ListSpend.GroupBy(x => new { x.Spend1_Id, x.Spend1_Name }).ToList();

            List<Spend> res = new List<Spend>();

            for (int s1 = 0; s1 < spend1list.Count; s1++)
            {
                var spend1 = spend1list[s1];
                var spend2list = ListSpend.Where(x => x.Spend1_Id == spend1.Key.Spend1_Id && x.Spend2_Id != 0).GroupBy(x => new {x.Spend2_Id, x.Spend2_Name }).ToList();

                List<Spend> res2 = new List<Spend>();

                for (int s2 = 0; s2 < spend2list.Count; s2++)
                {
                    var spend2 = spend2list[s2];
                    var spend3list = ListSpend.Where(x => x.Spend2_Id == spend2.Key.Spend2_Id && x.Spend3_Id != 0).GroupBy(x => new { x.Spend3_Id, x.Spend3_Name }).ToList();

                    List<Spend> res3 = new List<Spend>();

                    for (int s3 = 0; s3 < spend3list.Count; s3++)
                    {
                        var spend3 = spend3list[s3];

                        var spend4list = ListSpend.Where(x => x.Spend3_Id == spend3.Key.Spend3_Id && x.Spend4_Id != 0).GroupBy(x => new { x.Spend4_Id, x.Spend4_Name }).ToList();

                        List<Spend> res4 = new List<Spend>();
                        for (int s4 = 0; s4 < spend4list.Count; s4++)
                        {
                            var spend4 = spend4list[s4];
                            res4.Add(new Spend
                            {
                                Id = spend4.Key.Spend4_Id,
                                Name = spend4.Key.Spend4_Name,
                                Level = 4,                                
                            });
                        }

                        res3.Add(new Spend
                        {
                            Id = spend3.Key.Spend3_Id,
                            Name = spend3.Key.Spend3_Name,
                            Level = 3,
                            SubSpend = res4,
                        });

                    }

                    res2.Add(new Spend
                    {
                        Id = spend2.Key.Spend2_Id,
                        Name = spend2.Key.Spend2_Name,
                        Level = 3,
                        SubSpend = res3,
                    });
                }

                res.Add(new Spend
                {
                    Id = spend1.Key.Spend1_Id,
                    Name = spend1.Key.Spend1_Name,
                    Level = 1,
                    SubSpend = res2,
                });

            }

            return res;
        }

    }
}