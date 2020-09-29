using BMI.Context;
using BMI.Context.Model;
using BMI.Models;
using BMI.Services.ExcelDataReader;
using BMI.Services.Util;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;

namespace BMI.Services
{
    public class LoadDataServices:BaseService
    {
        public LoadDataServices(UnitOfWork db = null) : base(db)
        { }

        public ResultViewModel CreateClassifierType(List<LoadDataViewModel> model)
        {
            List<string> lineError = new List<string>();

            for(int i = 0; i < model.Count; i++)
            {
                var item = model[i];

                var name = model[i].Name.ToLower().Trim();

                var classType = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name.ToLower().Trim() == name);

                if(classType == null)
                {
                    ClassifierType newType = new ClassifierType
                    {
                        Name = item.Name,
                    };

                    mDb.ClassifierTypes.Add(newType);
                    mDb.ClassifierTypes.Save();
                }
            }

            return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
        }

        public ResultViewModel CreateSpend(List<LoadDataViewModel> list)
        {
            List<string> lineError = new List<string>();
            try
            {
                var spend1Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 1");
                var spend2Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 2");
                var spend3Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 3");
                var spend4Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 4");


                var spend1list = list.Where(x => x.Type.ToLower().Trim() == spend1Type.Name.ToLower().Trim()).ToList();
                var spend2list = list.Where(x => x.Type.ToLower().Trim() == spend2Type.Name.ToLower().Trim()).ToList();
                var spend3list = list.Where(x => x.Type.ToLower().Trim() == spend3Type.Name.ToLower().Trim()).ToList();
                var spend4list = list.Where(x => x.Type.ToLower().Trim() == spend4Type.Name.ToLower().Trim()).ToList();

                for (int i = 0; i < spend1list.Count; i++)
                {
                    var spend1 = spend1list[i];
                    Classifier temp = mDb.Classifiers.GetAll().FirstOrDefault(x => x.Name.ToLower().Trim() == spend1.Name.ToLower().Trim() && x.ClassifierType != null && x.ClassifierType.Id == spend1Type.Id);
                    if (temp == null)
                    {
                        temp = new Classifier
                        {
                            ClassifierType = spend1Type,
                            Name = spend1.Name,
                        };
                        mDb.Classifiers.Add(temp);
                        mDb.Classifiers.Save();
                    }

                    var subspend2 = spend2list.Where(x => x.Parent.ToLower().Trim() == temp.Name.ToLower().Trim()).ToList();
                    for (int s2 = 0; s2 < subspend2.Count; s2++)
                    {
                        var spend2 = subspend2[s2];
                        Classifier tempSpend2 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.Name.ToLower().Trim() == spend2.Name.ToLower().Trim() && x.ClassifierType != null && x.ClassifierType.Id == spend2Type.Id);
                        if (tempSpend2 == null)
                        {
                            tempSpend2 = new Classifier
                            {
                                ClassifierType = spend2Type,
                                Name = spend2.Name,
                                Parent = temp
                            };
                            mDb.Classifiers.Add(tempSpend2);
                            mDb.Classifiers.Save();
                        }

                        var subspend3 = spend3list.Where(x => x.Parent.ToLower().Trim() == tempSpend2.Name.ToLower().Trim()).ToList();
                        for (int s3 = 0; s3 < subspend3.Count; s3++)
                        {
                            var spend3 = subspend3[s3];
                            Classifier tempSpend3 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.Name.ToLower().Trim() == spend3.Name.ToLower().Trim() && x.ClassifierType != null && x.ClassifierType.Id == spend3Type.Id);
                            if (tempSpend3 == null)
                            {
                                tempSpend3 = new Classifier
                                {
                                    ClassifierType = spend3Type,
                                    Name = spend3.Name,
                                    Parent = tempSpend2
                                };
                                mDb.Classifiers.Add(tempSpend3);
                                mDb.Classifiers.Save();
                            }

                            var subspend4 = spend4list.Where(x => x.Parent.ToLower().Trim() == tempSpend3.Name.ToLower().Trim()).ToList();
                            for (int s4 = 0; s4 < subspend4.Count; s4++)
                            {
                                var spend4 = subspend4[s4];
                                Classifier tempSpend4 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.Name.ToLower().Trim() == spend4.Name.ToLower().Trim() && x.ClassifierType != null && x.ClassifierType.Id == spend4Type.Id);
                                if (tempSpend4 == null)
                                {
                                    tempSpend4 = new Classifier
                                    {
                                        ClassifierType = spend4Type,
                                        Name = spend4.Name,
                                        Parent = tempSpend3
                                    };
                                    mDb.Classifiers.Add(tempSpend4);
                                    mDb.Classifiers.Save();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var warning = ex.Message;
            }

            return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
        }

        public ResultViewModel CreateSpend4Level(List<LoadDataViewModel> list)
        {
            List<string> lineError = new List<string>();
            try
            {
                var spend1Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 1");
                var spend2Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 2");
                var spend3Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 3");
                var spend4Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 4");

                var groupBySpend1 = list.GroupBy(x => x.Spend1).ToList();

                for (int i = 0; i < groupBySpend1.Count; i++)
                {
                    var spend1Name = groupBySpend1[i].Key;

                    Classifier spend1 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend1Type.Id && x.Name.ToLower().Trim() == spend1Name.ToLower().Trim());

                    if (spend1 != null)
                    {
                        lineError.Add($"{spend1Name} - Already Spend1");
                    }
                    else
                    {
                        spend1 = new Classifier
                        {
                            ClassifierType = spend1Type,
                            Name = spend1Name,
                        };
                        mDb.Classifiers.Add(spend1);
                        mDb.Classifiers.Save();
                    }

                }

                var groupBySpend2 = list.GroupBy(x => new { x.Spend1, x.Spend2 }).ToList();
                for (int i = 0; i < groupBySpend2.Count; i++)
                {
                    var spend1Name = groupBySpend2[i].Key.Spend1;
                    var spend2Name = groupBySpend2[i].Key.Spend2;

                    var spend1 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend1Type.Id && x.Name.ToLower().Trim() == spend1Name.ToLower().Trim());

                    if (spend1 == null)
                    {
                        lineError.Add($"Spend1 - {spend1Name} not found. Spend2 - {spend2Name}");
                    }
                    else
                    {
                        var spend2 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend2Type.Id && x.Name.ToLower().Trim() == spend2Name.ToLower().Trim() &&
                        x.Parent != null && x.Parent.Id == spend1.Id);

                        if (spend2 != null)
                        {
                            lineError.Add($"Spend1 - {spend1Name}, Spend2 - {spend2Name} already");
                        }
                        else
                        {
                            spend2 = new Classifier
                            {
                                ClassifierType = spend2Type,
                                Name = spend2Name,
                                Parent = spend1
                            };
                            mDb.Classifiers.Add(spend2);
                            mDb.Classifiers.Save();
                        }
                    }

                }

                var groupBySpend3 = list.Where(x => x.Spend3 != null).GroupBy(x => new { x.Spend1, x.Spend2, x.Spend3 }).ToList();

                for (int i = 0; i < groupBySpend3.Count; i++)
                {
                    var spend1Name = groupBySpend3[i].Key.Spend1;
                    var spend2Name = groupBySpend3[i].Key.Spend2;
                    var spend3Name = groupBySpend3[i].Key.Spend3;

                    var spend1 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend1Type.Id && x.Name.ToLower().Trim() == spend1Name.ToLower().Trim());

                    if (spend1 == null)
                    {
                        lineError.Add($"Spend1 - {spend1Name} not found. Spend2 - {spend2Name}, Spend3 - {spend3Name}");
                    }
                    else
                    {
                        var spend2 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend2Type.Id && x.Name.ToLower().Trim() == spend2Name.ToLower().Trim() &&
                        x.Parent != null && x.Parent.Id == spend1.Id);

                        if (spend2 == null)
                        {
                            lineError.Add($"Spend1 - {spend1Name}. Spend2 - {spend2Name} not found, Spend3 - {spend3Name}");
                        }
                        else
                        {
                            var spend3 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend3Type.Id && x.Name.ToLower().Trim() == spend3Name.ToLower().Trim() &&
                            x.Parent != null && x.Parent.Id == spend2.Id);

                            if (spend3 != null)
                            {
                                lineError.Add($"Spend1 - {spend1Name}, Spend2 - {spend2Name}, Spend3 - {spend3Name} already");
                            }
                            else
                            {
                                spend3 = new Classifier
                                {
                                    ClassifierType = spend3Type,
                                    Name = spend3Name,
                                    Parent = spend2
                                };
                                mDb.Classifiers.Add(spend3);
                                mDb.Classifiers.Save();
                            }
                        }
                    }
                }

                var groupBySpend4 = list.Where(x => x.Spend3 != null && x.Spend4 != null).GroupBy(x => new { x.Spend1, x.Spend2, x.Spend3, x.Spend4 }).ToList();
                for (int i = 0; i < groupBySpend4.Count; i++)
                {
                    var spend1Name = groupBySpend4[i].Key.Spend1;
                    var spend2Name = groupBySpend4[i].Key.Spend2;
                    var spend3Name = groupBySpend4[i].Key.Spend3;
                    var spend4Name = groupBySpend4[i].Key.Spend4;

                    var spend1 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend1Type.Id && x.Name.ToLower().Trim() == spend1Name.ToLower().Trim());

                    if (spend1 == null)
                    {
                        lineError.Add($"Spend1 - {spend1Name} not found. Spend2 - {spend2Name}, Spend3 - {spend3Name}, Spend4 - {spend4Name}");
                    }
                    else
                    {
                        var spend2 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend2Type.Id && x.Name.ToLower().Trim() == spend2Name.ToLower().Trim() &&
                        x.Parent != null && x.Parent.Id == spend1.Id);

                        if (spend2 == null)
                        {
                            lineError.Add($"Spend1 - {spend1Name}. Spend2 - {spend2Name} not found, Spend3 - {spend3Name}, Spend4 - {spend4Name}");
                        }
                        else
                        {
                            var spend3 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend3Type.Id && x.Name.ToLower().Trim() == spend3Name.ToLower().Trim() &&
                            x.Parent != null && x.Parent.Id == spend2.Id);

                            if (spend3 == null)
                            {
                                lineError.Add($"Spend1 - {spend1Name}. Spend2 - {spend2Name} , Spend3 - {spend3Name} not found, Spend4 - {spend4Name}");
                            }
                            else
                            {
                                Classifier spend4 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend4Type.Id && x.Name.ToLower().Trim() == spend4Name.ToLower().Trim() &&
                                x.Parent != null && x.Parent.Id == spend3.Id);

                                if (spend4 != null)
                                {
                                    lineError.Add($"Spend1 - {spend1Name}, Spend2 - {spend2Name}, Spend3 - {spend3Name}, Spend4 - {spend4Name} already");
                                }
                                else
                                {
                                    spend4 = new Classifier
                                    {
                                        ClassifierType = spend4Type,
                                        Name = spend4Name,
                                        Parent = spend3
                                    };
                                    mDb.Classifiers.Add(spend4);
                                    mDb.Classifiers.Save();
                                }
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var warning = ex.Message;
            }

            return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
        }

        public ResultViewModel LoadActivities(List<LoadDataViewModel> list)
        {
            List<string> lineError = new List<string>();
            try
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
                var SpendList = ddm.ToList<SpendDDM>().ToList();

                var MonthList = mDb.Classifiers.GetAll().Where(x => x.ClassifierType != null && x.ClassifierType.Name == "Month").ToList();

                List<LoadEstimation> NewEstimateList = new List<LoadEstimation>();

                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];

                    var Spend1 = SpendList.FirstOrDefault(x => x.Spend1_Name.ToLower() == item.Spend1.ToLower().Trim());
                    if (Spend1 != null)
                    {
                        item.Spend1Id = Spend1.Spend1_Id;
                    }
                    else
                    {
                        lineError.Add("Не найден Spend1");
                        return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
                    }

                    var Spend2 = SpendList.FirstOrDefault(x => x.Spend2_Name.ToLower() == item.Spend2.ToLower().Trim() && x.Spend1_Id == item.Spend1Id);
                    if (Spend2 != null)
                    {
                        item.Spend2Id = Spend2.Spend2_Id;
                    }
                    else
                    {
                        lineError.Add("Не найден Spend2");
                        return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
                    }

                    var Spend3 = SpendList.FirstOrDefault(x => x.Spend3_Name.ToLower() == item.Spend3.ToLower().Trim() && x.Spend2_Id == item.Spend2Id && x.Spend1_Id == item.Spend1Id);
                    if (Spend3 != null)
                    {
                        item.Spend3Id = Spend3.Spend3_Id;
                    }
                    else
                    {
                        lineError.Add("Не найден Spend3");
                        return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
                    }

                    var Spend4 = SpendList.FirstOrDefault(x => x.Spend4_Name.ToLower() == item.Spend4.ToLower().Trim() && x.Spend3_Id == item.Spend3Id && x.Spend2_Id == item.Spend2Id && x.Spend1_Id == item.Spend1Id);
                    if (Spend4 != null)
                    {
                        item.Spend4Id = Spend4.Spend4_Id;
                    }
                    else
                    {
                        lineError.Add("Не найден Spend4");
                        return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
                    }

                    if (item.ActivityName == null)
                    {
                        lineError.Add("Не задан ActivityName");
                        return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
                    }

                    List<LoadEstimation> LoadEstimation = new List<LoadEstimation>();

                    sql = $@"SELECT
                                Id as EstimateId,
                                ActivityName,
                                EstimSum,
                                TargetSum,
                                Month_Id as MonthId,
                                Spend1_Id as Spend1Id,
                                Spend2_Id as Spend2Id,
                                Spend3_Id as Spend3Id,
                                Spend4_Id as Spend4Id

                                FROM [SpendBMI].[dbo].[Estimate]
                                WHERE ActivityName = '{item.ActivityName.Trim()}' AND Spend1_Id = {item.Spend1Id} AND Spend2_Id = {item.Spend2Id} 
                                AND Spend3_Id = {item.Spend3Id} AND Spend4_Id = {item.Spend4Id}";
                    ddm = new DirectDataManager(sql);
                    LoadEstimation = ddm.ToList<LoadEstimation>().ToList();

                    foreach (var month in MonthList)
                    {
                        item.EstimateId = 0;

                        var temp = LoadEstimation.FirstOrDefault(x => x.MonthId == month.Id);

                        if (temp != null)
                        {
                            item.EstimateId = temp.EstimateId;
                        }

                        var Estimate = new LoadEstimation
                        {
                            EstimateId = item.EstimateId,
                            ActivityName = item.ActivityName.Trim(),
                            MonthId = month.Id,
                            Spend1Id = item.Spend1Id,
                            Spend2Id = item.Spend2Id,
                            Spend3Id = item.Spend3Id,
                            Spend4Id = item.Spend4Id,
                        };

                        switch (month.Code)
                        {
                            case "Jan":
                                Estimate.EstimSum = Decimal.Parse(item.Jan);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "Feb":
                                Estimate.EstimSum = Decimal.Parse(item.Feb);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "Mar":
                                Estimate.EstimSum = Decimal.Parse(item.Mar);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "Apr":
                                Estimate.EstimSum = Decimal.Parse(item.Apr);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "May":
                                Estimate.EstimSum = Decimal.Parse(item.May);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "Jun":
                                Estimate.EstimSum = Decimal.Parse(item.Jun);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "Jul":
                                Estimate.EstimSum = Decimal.Parse(item.Jul);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "Aug":
                                Estimate.EstimSum = Decimal.Parse(item.Aug);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "Sep":
                                Estimate.EstimSum = Decimal.Parse(item.Sep);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "Oct":
                                Estimate.EstimSum = Decimal.Parse(item.Oct);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "Nov":
                                Estimate.EstimSum = Decimal.Parse(item.Nov);
                                NewEstimateList.Add(Estimate);
                                break;
                            case "Dec":
                                Estimate.EstimSum = Decimal.Parse(item.Dec);
                                NewEstimateList.Add(Estimate);
                                break;
                            default:
                                break;
                        }
                    }
                }

                StringBuilder estimStr = new StringBuilder();
                StringBuilder whereStr = new StringBuilder();

                StringBuilder newEstim = new StringBuilder();

                var sDate = DateTime.Now;

                for (int i = 0; i < NewEstimateList.Count; i++)
                {
                    var estim = NewEstimateList[i];

                    if (estim.EstimateId > 0)
                    {
                        estimStr.Append($"WHEN {estim.EstimateId} THEN {estim.EstimSum.ToString().Replace(",", ".")} ");
                        whereStr.Append("," + estim.EstimateId);
                    }
                    else
                    {
                        if (newEstim.Length > 0)
                        {
                            newEstim.Append($@", ('{sDate}','{sDate}', 1, 0,
                                                '{estim.ActivityName}', {estim.EstimSum.ToString().Replace(",", ".")}, 0, {estim.MonthId}, 
                                                {estim.Spend1Id}, {estim.Spend2Id}, {estim.Spend3Id}, {estim.Spend4Id})");
                        }
                        else
                        {
                            newEstim.Append($@"('{sDate}','{sDate}', 1, 0,
                                                '{estim.ActivityName}', {estim.EstimSum.ToString().Replace(",", ".")}, 0, {estim.MonthId}, 
                                                {estim.Spend1Id}, {estim.Spend2Id}, {estim.Spend3Id}, {estim.Spend4Id})");
                        }
                    }
                    //Каждые 50 записей производим апдейт, инсерт
                    if (i != 0 && i % 50 == 0)
                    {
                        //Если есть что обновлять - обновляем
                        if (whereStr.Length > 0)
                        {
                            UpdateEstimation(estimStr.ToString(), whereStr.ToString());
                            estimStr = new StringBuilder();
                            whereStr = new StringBuilder();
                        }
                        //Если есть что добавить - добавляем
                        if (newEstim.Length > 0)
                        {
                            InsertIntoEstimation(newEstim.ToString());
                            newEstim = new StringBuilder();
                        }
                    }
                }
                //После выхода из цикла - дозаписываем оставшие записи
                if (whereStr.Length > 0)
                {
                    UpdateEstimation(estimStr.ToString(), whereStr.ToString());
                }
                if (newEstim.Length > 0)
                {
                    InsertIntoEstimation(newEstim.ToString());
                }
            }
            catch (Exception ex)
            {
                lineError.Add(ex.Message);
            }

            return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
        }

        public void InsertIntoEstimation(string valuesInsert)
        {
            string sql = $@"BEGIN TRANSACTION;
					                INSERT INTO dbo.Estimate
                                        ([CreatedOn],[UpdatedOn],[Active],[Deleted],
                                        [ActivityName],[EstimSum],[TargetSum],[Month_Id], 
                                        [Spend1_Id],[Spend2_Id],[Spend3_Id],[Spend4_Id]) 
                                     VALUES {valuesInsert} ;COMMIT; ";
            var ddm = new DirectDataManager(sql);
            ddm.ExecuteQuery();
        }
        public void UpdateEstimation(string estim, string whereStr)
        {
            string sql = $@"UPDATE dbo.Estimate
                                SET EstimSum = CASE Id
                                {estim}
                                ELSE EstimSum END
                                WHERE Id IN (0 {whereStr})";
            var ddm = new DirectDataManager(sql);
            ddm.ExecuteQuery();
        }

        #region Parser

        public List<LoadDataViewModel> ParserClassifierType(string path)
        {
            var eda = new ExcelDataAdapter(null, new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            List<LoadDataViewModel> list = eda.ToList<LoadDataViewModel>(path);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Id = i + 1;
            }

            return list;
        }

        #endregion
    }
}