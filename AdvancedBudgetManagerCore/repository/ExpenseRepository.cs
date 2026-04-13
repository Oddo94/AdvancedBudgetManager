using AdvancedBudgetManagerCore.model.dto;
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
        private string sqlStatementGetExpenseDailyTotalsForDateInterval = @"SELECT DAY(date) AS 'Day', SUM(value) AS 'Total expenses'
                                                                            FROM expenses
                                                                            WHERE user_ID = @userId AND date BETWEEN @startDate AND @endDate
                                                                            GROUP BY DAY(date)
                                                                            ORDER BY DAY(date)";

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
                        int expenseValue = -1; ;

                        long.TryParse(expenseRow.ItemArray[0].ToString(), out expenseId);
                        long.TryParse(expenseRow.ItemArray[1].ToString(), out retrievedUserId);
                        string name = expenseRow.ItemArray[2].ToString();
                        long.TryParse(expenseRow.ItemArray[3].ToString(), out expenseType);
                        int.TryParse(expenseRow.ItemArray[4].ToString(), out expenseValue);
                        DateTime expenseDate = DateTime.Parse(expenseRow.ItemArray[5].ToString());
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

        public List<DailyExpenseTotalDto> GetDailyExpenseTotalsForDateInterval(long userId, DateTime startDate, DateTime endDate) {
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getExpenseDailyTotalsForDateIntervalCommand = new MySqlCommand(sqlStatementGetExpenseDailyTotalsForDateInterval, conn);
                    getExpenseDailyTotalsForDateIntervalCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getExpenseDailyTotalsForDateIntervalCommand.Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getExpenseDailyTotalsForDateIntervalCommand.Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getExpenseDailyTotalsForDateIntervalCommand);
                    DataTable retrievedDailyTotals = new DataTable();

                    dataAdapter.Fill(retrievedDailyTotals);

                    List<DailyExpenseTotalDto> dailyExpenseTotalList = new List<DailyExpenseTotalDto>();
                    foreach (DataRow dailyTotalRow in retrievedDailyTotals.Rows) {
                        int day = -1;
                        double totalValue = -1;

                        int.TryParse(dailyTotalRow.ItemArray[0].ToString(), out day);
                        double.TryParse(dailyTotalRow.ItemArray[1].ToString(), out totalValue);

                        DailyExpenseTotalDto currentDailyTotal = new DailyExpenseTotalDto(day, totalValue);
                        dailyExpenseTotalList.Add(currentDailyTotal);
                    }

                    return dailyExpenseTotalList;

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
