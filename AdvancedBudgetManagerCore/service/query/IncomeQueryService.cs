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
    public class IncomeQueryService {
        private IDatabaseConnection dbConnection;
        private string sqlStatementGetIncomesByUserIdAndDateInterval = @"SELECT inc.name, it.typeName, inc.value, inc.date
                                                                         FROM incomes inc
                                                                         INNER JOIN income_types it ON inc.incomeType = it.typeID
                                                                         WHERE inc.user_ID = @userId AND inc.date BETWEEN @startDate AND @endDate";
        private string sqlStatementGetAggregatedIncomesByCategory = @"WITH incomeCategoryStatistics AS (
                                                                      SELECT
	                                                                        it.typeName,
	                                                                        sum(inc.value) AS totalValue,
	                                                                        inc.date
                                                                      FROM
	                                                                        incomes inc
                                                                      INNER JOIN income_types it ON
	                                                                        inc.incomeType = it.typeID
                                                                      WHERE
	                                                                        inc.user_ID = @userId
	                                                                  AND inc.date BETWEEN @startDate AND @endDate
                                                                      GROUP BY
	                                                                        it.typeName) 
                                                                      SELECT
	                                                                        incomeCategoryStatistics.typeName,
	                                                                        incomeCategoryStatistics.totalValue,
	                                                                        ROUND((incomeCategoryStatistics.totalValue * 100) / SUM(incomeCategoryStatistics.totalValue) OVER (), 2) AS totalPercentage
                                                                      FROM
	                                                                        incomeCategoryStatistics";
        private string sqlStatementGetMonthlyIncomeEvolution = @"SELECT
	                                                                   DATE_FORMAT(date, '%M') AS 'Month',
	                                                                   SUM(value) AS 'Total incomes'
                                                                 FROM
	                                                                   incomes
                                                                 WHERE
	                                                                   user_ID = @userId
	                                                             AND YEAR(date) = @year
                                                                 GROUP BY
	                                                                   MONTH(date)
                                                                 ORDER BY
	                                                                   MONTH(date),
	                                                                   YEAR(date)";
        private IUserSessionService userSessionService;

        public IncomeQueryService([NotNull] IDatabaseConnection dbConnection,
            [NotNull] IUserSessionService userSessionService) {
            this.dbConnection = dbConnection;
            this.userSessionService = userSessionService;
        }

        public List<IncomeDto> GetIncomesByUserIdAndDateInterval(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getIncomesByUserIdAndDateIntervalCommand = new MySqlCommand(sqlStatementGetIncomesByUserIdAndDateInterval, conn);
                    getIncomesByUserIdAndDateIntervalCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getIncomesByUserIdAndDateIntervalCommand
                        .Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getIncomesByUserIdAndDateIntervalCommand
                        .Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    if (conn.State != ConnectionState.Open) {
                        conn.Open();
                    }

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getIncomesByUserIdAndDateIntervalCommand);
                    DataTable retrievedIncomes = new DataTable();

                    dataAdapter.Fill(retrievedIncomes);

                    List<IncomeDto> incomesList = new List<IncomeDto>();
                    foreach (DataRow incomeRow in retrievedIncomes.Rows) {
                        string name = string.Empty;
                        string incomeType = string.Empty;
                        int value = -1;
                        DateTime incomeDate = DateTime.Now;
                        //DateOnly date = DateOnly.MinValue;

                        name = incomeRow.ItemArray[0].ToString();
                        incomeType = incomeRow.ItemArray[1].ToString();
                        int.TryParse(incomeRow.ItemArray[2].ToString(), out value);
                        DateTime.TryParse(incomeRow.ItemArray[3].ToString(), out incomeDate);


                        IncomeDto incomeDto = new IncomeDto(name, incomeType, value, incomeDate.Date);

                        incomesList.Add(incomeDto);
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

        public BudgetItemCategoriesStatisticsDto GetAggregatedIncomesByCategory(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getAggregatedIncomesByCategoryCommand = new MySqlCommand(sqlStatementGetAggregatedIncomesByCategory, conn);
                    getAggregatedIncomesByCategoryCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getAggregatedIncomesByCategoryCommand
                        .Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                    getAggregatedIncomesByCategoryCommand
                        .Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getAggregatedIncomesByCategoryCommand);
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

        public BudgetItemMonthlyEvolutionDto GetMonthlyIncomeEvolution(int year) {
            long userId = userSessionService.AuthenticatedUser.UserId;

            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getMonthlyIncomeEvolutionCommand = new MySqlCommand(sqlStatementGetMonthlyIncomeEvolution, conn);
                    getMonthlyIncomeEvolutionCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                    getMonthlyIncomeEvolutionCommand.Parameters.Add("@year", MySqlDbType.Int32).Value = year;

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getMonthlyIncomeEvolutionCommand);
                    DataTable retrievedIncomeEvolution = new DataTable();

                    dataAdapter.Fill(retrievedIncomeEvolution);

                    Dictionary<Month, int> monthlyIncomeStatistics = new Dictionary<Month, int>();
                    foreach (DataRow incomeEvolutionRow in retrievedIncomeEvolution.Rows) {
                        Month month = Month.Undefined;
                        int totalIncomes = -1;

                        month = MonthExtensions.GetTypeByDescription(incomeEvolutionRow.ItemArray[0].ToString());
                        int.TryParse(incomeEvolutionRow.ItemArray[1].ToString(), out totalIncomes);

                        MonthlyStatisticsDto monthlyStatisticsDto = new MonthlyStatisticsDto(month, totalIncomes);

                        monthlyIncomeStatistics.Add(month, totalIncomes);
                    }

                    return new BudgetItemMonthlyEvolutionDto(BudgetItem.Income, monthlyIncomeStatistics);
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
