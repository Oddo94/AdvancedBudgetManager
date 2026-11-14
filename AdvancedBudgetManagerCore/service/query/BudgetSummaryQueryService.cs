using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.exception;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.service.query {
    public class BudgetSummaryQueryService {
        /// <summary>
        /// The database connection used for retrieving the data.
        /// </summary>
        private IDatabaseConnection dbConnection;

        /// <summary>
        /// The query used for retrieving the budget summary statistics for the specified time interval.
        /// </summary>
        private string getBudgetSummaryByUserIdAndDateIntervalQuery = @"WITH income_statistics AS (
                                                                                 SELECT
                                                                                     'Income' AS budget_item_type,
                                                                                     COALESCE(SUM(value), 0) AS total_value,
                                                                                     CASE
                                                                                         WHEN SUM(value) > 0 THEN 100
                                                                                         ELSE 0
                                                                                     END AS total_percentage
                                                                                 FROM incomes
                                                                                 WHERE
                                                                                     user_ID = @userId
                                                                                     AND date BETWEEN @startDate AND @endDate
                                                                             ),
                                                                             expense_statistics AS (
                                                                                 SELECT
                                                                                     'Expense' AS budget_item_type,
                                                                                     COALESCE(SUM(value), 0) AS total_value
                                                                                 FROM expenses
                                                                                 WHERE
                                                                                     user_ID = @userId
                                                                                     AND date BETWEEN @startDate AND @endDate
                                                                             ),
                                                                             debt_statistics AS (
                                                                                 SELECT
                                                                                     'Debt' AS budget_item_type,
                                                                                     COALESCE(SUM(value), 0) AS total_value
                                                                                 FROM debts
                                                                                 WHERE
                                                                                     user_ID = @userId
                                                                                     AND date BETWEEN @startDate AND @endDate
                                                                             ),
                                                                             saving_statistics AS (
                                                                                 SELECT
                                                                                     'Saving' AS budget_item_type,
                                                                                     COALESCE(SUM(value), 0) AS total_value
                                                                                 FROM savings
                                                                                 WHERE
                                                                                     user_ID = @userId
                                                                                     AND date BETWEEN @startDate AND @endDate
                                                                             )
                                                                             SELECT
                                                                                 inc_stat.budget_item_type,
                                                                                 inc_stat.total_value,
                                                                                 inc_stat.total_percentage
                                                                             FROM income_statistics inc_stat

                                                                             UNION ALL

                                                                             SELECT
                                                                                 exp_stat.budget_item_type,
                                                                                 exp_stat.total_value,
                                                                                 COALESCE(
                                                                                     ROUND(
                                                                                         (exp_stat.total_value * 100 / NULLIF(inc_stat.total_value, 0)),
                                                                                         2
                                                                                     ),
                                                                                     0
                                                                                 ) AS total_percentage
                                                                             FROM expense_statistics exp_stat, income_statistics inc_stat

                                                                             UNION ALL

                                                                             SELECT
                                                                                 dbt_stat.budget_item_type,
                                                                                 dbt_stat.total_value,
                                                                                 COALESCE(
                                                                                     ROUND(
                                                                                         (dbt_stat.total_value * 100 / NULLIF(inc_stat.total_value, 0)),
                                                                                         2
                                                                                     ),
                                                                                     0
                                                                                 ) AS total_percentage
                                                                             FROM debt_statistics dbt_stat, income_statistics inc_stat

                                                                             UNION ALL

                                                                             SELECT
                                                                                 sav_stat.budget_item_type,
                                                                                 sav_stat.total_value,
                                                                                 COALESCE(
                                                                                     ROUND(
                                                                                         (sav_stat.total_value * 100 / NULLIF(inc_stat.total_value, 0)),
                                                                                         2
                                                                                     ),
                                                                                     0
                                                                                 ) AS total_percentage
                                                                             FROM saving_statistics sav_stat, income_statistics inc_stat";

        /// <summary>
        /// The query used for retrieving the daily expense totals for the specified time interval.
        /// </summary>
        private string getDailyExpenseTotalsForDateIntervalQuery = @"SELECT DAY(date) AS 'Day', SUM(value) AS 'Total expenses'
                                                                            FROM expenses
                                                                            WHERE user_ID = @userId AND date BETWEEN @startDate AND @endDate
                                                                            GROUP BY DAY(date)
                                                                            ORDER BY DAY(date)";

        /// <summary>
        /// The user session service used for retrieving the curent user's data.
        /// </summary>
        private IUserSessionService userSessionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetSummaryQueryService"/> based on the provided <see cref="IDatabaseConnection"/> and <see cref="IUserSessionService"/>.
        /// </summary>
        /// <param name="dbConnection">The database connection used for retrieving the data.</param>
        /// <param name="userSessionService">The user session service used for retrieving the curent user's data.</param>
        public BudgetSummaryQueryService([NotNull] IDatabaseConnection dbConnection, [NotNull] IUserSessionService userSessionService) {
            this.dbConnection = dbConnection;
            this.userSessionService = userSessionService;
        }

        /// <summary>
        /// Retrieves the aggregated total incomes, expense, debts, savings and their corresponding percentages for the time interval specified by the start date and end date.
        /// </summary>
        /// <param name="startDate">The start date of the time interval.</param>
        /// <param name="endDate">The end date of the time interval.</param>
        /// <returns>A list of <see cref="BudgetItemStatistics"/> objects.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>

        public List<BudgetItemStatistics> GetBudgetSummaryByUserIdAndDateInterval(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getBudgetSummaryByUserIdAndDateIntervalCommand = new MySqlCommand(getBudgetSummaryByUserIdAndDateIntervalQuery, conn);
                    getBudgetSummaryByUserIdAndDateIntervalCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getBudgetSummaryByUserIdAndDateIntervalCommand.Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getBudgetSummaryByUserIdAndDateIntervalCommand.Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getBudgetSummaryByUserIdAndDateIntervalCommand);
                    DataTable retrievedBudgetSummaryStatistics = new DataTable();

                    dataAdapter.Fill(retrievedBudgetSummaryStatistics);

                    List<BudgetItemStatistics> budgetSummaryStatistics = new List<BudgetItemStatistics>();

                    foreach (DataRow statisticsRow in retrievedBudgetSummaryStatistics.Rows) {
                        BudgetItem budgetItem = BudgetItem.Undefined;
                        int totalValue = -1;
                        double totalPercentage = 0.0;

                        budgetItem = BudgetItemExtensions.GetTypeByDescription(statisticsRow.ItemArray[0].ToString());
                        int.TryParse(statisticsRow.ItemArray[1].ToString(), out totalValue);
                        double.TryParse(statisticsRow.ItemArray[2].ToString(), out totalPercentage);

                        BudgetItemStatistics budgetItemStatistics = new BudgetItemStatistics(budgetItem, totalValue, totalPercentage);

                        budgetSummaryStatistics.Add(budgetItemStatistics);
                    }

                    return budgetSummaryStatistics;
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
        /// Retrieves the monthly expense evolution data for a specified time interval.
        /// </summary>
        /// <param name="startDate">The start date of the time interval.</param>
        /// <param name="endDate">The end date of the time interval.</param>
        /// <returns>A <see cref="BudgetItemDailyEvolutionDto"/> object.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        public BudgetItemDailyEvolutionDto GetDailyExpenseEvolution(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getDailyExpenseTotalsForDateIntervalCommand = new MySqlCommand(getDailyExpenseTotalsForDateIntervalQuery, conn);
                    getDailyExpenseTotalsForDateIntervalCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getDailyExpenseTotalsForDateIntervalCommand.Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getDailyExpenseTotalsForDateIntervalCommand.Parameters.Add("@endDate", MySqlDbType.Date).Value = startDate;

                    conn.Open();

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getDailyExpenseTotalsForDateIntervalCommand);
                    DataTable retrievedExpensesEvolution = new DataTable();

                    dataAdapter.Fill(retrievedExpensesEvolution);

                    Dictionary<int, int> dailyExpensesStatistics = new Dictionary<int, int>();
                    foreach (DataRow expensesEvolutionRow in retrievedExpensesEvolution.Rows) {
                        int day = -1;
                        int totalExpenses = -1;

                        int.TryParse(expensesEvolutionRow.ItemArray[0].ToString(), out day);
                        int.TryParse(expensesEvolutionRow.ItemArray[1].ToString(), out totalExpenses);

                        dailyExpensesStatistics.Add(day, totalExpenses);
                    }

                    return new BudgetItemDailyEvolutionDto(BudgetItem.Expense, dailyExpensesStatistics);
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
