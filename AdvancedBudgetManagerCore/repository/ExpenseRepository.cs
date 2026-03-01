using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.exception;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
    public class ExpenseRepository : IExpenseRepository {
        private IDatabaseConnection dbConnection;
        private string sqlStatementGetExpensesByUserIdAndDateInterval = "SELECT expenseID, user_ID, name, type, value, date FROM expenses WHERE user_ID = @userId AND date BETWEEN @startDate AND @endDate";

        public ExpenseRepository(IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
        }

        public bool Delete(long id) {
            throw new NotImplementedException();
        }

        public IEnumerable<Expense> GetAll() {
            throw new NotImplementedException();
        }

        public List<Expense> GetAllLikeName(long userId, string name) {
            throw new NotImplementedException();
        }

        public Expense GetById(long id) {
            throw new NotImplementedException();
        }

        public Expense GetByName(long userId, string name) {
            throw new NotImplementedException();
        }

        public List<Expense> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate) {
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getExpensesByUserIdAndDateIntervalCommand = new MySqlCommand(sqlStatementGetExpensesByUserIdAndDateInterval, conn);
                    getExpensesByUserIdAndDateIntervalCommand.Parameters.AddWithValue("@userId", userId);
                    getExpensesByUserIdAndDateIntervalCommand.Parameters.AddWithValue("@startDate", startDate);
                    getExpensesByUserIdAndDateIntervalCommand.Parameters.AddWithValue("@endDate", endDate);

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getExpensesByUserIdAndDateIntervalCommand);
                    DataTable retrievedExpenses = new DataTable();

                    dataAdapter.Fill(retrievedExpenses);

                    List<Expense> expensesList = new List<Expense>();
                    foreach (DataRow expenseRow in retrievedExpenses.Rows) {
                        long expenseId = -1;
                        long retrievedUserId = -1;
                        long expenseType = -1; ;
                        long expenseValue = -1; ;

                        long.TryParse(expenseRow.ItemArray[0].ToString(), out expenseId);
                        long.TryParse(expenseRow.ItemArray[1].ToString(), out retrievedUserId);
                        string name = expenseRow.ItemArray[2].ToString();
                        long.TryParse(expenseRow.ItemArray[3].ToString(), out expenseType);
                        long.TryParse(expenseRow.ItemArray[4].ToString(), out expenseValue);
                        DateTime expenseDate = DateTime.Parse(expenseRow.ItemArray[5].ToString());
                        //DateTime date = DateTime.ParseExact(expenseRow.ItemArray[5].ToString(), "yyyy-MM-dd", CultureInfo.CurrentCulture);
                        Expense expense = new Expense(expenseId, retrievedUserId, name, expenseType, expenseValue, expenseDate);

                        expensesList.Add(expense);
                    }

                    return expensesList;

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


        public Expense Insert(Expense entity) {
            throw new NotImplementedException();
        }

        public Expense Update(Expense entity) {
            throw new NotImplementedException();
        }
    }
}
