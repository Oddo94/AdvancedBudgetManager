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
    /// <summary>
    /// Service class used for providing the aggregated data related to the user's savings.
    /// </summary>
    public class SavingQueryService {
        /// <summary>
        /// The database connection used for retrieving the data.
        /// </summary>
        private IDatabaseConnection dbConnection;

        /// <summary>
        /// The query used for retrieving the savings list.
        /// </summary>
        private string getSavingsByUserIdAndDateIntervalQuery = @"SELECT name, value, date 
                                                                FROM savings 
                                                                WHERE user_ID = @userId
                                                                AND date BETWEEN @startDate AND @endDate
                                                                ORDER BY date";

        /// <summary>
        /// The query used for retrieving the savings percentage from the total incomes, for the specified time period.
        /// </summary>
        private string getSavingsPercentageFromTotalIncomesForDateIntervalQuery = @"WITH total_savings AS (
                                                                                 SELECT COALESCE(SUM(value), 0) AS savings_sum
                                                                                 FROM savings 
                                                                                 WHERE user_ID = @userId
                                                                                 AND date BETWEEN @startDate AND @endDate),

                                                                               total_incomes AS (
                                                                                 SELECT COALESCE(SUM(value), 0) AS incomes_sum
                                                                                 FROM incomes 
                                                                                 WHERE user_ID = @userId
                                                                                 AND date BETWEEN @startDate AND @endDate)

                                                                               SELECT inc.incomes_sum,
                                                                                  CASE WHEN inc.incomes_sum > 0 
                                                                                  THEN 100 
                                                                                  ELSE 0
                                                                                  END AS incomes_percentage,
                                                                                  sav.savings_sum,
                                                                                  ROUND(COALESCE((sav.savings_sum * 100 / inc.incomes_sum), 0), 2) AS savings_percentage
                                                                               FROM total_savings sav
                                                                               CROSS JOIN total_incomes inc";

        /// <summary>
        /// The query used for retrieving the monthly savings evolution for a specified year.
        /// </summary>
        private string getMonthlySavingsEvolutionQuery = @"SELECT 
                                                                 DATE_FORMAT(date, '%M'),
	                                                             SUM(value)
                                                           FROM 
                                                                 savings
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
        /// Initializes a new instance of the <see cref="SavingQueryService"/> based on the provided <see cref="IDatabaseConnection"/> and <see cref="IUserSessionService"/>.
        /// </summary>
        /// <param name="dbConnection">The database connection used for retrieving the data.</param>
        /// <param name="userSessionService">The user session service used for retrieving the curent user's data.</param>
        public SavingQueryService([NotNull] IDatabaseConnection dbConnection, [NotNull] IUserSessionService userSessionService) {
            this.dbConnection = dbConnection;
            this.userSessionService = userSessionService;
        }

        /// <summary>
        /// Retrieves the list of savings for the time interval specified by the start date and end date.
        /// </summary>
        /// <param name="startDate">The start date of the time interval.</param>
        /// <param name="endDate">The end date of the time interval.</param>
        /// <returns>A list of <see cref="SavingDto"/> objects.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        public List<SavingDto> GetSavingsByUserIdAndDateInterval(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getSavingsByUserIdAndDateIntervalCommand = new MySqlCommand(getSavingsByUserIdAndDateIntervalQuery, conn);
                    getSavingsByUserIdAndDateIntervalCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getSavingsByUserIdAndDateIntervalCommand.Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getSavingsByUserIdAndDateIntervalCommand.Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    conn.Open();

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getSavingsByUserIdAndDateIntervalCommand);
                    DataTable retrievedSavings = new DataTable();

                    dataAdapter.Fill(retrievedSavings);

                    List<SavingDto> savingsList = new List<SavingDto>();
                    foreach (DataRow savingRow in retrievedSavings.Rows) {
                        string name = string.Empty;
                        int value = -1;
                        DateTime savingDate = DateTime.Now;

                        name = savingRow.ItemArray[0].ToString();
                        int.TryParse(savingRow.ItemArray[1].ToString(), out value);
                        DateTime.TryParse(savingRow.ItemArray[2].ToString(), out savingDate);


                        SavingDto savingDto = new SavingDto(name, value, savingDate.Date);

                        savingsList.Add(savingDto);
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

        /// <summary>
        /// Retrieves the aggregated total savings, incomes and their corresponding percentages for the time interval specified by the start date and end date.
        /// </summary>
        /// <param name="startDate">The start date of the time interval.</param>
        /// <param name="endDate">The end date of the time interval.</param>
        /// <returns>A list of <see cref="BudgetItemStatistics"/> objects.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        public List<BudgetItemStatistics> GetSavingsPercentageFromTotalIncomes(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getSavingsPercentageFromTotalIncomesForDateIntervalCommand = new MySqlCommand(getSavingsPercentageFromTotalIncomesForDateIntervalQuery, conn);
                    getSavingsPercentageFromTotalIncomesForDateIntervalCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getSavingsPercentageFromTotalIncomesForDateIntervalCommand
                        .Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getSavingsPercentageFromTotalIncomesForDateIntervalCommand
                        .Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getSavingsPercentageFromTotalIncomesForDateIntervalCommand);
                    DataTable retrievedSavingsAndIncomesStatistics = new DataTable();

                    dataAdapter.Fill(retrievedSavingsAndIncomesStatistics);

                    List<BudgetItemStatistics> savingsAndIncomesPercentageStatisticsList = new List<BudgetItemStatistics>();

                    int incomesValue = -1;
                    double incomesPercentage = 0.0;
                    int savingsValue = -1;
                    double savingsPercentage = 0.0;

                    int.TryParse(retrievedSavingsAndIncomesStatistics.Rows[0].ItemArray[0].ToString(), out incomesValue);
                    double.TryParse(retrievedSavingsAndIncomesStatistics.Rows[0].ItemArray[1].ToString(), out incomesPercentage);
                    int.TryParse(retrievedSavingsAndIncomesStatistics.Rows[0].ItemArray[2].ToString(), out savingsValue);
                    double.TryParse(retrievedSavingsAndIncomesStatistics.Rows[0].ItemArray[3].ToString(), out savingsPercentage);

                    //Populates the list only if incomes and savings were found
                    if (incomesValue > 0 && savingsValue > 0) {
                        BudgetItemStatistics incomesStatistics = new BudgetItemStatistics(BudgetItem.Income, incomesValue, incomesPercentage);
                        BudgetItemStatistics savingsStatistics = new BudgetItemStatistics(BudgetItem.Saving, savingsValue, savingsPercentage);

                        savingsAndIncomesPercentageStatisticsList.Add(incomesStatistics);
                        savingsAndIncomesPercentageStatisticsList.Add(savingsStatistics);
                    }

                    return savingsAndIncomesPercentageStatisticsList;
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
        /// Retrieves the monthly saving evolution data for a specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>A <see cref="BudgetItemMonthlyEvolutionDto"/> object.</returns>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        public BudgetItemMonthlyEvolutionDto GetMonthlySavingsEvolution(int year) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getMonthlySavingsEvolutionCommand = new MySqlCommand(getMonthlySavingsEvolutionQuery, conn);
                    getMonthlySavingsEvolutionCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getMonthlySavingsEvolutionCommand.Parameters.Add("@year", MySqlDbType.Int32).Value = year;

                    conn.Open();

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getMonthlySavingsEvolutionCommand);
                    DataTable retrievedSavingsEvolution = new DataTable();

                    dataAdapter.Fill(retrievedSavingsEvolution);

                    Dictionary<Month, int> monthlySavingsStatistics = new Dictionary<Month, int>();
                    foreach (DataRow savingsEvolutionRow in retrievedSavingsEvolution.Rows) {
                        Month month = Month.Undefined;
                        int totalSavings = -1;

                        month = MonthExtensions.GetTypeByDescription(savingsEvolutionRow.ItemArray[0].ToString());
                        int.TryParse(savingsEvolutionRow.ItemArray[1].ToString(), out totalSavings);

                        monthlySavingsStatistics.Add(month, totalSavings);
                    }

                    return new BudgetItemMonthlyEvolutionDto(BudgetItem.Saving, monthlySavingsStatistics);
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
