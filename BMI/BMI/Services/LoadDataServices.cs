using BMI.Context;
using BMI.Context.Model;
using BMI.Models;
using BMI.Services.ExcelDataReader;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Linq;
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


            return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
        }

        public ResultViewModel CreateSpend4Level(List<LoadDataViewModel> list)
        {
            List<string> lineError = new List<string>();

            var spend1Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 1");
            var spend2Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 2");
            var spend3Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 3");
            var spend4Type = mDb.ClassifierTypes.GetAll().FirstOrDefault(x => x.Name == "Spend 4");

            var groupBySpend1 = list.GroupBy(x => x.Spend1).ToList();

            for (int i = 0;i < groupBySpend1.Count;i++)
            {
                var spend1Name = groupBySpend1[i].Key;

                Classifier spend1 = mDb.Classifiers.GetAll().FirstOrDefault(x => x.ClassifierType.Id == spend1Type.Id && x.Name.ToLower().Trim() == spend1Name.ToLower().Trim());

                if(spend1 != null)
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
                
                if(spend1 == null)
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

                    if(spend2 == null)
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

                        if(spend3 == null)
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

            return new ResultViewModel { LineError = lineError.Skip(0).Take(1000).ToList() };
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