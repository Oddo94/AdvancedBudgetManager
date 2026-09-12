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
    public class DebtsQueryService {
        /// <summary>
        /// The database connection used for retrieving the data.
        /// </summary>
        private IDatabaseConnection dbConnection;

        /// <summary>
        /// The query used for retrieving the debts list.
        /// </summary>
        private string getDebtsByUserAndDateIntervalQuery = @"SELECT dbt.name, crd.creditorName, dbt.value, dbt.date
                                                              FROM debts dbt
                                                              INNER JOIN creditors crd ON dbt.creditor_ID = crd.creditorID
                                                              WHERE dbt.user_ID = @userId 
                                                              AND dbt.date BETWEEN @startDate AND @endDate
                                                              ORDER BY dbt.date";

        /// <summary>
        /// The query used for retrieving the total debts by creditor.
        /// </summary>
        private string getAggregatedDebtsByCreditorQuery = @"WITH debtsCreditorStatistics AS (
                                                        SELECT crd.creditorName, 
                                                               SUM(dbt.value) AS totalValue
                                                        FROM 
                                                               debts dbt
                                                        INNER JOIN creditors crd ON 
                                                              dbt.creditor_ID = crd.creditorID
                                                        WHERE 
                                                              dbt.user_ID = @userId
                                                        AND dbt.date BETWEEN @startDate AND @endDate
                                                        GROUP BY crd.creditorName)
                                                        SELECT 
                                                              debtsCreditorStatistics.creditorName,
                                                              debtsCreditorStatistics.totalValue,
                                                              ROUND((debtsCreditorStatistics.totalValue * 100) / SUM(debtsCreditorStatistics.totalValue) OVER(), 2) AS totalPercentage
                                                        FROM debtsCreditorStatistics";

        /// <summary>
        /// The query used for retrieving the monthly debts evolution for a specified year.
        /// </summary>
        private string getMonthlyDebtsEvolutionQuery = @"SELECT
	                                                           DATE_FORMAT(date, '%M'),
	                                                           SUM(value)
                                                         FROM
	                                                           debts
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
        /// Initializes a new instance of the <see cref="DebtsQueryService"/> based on the provided <see cref="IDatabaseConnection"/> and <see cref="IUserSessionService"/>.
        /// </summary>
        /// <param name="dbConnection">The database connection used for retrieving the data.</param>
        /// <param name="userSessionService">The user session service used for retrieving the curent user's data.</param>
        public DebtsQueryService([NotNull] IDatabaseConnection dbConnection, [NotNull] IUserSessionService userSessionService) {
            this.dbConnection = dbConnection;
            this.userSessionService = userSessionService;
        }

        /// <summary>
        /// Retrieves the list of debts for the time interval specified by the start date and end date.
        /// </summary>
        /// <param name="startDate">The start date of the time interval.</param>
        /// <param name="endDate">The end date of the time interval.</param>
        /// <returns>A list of <see cref="DebtDto"/> objects.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        public List<DebtDto> GetDebtsByUserIdAndDateInterval(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getDebtsByUserIdAndDateIntervalCommand = new MySqlCommand(getDebtsByUserAndDateIntervalQuery, conn);
                    getDebtsByUserIdAndDateIntervalCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getDebtsByUserIdAndDateIntervalCommand
                        .Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getDebtsByUserIdAndDateIntervalCommand
                        .Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    conn.Open();

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getDebtsByUserIdAndDateIntervalCommand);
                    DataTable retrievedDebts = new DataTable();

                    dataAdapter.Fill(retrievedDebts);

                    List<DebtDto> debtsList = new List<DebtDto>();
                    foreach (DataRow debtRow in retrievedDebts.Rows) {
                        string name = string.Empty;
                        string creditorName = string.Empty;
                        int value = -1;
                        DateTime debtDate = DateTime.Now;

                        name = debtRow.ItemArray[0].ToString();
                        creditorName = debtRow.ItemArray[1].ToString();
                        int.TryParse(debtRow.ItemArray[2].ToString(), out value);
                        DateTime.TryParse(debtRow.ItemArray[3].ToString(), out debtDate);


                        DebtDto incomeDto = new DebtDto(name, creditorName, value, debtDate.Date);

                        debtsList.Add(incomeDto);
                    }

                    return debtsList;
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
        /// Retrieves the aggregated total debts by creditor for the time interval specified by the start date and end date.
        /// </summary>
        /// <param name="startDate">The start date of the time interval.</param>
        /// <param name="endDate">The end date of the time interval.</param>
        /// <returns>A <see cref="BudgetItemCategoriesStatisticsDto"/> object.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        public BudgetItemCategoriesStatisticsDto GetAggregatedDebtsByCreditor(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getAggregatedDebtsByCreditorCommand = new MySqlCommand(getAggregatedDebtsByCreditorQuery, conn);
                    getAggregatedDebtsByCreditorCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getAggregatedDebtsByCreditorCommand
                        .Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getAggregatedDebtsByCreditorCommand
                        .Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getAggregatedDebtsByCreditorCommand);
                    DataTable retrievedCreditorStatistics = new DataTable();

                    dataAdapter.Fill(retrievedCreditorStatistics);

                    List<CategoryStatisticsDto> creditorStatisticsList = new List<CategoryStatisticsDto>();
                    foreach (DataRow creditorStatisticsRow in retrievedCreditorStatistics.Rows) {
                        string creditorName = string.Empty;
                        int value = -1;
                        double percentage = 0.0;

                        creditorName = creditorStatisticsRow.ItemArray[0].ToString();
                        int.TryParse(creditorStatisticsRow.ItemArray[1].ToString(), out value);
                        double.TryParse(creditorStatisticsRow.ItemArray[2].ToString(), out percentage);

                        CategoryStatisticsDto creditorStatisticsDto = new CategoryStatisticsDto(creditorName, value, percentage);

                        creditorStatisticsList.Add(creditorStatisticsDto);
                    }

                    return new BudgetItemCategoriesStatisticsDto(BudgetItem.Debt, creditorStatisticsList);
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
        /// Retrieves the monthly debt evolution data for a specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>A <see cref="BudgetItemMonthlyEvolutionDto"/> object.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        public BudgetItemMonthlyEvolutionDto GetMonthlyDebtsEvolution(int year) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getMonthlyDebtsEvolutionCommand = new MySqlCommand(getMonthlyDebtsEvolutionQuery, conn);
                    getMonthlyDebtsEvolutionCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getMonthlyDebtsEvolutionCommand.Parameters.Add("@year", MySqlDbType.Int32).Value = year;

                    conn.Open();

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getMonthlyDebtsEvolutionCommand);
                    DataTable retrievedDebtsEvolution = new DataTable();

                    dataAdapter.Fill(retrievedDebtsEvolution);

                    Dictionary<Month, int> monthlyDebtsStatistics = new Dictionary<Month, int>();
                    foreach (DataRow debtsEvolutionRow in retrievedDebtsEvolution.Rows) {
                        Month month = Month.Undefined;
                        int totalDebts = -1;

                        month = MonthExtensions.GetTypeByDescription(debtsEvolutionRow.ItemArray[0].ToString());
                        int.TryParse(debtsEvolutionRow.ItemArray[1].ToString(), out totalDebts);

                        MonthlyStatisticsDto monthlyStatisticsDto = new MonthlyStatisticsDto(month, totalDebts);

                        monthlyDebtsStatistics.Add(month, totalDebts);
                    }

                    return new BudgetItemMonthlyEvolutionDto(BudgetItem.Income, monthlyDebtsStatistics);
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
