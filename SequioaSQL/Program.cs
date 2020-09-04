using SequoiaReport.Managers.DDM;
using SequoiaReport.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaReport
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionStringSequoiaKA = "SequoiaKAContext";
            string connectionStringSequioaReport = "SequoiaReportContext";
            string sql = $@"SELECT
                            mEstim.Id,
                            mEstim.EstimSum,
                            mEstim.TargetSum,
                            mEstim.FactSum
                            --mEstim.Category_Id

                            FROM
                            dbo.Estimate AS mEstim
                            WHERE mEstim.Year_Id=67 AND mEstim.Client_Id=1 AND Category_Id=146";

            DirectDataManager ddm = new DirectDataManager(sql, connectionStringSequoiaKA);
            var Estimations = ddm.ToList<Estimate>().ToList();

            bool ExistsTable = ExiststDataBase(connectionStringSequioaReport, "Estimate");

            if (!ExistsTable)
            {
                string sqlCreateTable = $@"   CREATE TABLE [dbo].[Estimate] (
                                                [Id] [bigint] IDENTITY(1,1) NOT NULL,
                                                [SequoiaKaId] [bigint] NOT NULL,
	                                            [EstimSum] [decimal](18, 2) NOT NULL,
	                                            [TargetSum] [decimal](18, 2) NOT NULL,
	                                            [FactSum] [decimal](18, 2) NOT NULL);";
                DirectDataManager ddmCreate = new DirectDataManager(sqlCreateTable, connectionStringSequioaReport);
                ddmCreate.ExecuteQuery();
            }

            //DeleteEstimate(connectionStringSequioaReport);
            //InsertIntoEstimate(connectionStringSequioaReport);
            //UpdateEstimate(connectionStringSequioaReport);

            AllOperatin(connectionStringSequioaReport);

            //Console.ReadKey();

        }

        public static bool ExiststDataBase(string ConnectionString, string TableName)
        {
            System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
            builder.ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
            string database = builder["Initial Catalog"] as string;

            string sql = $@"SELECT TOP 1 TABLE_NAME
                            FROM information_schema.TABLES
                            WHERE (TABLE_CATALOG = '{database}') AND (TABLE_NAME = '{TableName}')";
            DirectDataManager ddm = new DirectDataManager(sql, ConnectionString);
            var result = ddm.SingleObject("TABLE_NAME");

            if(result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавление записей, которых еще нет в репорте
        /// </summary>
        /// <param name="connectionString"></param>
        public static void InsertIntoEstimate(string connectionString)
        {
            string sql = $@"BEGIN TRANSACTION;
                                INSERT INTO SequoiaReport.dbo.Estimate
                                ([SequoiaKaId],
                                [EstimSum],
                                [TargetSum],
                                [FactSum])
                                SELECT mSE.Id,
                                mSE.EstimSum,
                                mSE.TargetSum,
                                mSE.FactSum
                                FROM Sequoia.dbo.Estimate AS mSE
                                WHERE NOT EXISTS (
                                SELECT 1
                                FROM SequoiaReport.dbo.Estimate AS mSR
                                WHERE mSE.Id=mSR.SequoiaKaId)
                                COMMIT;";
            DirectDataManager ddmCreate = new DirectDataManager(sql, connectionString);
            ddmCreate.ExecuteQuery();
        }

        /// <summary>
        /// Обновление записей в репорте, которые поменялись
        /// </summary>
        /// <param name="connectionString"></param>
        public static void UpdateEstimate(string connectionString)
        {
            string sql = $@"BEGIN TRANSACTION;
                            UPDATE mSR
                            SET
	                            mSR.EstimSum=mSE.EstimSum,
	                            mSR.FactSum=mSE.FactSum,
	                            mSR.TargetSum=mSE.TargetSum
                            FROM SequoiaReport.dbo.Estimate AS mSR,
                            Sequoia.dbo.Estimate AS mSE
                            WHERE mSR.SequoiaKaId=mSE.Id AND (
                            mSR.EstimSum != mSE.EstimSum OR
                            mSR.FactSum != mSE.FactSum OR
                            mSR.TargetSum != mSE.TargetSum)
                            COMMIT;";
            DirectDataManager ddmCreate = new DirectDataManager(sql, connectionString);
            ddmCreate.ExecuteQuery();
        }

        /// <summary>
        /// Удаление записей из Репорта, которых нет в основной таблицы
        /// </summary>
        /// <param name="conn"></param>
        public static void DeleteEstimate(string connectionString)
        {
            string sql = $@"BEGIN TRANSACTION;
                                DELETE mSR
                                FROM SequoiaReport.dbo.Estimate AS mSR
                                WHERE mSR.SequoiaKaId NOT IN (
                                SELECT mSE.Id
                                FROM Sequoia.dbo.Estimate AS mSE)
                                COMMIT;";
            DirectDataManager ddmCreate = new DirectDataManager(sql, connectionString);
            ddmCreate.ExecuteQuery();
        }

        public static void AllOperatin(string connectionString)
        {
            string sql = $@"EXEC all_operation";
            DirectDataManager ddmAll = new DirectDataManager(sql, connectionString);
            ddmAll.ExecuteQuery();
        }
    }
}
