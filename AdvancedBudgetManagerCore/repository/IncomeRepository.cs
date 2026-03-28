using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.exception;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
    public class IncomeRepository : IIncomeRepository {
        private IDatabaseConnection dbConnection;
        private string sqlStatementGetIncomesByUserIdAndDateInterval = "SELECT incomeID, user_ID, name, incomeType, value, date FROM incomes WHERE user_ID = @userId AND date BETWEEN @startDate AND @endDate";

        public IncomeRepository(IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
        }

        public bool Delete(long id) {
            throw new NotImplementedException();
        }

        public IEnumerable<Income> GetAll() {
            throw new NotImplementedException();
        }

        public List<Income> GetAllLikeName(long userId, string name) {
            throw new NotImplementedException();
        }

        public Income GetById(long id) {
            throw new NotImplementedException();
        }

        public Income GetByName(long userId, string name) {
            throw new NotImplementedException();
        }

        public List<Income> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate) {
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getIncomesByUserIdAndDateIntervalCommand = new MySqlCommand(sqlStatementGetIncomesByUserIdAndDateInterval, conn);
                    getIncomesByUserIdAndDateIntervalCommand.Parameters.AddWithValue("@userId", userId);
                    getIncomesByUserIdAndDateIntervalCommand.Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getIncomesByUserIdAndDateIntervalCommand.Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getIncomesByUserIdAndDateIntervalCommand);
                    DataTable retrievedIncomes = new DataTable();

                    dataAdapter.Fill(retrievedIncomes);

                    List<Income> incomesList = new List<Income>();
                    foreach (DataRow incomeRow in retrievedIncomes.Rows) {
                        long incomeId = -1;
                        long retrievedUserId = -1;
                        string name = string.Empty;
                        long incomeType = -1;
                        int value = -1;
                        DateTime date = DateTime.MinValue;

                        long.TryParse(incomeRow.ItemArray[0].ToString(), out incomeId);
                        long.TryParse(incomeRow.ItemArray[1].ToString(), out retrievedUserId);
                        name = incomeRow.ItemArray[2].ToString();
                        long.TryParse(incomeRow.ItemArray[3].ToString(), out incomeType);
                        int.TryParse(incomeRow.ItemArray[4].ToString(), out value);
                        date = DateTime.Parse(incomeRow.ItemArray[5].ToString());
                        Income income = new Income(incomeId, userId, name, incomeType, value, date);

                        incomesList.Add(income);
                    }

                    return incomesList;

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

        public Income Insert(Income entity) {
            throw new NotImplementedException();
        }

        public Income Update(Income entity) {
            throw new NotImplementedException();
        }
    }
}
