using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.exception;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
    public class SavingRepository : ISavingRepository {
        private IDatabaseConnection dbConnection;
        private string sqlStatementGetSavingsByUserIdAnDateInterval = @"SELECT savingID, user_ID, name, value, date FROM savings WHERE user_ID = @userId AND date BETWEEN @startDate AND @endDate";

        public SavingRepository(IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
        }

        public bool Delete(long id) {
            throw new NotImplementedException();
        }

        public IEnumerable<Saving> GetAll() {
            throw new NotImplementedException();
        }

        public List<Saving> GetAllLikeName(long userId, string name) {
            throw new NotImplementedException();
        }

        public Saving GetById(long id) {
            throw new NotImplementedException();
        }

        public Saving GetByName(long userId, string name) {
            throw new NotImplementedException();
        }

        public List<Saving> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate) {
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getSavingsByUserIdAndDateIntervalCommand = new MySqlCommand(sqlStatementGetSavingsByUserIdAnDateInterval, conn);
                    getSavingsByUserIdAndDateIntervalCommand.Parameters.AddWithValue("@userId", userId);
                    getSavingsByUserIdAndDateIntervalCommand.Parameters.AddWithValue("@startDate", startDate);
                    getSavingsByUserIdAndDateIntervalCommand.Parameters.AddWithValue("@endDate", endDate);

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getSavingsByUserIdAndDateIntervalCommand);
                    DataTable retrievedSavings = new DataTable();

                    dataAdapter.Fill(retrievedSavings);

                    List<Saving> savingsList = new List<Saving>();
                    foreach (DataRow savingRow in retrievedSavings.Rows) {
                        long savingId = -1;
                        long retrievedUserId = -1;
                        string name = string.Empty;
                        int value = -1;
                        DateTime date = DateTime.MinValue;

                        long.TryParse(savingRow.ItemArray[0].ToString(), out savingId);
                        long.TryParse(savingRow.ItemArray[1].ToString(), out retrievedUserId);
                        name = savingRow.ItemArray[2].ToString();
                        int.TryParse(savingRow.ItemArray[3].ToString(), out value);
                        date = DateTime.Parse(savingRow.ItemArray[4].ToString());
                        Saving saving = new Saving(savingId, userId, name, value, date);

                        savingsList.Add(saving);
                    }

                    return savingsList;

                } catch (MySqlException ex) {
                    int errorCode = ex.Number;
                    String message;

                    if (errorCode == 1042) {
                        message = "Unable to connect to the database! Please check the connection and try again.";
                    } else {
                        message = "An error occurred while retrieving data! Please try again.";
                    }

                    throw new AdvancedBudgetManagerException(ExceptionCategory.Persistence, message, ex);
                }
            }
        }

        public Saving Insert(Saving entity) {
            throw new NotImplementedException();
        }

        public Saving Update(Saving entity) {
            throw new NotImplementedException();
        }
    }
}
