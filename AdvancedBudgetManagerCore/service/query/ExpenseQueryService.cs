using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.exception;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.service.query {
    /// <summary>
    /// Service class used for providing the aggregated data related to the user's expenses.
    /// </summary>
    public class ExpenseQueryService {
        /// <summary>
        /// The database connection used for retrieving the data.
        /// </summary>
        private IDatabaseConnection dbConnection;

        /// <summary>
        /// The query used for retrieving the expenses list.
        /// </summary>
        private string getExpensesByUserIdAndDateIntervalQuery = @"SELECT ex.name, et.categoryName, ex.value, ex.date
                                                                   FROM expenses ex
                                                                   INNER JOIN expense_types et ON ex.TYPE = et.categoryID
                                                                   WHERE user_ID = @userId AND date BETWEEN @startDate AND @endDate";

        /// <summary>
        /// The query used for retrieving the total expenses by category.
        /// </summary>
        private string getAggregatedExpensesByCategoryQuery = @"WITH expenseCategoryStatistics AS (
                                                                SELECT
	                                                                  et.categoryName,
	                                                                  SUM(ex.value) AS totalValue
                                                                FROM
	                                                                  expenses ex
                                                                INNER JOIN expense_types et ON
	                                                                  ex.type = et.categoryID
                                                                WHERE
	                                                                  user_ID = @userId
	                                                            AND date BETWEEN @startDate AND @endDate
                                                                GROUP BY et.categoryName) 
                                                                SELECT
                                                                      expenseCategoryStatistics.categoryName,
                                                                      expenseCategoryStatistics.totalValue,
                                                                      ROUND((expenseCategoryStatistics.totalValue * 100) / SUM(expenseCategoryStatistics.totalValue) OVER (), 2) AS totalPercentage
                                                                FROM
                                                                      expenseCategoryStatistics";

        /// <summary>
        /// The query used for retrieving the monthly expenses evolution for a specified year.
        /// </summary>
        private string getMonthlyExpensesEvolutionQuery = @"SELECT
	                                                             DATE_FORMAT(date, '%M') AS 'Month',
	                                                             SUM(value) AS 'Total expenses'
                                                           FROM
	                                                             expenses
                                                           WHERE
	                                                             user_ID = @userId
	                                                       AND YEAR(date) = @year
                                                           GROUP BY
	                                                               MONTH(date),
                                                                   DATE_FORMAT(date, '%M')
                                                           ORDER BY
	                                                               MONTH(date)";

        /// <summary>
        /// The user session service used for retrieving the curent user's data.
        /// </summary>
        private IUserSessionService userSessionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseQueryService"/> based on the provided <see cref="IDatabaseConnection"/> and <see cref="IUserSessionService"/>.
        /// </summary>
        /// <param name="dbConnection">The database connection used for retrieving the data.</param>
        /// <param name="userSessionService">The user session service used for retrieving the curent user's data.</param>
        public ExpenseQueryService([NotNull] IDatabaseConnection dbConnection, [NotNull] IUserSessionService userSessionService) {
            this.dbConnection = dbConnection;
            this.userSessionService = userSessionService;
        }

        /// <summary>
        /// Retrieves the list of expenses for the time interval specified by the start date and end date.
        /// </summary>
        /// <param name="startDate">The start date of the time interval.</param>
        /// <param name="endDate">The end date of the time interval.</param>
        /// <returns>A list of <see cref="ExpenseDto"/> objects.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        public List<ExpenseDto> GetExpensesByUserIdAndDateInterval(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getExpensesByUserIdAndDateIntervalCommand = new MySqlCommand(getExpensesByUserIdAndDateIntervalQuery, conn);
                    getExpensesByUserIdAndDateIntervalCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getExpensesByUserIdAndDateIntervalCommand
                        .Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getExpensesByUserIdAndDateIntervalCommand
                        .Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    conn.Open();

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getExpensesByUserIdAndDateIntervalCommand);
                    DataTable retrievedIncomes = new DataTable();

                    dataAdapter.Fill(retrievedIncomes);

                    List<ExpenseDto> expensesList = new List<ExpenseDto>();
                    foreach (DataRow expenseRow in retrievedIncomes.Rows) {
                        string name = string.Empty;
                        string expenseType = string.Empty;
                        int value = -1;
                        DateTime expenseDate = DateTime.Now;

                        name = expenseRow.ItemArray[0].ToString();
                        expenseType = expenseRow.ItemArray[1].ToString();
                        int.TryParse(expenseRow.ItemArray[2].ToString(), out value);
                        DateTime.TryParse(expenseRow.ItemArray[3].ToString(), out expenseDate);


                        ExpenseDto incomeDto = new ExpenseDto(name, expenseType, value, expenseDate.Date);

                        expensesList.Add(incomeDto);
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

        /// <summary>
        /// Retrieves the aggregated total expenses by category for the time interval specified by the start date and end date.
        /// </summary>
        /// <param name="startDate">The start date of the time interval.</param>
        /// <param name="endDate">The end date of the time interval.</param>
        /// <returns>A <see cref="BudgetItemCategoriesStatisticsDto"/> object.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        public BudgetItemCategoriesStatisticsDto GetAggregatedExpensesByCategory(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getAggregatedExpensesByCategoryCommand = new MySqlCommand(getAggregatedExpensesByCategoryQuery, conn);
                    getAggregatedExpensesByCategoryCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getAggregatedExpensesByCategoryCommand
                        .Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getAggregatedExpensesByCategoryCommand
                        .Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getAggregatedExpensesByCategoryCommand);
                    DataTable retrievedCategoryStatistics = new DataTable();

                    dataAdapter.Fill(retrievedCategoryStatistics);

                    List<CategoryStatisticsDto> categoryStatisticsList = new List<CategoryStatisticsDto>();
                    foreach (DataRow categoryStatisticsRow in retrievedCategoryStatistics.Rows) {
                        string name = string.Empty;
                        int value = -1;
                        double percentage = 0.0;

                        name = categoryStatisticsRow.ItemArray[0].ToString();
                        int.TryParse(categoryStatisticsRow.ItemArray[1].ToString(), out value);
                        double.TryParse(categoryStatisticsRow.ItemArray[2].ToString(), out percentage);

                        CategoryStatisticsDto categoryStatisticsDto = new CategoryStatisticsDto(name, value, percentage);

                        categoryStatisticsList.Add(categoryStatisticsDto);
                    }

                    return new BudgetItemCategoriesStatisticsDto(BudgetItem.Income, categoryStatisticsList);
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

        /// <summary>
        /// Retrieves the monthly expense evolution data for a specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>A <see cref="BudgetItemMonthlyEvolutionDto"/> object.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        public BudgetItemMonthlyEvolutionDto GetMonthlyExpensesEvolution(int year) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getMonthlyExpensesEvolutionCommand = new MySqlCommand(getMonthlyExpensesEvolutionQuery, conn);
                    getMonthlyExpensesEvolutionCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getMonthlyExpensesEvolutionCommand.Parameters.Add("@year", MySqlDbType.Int32).Value = year;

                    conn.Open();

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getMonthlyExpensesEvolutionCommand);
                    DataTable retrievedExpensesEvolution = new DataTable();

                    dataAdapter.Fill(retrievedExpensesEvolution);

                    Dictionary<Month, int> monthlyExpensesStatistics = new Dictionary<Month, int>();
                    foreach (DataRow expensesEvolutionRow in retrievedExpensesEvolution.Rows) {
                        Month month = Month.Undefined;
                        int totalExpenses = -1;

                        month = MonthExtensions.GetTypeByDescription(expensesEvolutionRow.ItemArray[0].ToString());
                        int.TryParse(expensesEvolutionRow.ItemArray[1].ToString(), out totalExpenses);

                        MonthlyStatisticsDto monthlyStatisticsDto = new MonthlyStatisticsDto(month, totalExpenses);

                        monthlyExpensesStatistics.Add(month, totalExpenses);
                    }

                    return new BudgetItemMonthlyEvolutionDto(BudgetItem.Income, monthlyExpensesStatistics);
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

    }
}
